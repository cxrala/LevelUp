using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    // TODO: generate the countries
    private HashSet<Country> countries = new HashSet<Country>();
    private Dictionary<(Country, Country), double> capitalCityDistances;

    private void GenerateGraph() {
        // generate graph here/extrapolate this function to new class.
        // update countries
    }

    private void UpdateCapitalCityDistances() {
        // update capitalCityDistances
    }

    public void TechnologyUpdated(Country country) {
        foreach (Country neighbour in country.NeighbourCountries) {
            if (capitalCityDistances[(country, neighbour)] <= country.TechnologyDistance) {
                if (!country.NeighbourCountries.Contains(neighbour)) { // can remove this check if we make it a set.
                    country.NeighbourCountries.Add(neighbour);
                    Debug.Log("Successfully added the country!");
                }
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        GenerateGraph();
        UpdateCapitalCityDistances();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
