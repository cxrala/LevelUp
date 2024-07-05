using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using Unity.Mathematics;
using System.Linq;
using System;

public class Country : MonoBehaviour
{
    
    [SerializeField] private string countryName;

    private Aggressiveness _aggressiveness = new Aggressiveness(0.2);
    private Technology _technology = new Technology(0.2);
    private Fertility _fertility = new Fertility();
    private Food _food = new Food();
    private Population _population;


    [SerializeField] private Country[] countries; // all the countries -- to refactor to a graph class
    [SerializeField] private List<Country> neighbourCountries;

    private bool isInWar = false;
    public string CountryName { get => countryName; set => countryName = value; }
    public double Aggressiveness {
        get => _aggressiveness.AggressivenessValue;
    }
    public double Technology {
        get => _technology.TechnologyValue;
    }
    public Population Population {
        get => _population;
    }
    
    private void Start() {
        countries = GetCountries();
        neighbourCountries = new List<Country>();
        _population = Population.GetDefaultPopulation(_fertility, _food, UnityEngine.Random.Range(0, 100));
        StartCoroutine(_fertility.DecreaseFertility());
        StartCoroutine(_food.UpdateFood());
        StartCoroutine(_population.SimulatePopulationChange());
    }

    private IEnumerator WarInitiator() {
        while (true) {
            if (!isInWar) {
                int randomIndex = UnityEngine.Random.Range(0,neighbourCountries.Count() - 1);
                Country attackedCountry = neighbourCountries[randomIndex];
                bool isWarInitiated = _aggressiveness.RollWar(this, attackedCountry);
                if (isWarInitiated) {
                    isInWar = true;
                    StartCoroutine(_aggressiveness.InitiateWar(this, attackedCountry));
                }
                yield return new WaitForSeconds(UnityEngine.Random.Range(8, 20));
            }
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

    private void ConnectCountries() { //Adds/Removes connecting countries from connectingCountries
        for (int i = 0; i < countries.Length; i++) {
            if (countries[i] != this) { 
                float distance = Vector3.Distance(transform.position,countries[i].transform.position);

                if (distance < _technology.GetTechnologyRadius()) {
                    if (!neighbourCountries.Contains(countries[i])) {
                        neighbourCountries.Add(countries[i]);
                    }
                } 
                else {
                    
                    if (neighbourCountries.Contains(countries[i])) {
                        neighbourCountries.Remove(countries[i]);
                    }
                }
            }
        }
    }

    public Country[] GetCountries() {
        return FindObjectsOfType<Country>();
    }
}
