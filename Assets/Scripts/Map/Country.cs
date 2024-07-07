using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using Unity.Mathematics;
using System.Linq;
using System;

public class Country : MonoBehaviour
{

    private string countryName;

    private Aggressiveness _aggressiveness;
    private Technology _technology;
    private Fertility _fertility;
    private Food _food;
    private Population _population;

    [SerializeField] private HashSet<Country> neighbourCountries;

    private bool isInWar = false;
    public string CountryName { get => countryName; set => countryName = value; }
    public double Aggressiveness {
        get => _aggressiveness.AggressivenessValue;
    }
    public double Technology {
        get => _technology.TechnologyValue;
    }
    public double TechnologyDistance {
        get => _technology.GetTechnologyRadius();
    }
    public double Fertility {
        get => _fertility.FertilityValue;
    }
    public double Food {
        get => GetSigmoid((double) (_food.FoodValue - PopulationCount) / 1000);
    }
    private double GetSigmoid(double val) {
        return 1 / (1 + Math.Exp(-val));
    }
    public Population Population {
        get => _population;
    }
    public long PopulationCount {
        get => _population.PopulationCount;
    }
    public double FaithProportion {
        get => _population.FaithProportion;
    }

    public HashSet<Country> NeighbourCountries => neighbourCountries;

    public void Init(
        Graph graph,
        HashSet<Country> neighbours,
        string countryName,
        bool isProphetDeployedHere
    ) {
        this.countryName = countryName;
        _aggressiveness = new Aggressiveness(UnityEngine.Random.Range(0.2f, 0.5f));
        _technology = new Technology(UnityEngine.Random.Range(0.2f, 0.5f), this, graph);
        _fertility = new Fertility(UnityEngine.Random.Range(0.5f, 0.8f));
        _food = new Food();
        long randomPopulation = UnityEngine.Random.Range(100, 300);
        if (isProphetDeployedHere) {
            _population = Population.GetProphetPopulation(_fertility, _food, randomPopulation);
        } else {
            _population = Population.GetDefaultPopulation(_fertility, _food, UnityEngine.Random.Range(100, 300));
        }
        neighbourCountries = neighbours;
    }
    
    private void Start() {
        Init(null, new HashSet<Country>(), UnityEngine.Random.Range(0, 1000).ToString(), false);
        StartCoroutine(_fertility.DecreaseFertility());
        StartCoroutine(_food.UpdateFood());
        StartCoroutine(_population.SimulatePopulationChange());
        StartCoroutine(WarInitiator());
        StartCoroutine(DebugLogCountryInfo());
        StartCoroutine(_technology.UpdateTechnology());
        StartCoroutine(_aggressiveness.UpdateAggressiveness());
    }

    private IEnumerator WarInitiator() {
        while (true) {
            if (!isInWar && neighbourCountries.Count != 0) {
                Debug.Log("Rolling for war");
                int randomIndex = UnityEngine.Random.Range(0,neighbourCountries.Count() - 1);
                Country attackedCountry = neighbourCountries.ToList()[randomIndex];
                bool isWarInitiated = _aggressiveness.RollWar(this, attackedCountry);
                if (isWarInitiated && this != attackedCountry) {
                    isInWar = true;
                    StartCoroutine(_aggressiveness.InitiateWar(this, attackedCountry));
                }
            }
            yield return new WaitForSeconds(UnityEngine.Random.Range(8, 20));
        }
    }

    public void CompleteWar(long numSurvivors, bool warWon) {
        if (_population.FaithProportion > 0 && warWon) _population.IncFaith(0.25);
        if (_population.FaithProportion < 0 && !warWon) _population.DecFaith(0.8);
        _population.AddPopulation(numSurvivors);
        isInWar = false;
    }


    private void Update() {
        // ConnectCountries(); refactor out of this class
    }

    public Country[] GetCountries() {
        return FindObjectsOfType<Country>();
    }


    private IEnumerator DebugLogCountryInfo() {
        while (true) {
            Dictionary<string, string> countryStats = GetCountryStats();
            countryStats["Time"] = UnityEngine.Time.fixedTime.ToString();
            Debug.Log(ToDebugString(countryStats));
            yield return new WaitForSeconds(10);
        }
    }

    private Dictionary<string, string> GetCountryStats() {
        Dictionary<string, string> countryStatDict = new Dictionary<string, string>
        {
            { "Name", countryName },
            { "Aggressiveness", Aggressiveness.ToString() },
            { "In War", isInWar.ToString() },
            { "Fertility", _fertility.FertilityValue.ToString() },
            { "Food", _food.FoodValue.ToString() },
            { "Population Count", PopulationCount.ToString() },
            { "Faith Proportion", FaithProportion.ToString() },
            { "TechnologyValue", Technology.ToString() }
        };
        return countryStatDict;
    }

    private string ToDebugString (Dictionary<string, string> dictionary)
    {
        return "{" + string.Join(",\n", dictionary.Select(kv => kv.Key + "=" + kv.Value).ToArray()) + "}";
    }

    public ITrait GodBlessing(string godName) {
        switch (godName){
            case "ares":
                return _aggressiveness;
            case "athena":
                return _technology;
            case "aphrodite":
                return _fertility;
            case "demeter":
                return _food;
        }
        throw new ArgumentException();
    }
}
