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
