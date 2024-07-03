using System.Collections.Generic;
using UnityEngine;

public class Country : MonoBehaviour
{
    [SerializeField] private string countryName;
    [SerializeField] private int countryPopulation;

    [SerializeField, Range(0f, 1f)] private float technology;
    [SerializeField, Range(0f, 1f)] private float aggressiveness;
    [SerializeField, Range(0f, 1f)] private float hunger;
    [SerializeField, Range(0f, 1f)] private float fertility;
    [SerializeField] private Country[] countries;
    [SerializeField] private List<Country> connectingCountries;

    private void Start() {
        countries = GetCountries();
        connectingCountries = new List<Country>(); 
    }

    private void Update() {
        for (int i = 0; i < countries.Length; i++) {
            if (countries[i] != this) { 
                Vector3 dirVector = transform.position - countries[i].transform.position;
                float distance = dirVector.magnitude;

                if (distance < 10) {
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
        return technology * 100;
    }

    public Country[] GetCountries() {
        return FindObjectsOfType<Country>();
    }
}
