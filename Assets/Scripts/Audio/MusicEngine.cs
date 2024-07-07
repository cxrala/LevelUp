using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public enum Tag {
    Intro, Melodic, NonMelodic, Special, Harp, Guitar, Oboe, Saxophone, Horn, Flute, Violin
}

[Serializable]
public enum Chord {
    A, Am, Bb, Bbm, B, Bm, C, Cm, Db, CSm, D, Dm, Eb, Ebm, E, Em, F, Fm, FS, FSm, G, Gm, Ab, GSm,
}

[Serializable]
public class NextGroup {
    public string next;
    public double weighting;

    public NextGroup(string next, double weighting) { this.next = next; this.weighting = weighting; }
}

[Serializable]
public struct NextTag {
    public Tag tag;
    public double weighting;
}

[Serializable]
public struct ChordTerm {
    public Chord chord;
    public double beats;
}

[Serializable]
public class ChordProgression {
    public List<ChordTerm> chords;

    public bool Match(ChordProgression other) => Match(chords, other.chords);

    public static bool Match(List<ChordTerm> a, List<ChordTerm> b) {
        if (a.Count != b.Count) return false;
        for (int i = 0; i < a.Count; ++i) {
            if (a[i].chord != b[i].chord || a[i].beats != b[i].beats) return false;
        }
        return true;
    }

}

[Serializable]
public struct TrackData {
    public AudioClip clip;
    public List<string> groups;
    public List<Tag> tags;
    public List<NextGroup> nextGroups;
    public List<NextTag> nextTags;
    public ChordProgression chordProgression;
    public Chord next;

    public double bpm;
    public double beats;
    public double upbeats;
}

public class TrackComponent {
    public readonly TrackData coreData;
    public readonly double bps;
    public readonly double upbeatLength;
    public readonly double segmentLength;
    public readonly IDictionary<Tag, double> nextTags;
    public readonly List<NextGroup> normalisedNextGroups;

    public TrackComponent(TrackData data) {
        coreData = data;
        bps = data.bpm / 60;
        upbeatLength = data.upbeats / bps;
        segmentLength = data.beats / bps;
        nextTags = new Dictionary<Tag, double>();
        foreach(var nextTag in data.nextTags) {
            nextTags[nextTag.tag] = nextTag.weighting;
        }
        double totalWeight = 0;
        normalisedNextGroups = new();
        foreach(var nextGroup in data.nextGroups) {
            double exped = Math.Exp(nextGroup.weighting);
            totalWeight += exped;
            normalisedNextGroups.Add(new NextGroup(nextGroup.next, exped));
        }
        foreach(var nextGroup in normalisedNextGroups) {
            nextGroup.weighting /= totalWeight;
        }
    }

    public double getWeighting(IDictionary<Tag, double> nextTags) {
        double totalWeight = 0;
        foreach(var nextTag in nextTags) {
            if (coreData.tags.Contains(nextTag.Key)) {
                totalWeight += nextTag.Value;
            }
        }
        return Math.Exp(totalWeight);
    }

    public Chord GetFirstChord() => coreData.chordProgression.chords[0].chord;
}

[Serializable]
public class AudioPair {
    static double delay = 0.1;

    public AudioSource audio1;
    public AudioSource audio2;
    private int playing = 0;
    private AudioSource[] audios;

    public void Initialise() { audios = new AudioSource[] { audio1, audio2 }; }

    public void Play(AudioClip clip, double time) {
        audios[playing].clip = clip;
        audios[playing].PlayScheduled(time + delay);
        playing = 1 - playing;
    }
}

public class MusicEngine : MonoBehaviour
{
    public List<TrackData> trackData;
    public List<TrackComponent> trackComponents = new();
    public Dictionary<string, List<TrackComponent>> tracksByGroup = new();
    public AudioPair mainAudio;
    public AudioPair[] compAudio;
    public double startTime;
    public double nextStartTime;
    public int playing = 0;
    public TrackComponent current;
    public TrackComponent next;
    int started = 0;
    double buffer = 0.5;
    TrackComponent[] complementQueue;
    double[] complementNextStart;

    // Start is called before the first frame update
    void Start()
    {
        mainAudio.Initialise();
        foreach(var data in trackData) {
            TrackComponent comp = new(data);
            trackComponents.Add(comp);
            foreach(var group in comp.coreData.groups) {
                if (tracksByGroup.TryGetValue(group, out var list)) {
                    list.Add(comp);
                } else {
                    tracksByGroup[group] = new List<TrackComponent>() { comp };
                }
            }
        }
        next = trackComponents[0];

        foreach(var comp in compAudio) {
            comp.Initialise();
        }
        complementQueue = new TrackComponent[compAudio.Length];
        complementNextStart = new double[compAudio.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if (started < 3) {
            started++; 
            if (started == 3) {
                started++;
                nextStartTime = AudioSettings.dspTime;
                startTime = nextStartTime;
                current = null;
                PlayNext();
            }
        }
        else if (AudioSettings.dspTime > nextStartTime - buffer) {
            PlayNext();
        }
    }

    void SelectNext() {
        string nextGroup = current.normalisedNextGroups[0].next;
        double choice = UnityEngine.Random.Range(0, 1);
        foreach(var nextChoice in current.normalisedNextGroups) {
            choice -= nextChoice.weighting;
            if (choice < 0) {
                nextGroup = nextChoice.next;
                break;
            }
        }
        var nextTracks = tracksByGroup[nextGroup];
        List<double> nextWeightings = new();
        double totalWeight = 0;
        foreach(var track in nextTracks) {
            nextWeightings.Add(track.getWeighting(current.nextTags));
            totalWeight += nextWeightings[^1];
        }
        next = nextTracks[0];
        choice = UnityEngine.Random.Range(0, (float)totalWeight);
        for(int i = 0; i < nextTracks.Count; ++i) {
            choice -= nextWeightings[i];
            if (choice < 0) {
                next = nextTracks[i];
                break;
            }
        }
    }

    public void reselectNext(int i) {
        reselectNext(trackComponents[i]);
    }

    void reselectNext(TrackComponent component) {
        next = component;
        nextStartTime = startTime + (current?.segmentLength ?? 0) - (next?.upbeatLength ?? 0);
    }

    void PlayNext() {
        mainAudio.Play(next.coreData.clip, nextStartTime);
        startTime += (current?.segmentLength ?? 0);
        playing = 1 - playing;
        current = next;
        SelectNext();
        nextStartTime = startTime + (current?.segmentLength ?? 0) - (next?.upbeatLength ?? 0);
    }

    void PlayComplement(int i) {
        if (complementQueue[i] is not null) {
            compAudio[i].Play(complementQueue[i].coreData.clip, complementNextStart[i]);
        }
    }
}
