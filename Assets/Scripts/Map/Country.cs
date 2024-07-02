using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class Country : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private string countryName;

    [SerializeField] private float technology;
    [SerializeField] private float aggressiveness;
    [SerializeField] private float hunger;
    [SerializeField] private float fertility;

    [SerializeField] private List<Country> connectingCountries;


}
