using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplayManager : MonoBehaviour
{
    private List<List<int>> map; // grid of cells containing country IDs, type int for now
    private Dictionary<int, GameObject> countries = new Dictionary<int, GameObject>(); // map from country ID to game object

    [SerializeField] private GameObject countryTilemap; // tilemap prefab to be instantiated for each country
    [SerializeField] private GameObject grid; // grid object, parent to all tilemaps

    // Start is called before the first frame update
    void Awake()
    {
        // to delete: initialise map
        map = new List<List<int>>();
        map.Add(new List<int>{0, 1, 1, 0, 1, 1, 0});
        map.Add(new List<int>{1, 1, 1, 0, 1, 1, 1});
        map.Add(new List<int>{1, 1, 1, 0, 3, 3, 1});
        map.Add(new List<int>{0, 0, 0, 0, 3, 3, 0});
        map.Add(new List<int>{0, 0, 2, 2, 2, 3, 0});
        map.Add(new List<int>{0, 0, 0, 2, 2, 3, 0});

        // loop over map cells
        foreach (List<int> row in map) {
            foreach (int countryID in row) {
                if (!countries.ContainsKey(countryID)) {
                    // instantiate a tilemap for the country
                    GameObject newCountry = Instantiate(countryTilemap, new Vector3(0, 0, 0), Quaternion.identity, grid.transform);
                    newCountry.name = countryID.ToString();
                    newCountry.GetComponent<CountryTilemap>().MapDisplayManager = gameObject;
                    newCountry.GetComponent<CountryTilemap>().countryID = countryID;
                    // Debug.Log(newCountry.GetComponent<CountryTilemap>().MapDisplayManager);
                    countries.Add(countryID, newCountry);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<List<int>> GetMap() {
        return map;
    }
}
