using System.Collections;
using System;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using System.ComponentModel;

public class Country : MonoBehaviour
{
    [SerializeField] private string countryName;
    [SerializeField] private int countryPopulation;

    [SerializeField, Range(0f, 1f)] private float technology;
    [SerializeField, Range(0f, 1f)] private float aggressiveness;
    [SerializeField, Range(0f, 1f)] private float hunger;
    [SerializeField, Range(0f, 1f)] private float fertility;

    [SerializeField] private Country[] countryList;
    [SerializeField] private List<Country> connectingCountries;
    
    private void Start() {
        countryList = FindObjectsOfType<Country>();
    }


}
