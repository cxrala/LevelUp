using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelieverProperties : MonoBehaviour
{
    [SerializeField] private long believerCount = 0;
    private long nextBelieverMilestone = 0;
    private long milestones = 0;

    [SerializeField] public GameObject manaBar;
    private ManaProperties manaBarProps;

    // Start is called before the first frame update
    void Start()
    {
        manaBarProps = manaBar.GetComponent<ManaProperties>();
    }

    void Update()
    // Update is called once per frame
    {
        
    }

    public void UpdateBelieverCount(int amount) {
        long newBelieverCount = believerCount + amount;
        if (newBelieverCount > nextBelieverMilestone) {
            Debug.Log("Next milestone achieved!");
            milestones++;
            manaBarProps.UpgradeManaBar();
            UpdateNextBelieverMilestone(believerCount);
            Debug.Log(nextBelieverMilestone);
        }
        believerCount = newBelieverCount;
        Debug.Log(believerCount);
    }

    private void UpdateNextBelieverMilestone(long believerCount) {
        // detect overflow
        if (Math.Pow(2, milestones) < 0) {
            nextBelieverMilestone = long.MaxValue;
            return;
        }
        nextBelieverMilestone = (long) Math.Pow(2, milestones) + 100;
    }
}
