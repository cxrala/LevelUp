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
    [SerializeField] private int countryPopulation;
    [SerializeField] private float faith; //faith always less that country population
    [SerializeField, Range(0f, 1f)] private float technology;
    [SerializeField, Range(0f, 1f)] private float aggressiveness;
    [SerializeField, Range(0f, 1f)] private float hunger;
    [SerializeField, Range(0f, 1f)] private float fertility;

    [SerializeField] private Country[] countries;
    [SerializeField] private List<Country> connectingCountries;

    private const float warWaitTime = 30;
    [SerializeField] private float warRollWaitTime = 5f;
    private float warRollCountdown = 0f;
    private bool inWar;

    private void Start() {
        countries = GetCountries();
        connectingCountries = new List<Country>();
        
    }


    private void Update() {
        ConnectCountries();
        WarCountdown();
    }
    private void WarCountdown() { //War can only occur after a fixed wait time (will be determined by aggressiveness)
        if (warRollCountdown <= 0f && aggressiveness >= 0.8 && !inWar && connectingCountries != null) {
            StartCoroutine(RollWar());
            warRollCountdown = warRollWaitTime;
        }
        else
        {
            warRollCountdown -= Time.deltaTime; 
        }
    }
    private void ConnectCountries() { //Adds/Removes connecting countries from connectingCountries
        for (int i = 0; i < countries.Length; i++) {
            if (countries[i] != this) { 
                float distance = Vector3.Distance(transform.position,countries[i].transform.position);

                if (distance < TechnologyRadius()) {
                    if (!connectingCountries.Contains(countries[i])) {
                        connectingCountries.Add(countries[i]);
                    }
                } 
                else {
                    
                    if (connectingCountries.Contains(countries[i])) {
                        connectingCountries.Remove(countries[i]);
                    }
                }
            }
        }
    }
    private float TechnologyRadius() {
        return 10; //technology * 100;
    }

    public Country[] GetCountries() {
        return FindObjectsOfType<Country>();
    }

    public IEnumerator RollWar() { //Does the war logic
        Debug.Log("Rolling War");
        float probabilityOfWar = Math.Min(aggressiveness*aggressiveness*aggressiveness,1);
        float randomFloat = UnityEngine.Random.Range(0,100)/100;
        if (randomFloat < probabilityOfWar && connectingCountries != null) {
            if(connectingCountries != null) {
                int randomIndex = UnityEngine.Random.Range(0,connectingCountries.Count()-1);
                Country attackedCountry = connectingCountries[randomIndex];
                Debug.Log("War Started between" + attackedCountry.countryName + countryName);
                yield return new WaitForSeconds(1);
                //initiate war
                inWar = true;
                //Do the ui stuff
                int populationInWar = countryPopulation/4;
                //temporarily subtract 25% of population
                yield return new WaitForSeconds(10);

                randomFloat = UnityEngine.Random.Range(0,100)/100;
                if (randomFloat < 0.7f) // win
                {
                    Debug.Log("War Ended: " + countryName + " won");
                    //add percentage of hunger
                    //add percentage of faith
                    //subtract 50% of people sent to war
                    //add believers to country
                }
                else // loss
                {
                    Debug.Log("War Ended: " + attackedCountry.countryName + " won");
                    //decrease hunger
                    //decrease faith if god helped war
                    //subtract 100% of people sent to war
                    //believers of country not added
                }
                inWar = false;
            }
        }
    }
}
