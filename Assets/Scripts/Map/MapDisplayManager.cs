using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplayManager : MonoBehaviour
{
    private List<List<int>> map; // grid of cells containing country IDs, type int for now
    private int scaleFactor; // scale factor for the map

    private Dictionary<int, GameObject> countries = new Dictionary<int, GameObject>(); // map from country ID to game object

    [SerializeField] private GameObject countryTilemap; // tilemap prefab to be instantiated for each country
    [SerializeField] private GameObject grid; // grid object, parent to all tilemaps

    private Vector2Int gridSize; // shape of the grid in cells
    private Vector3Int startCoord; // coordinate of the top left cell in the grid

    // Start is called before the first frame update
    void Awake()
    {
        // to delete: initialise map
        // map = new List<List<int>>();
        // map.Add(new List<int>{0, 1, 1, 0, 5, 5, 0});
        // map.Add(new List<int>{1, 1, 1, 0, 5, 5, 5});
        // map.Add(new List<int>{1, 1, 1, 0, 3, 3, 5});
        // map.Add(new List<int>{0, 0, 0, 0, 3, 3, 0});
        // map.Add(new List<int>{0, 0, 2, 2, 2, 3, 0});
        // map.Add(new List<int>{0, 0, 0, 2, 2, 3, 0});
        // map.Add(new List<int>{0, 0, 0, 0, 0, 3, 0});

        // to delete: randomly initialise map
        GenerateRandomMap();

        // adjust grid based on shape of input
        AdjustGrid();

        // loop over map cells
        foreach (List<int> row in map) {
            foreach (int countryID in row) {
                if (!countries.ContainsKey(countryID)) {
                    // instantiate a tilemap for the country
                    GameObject newCountry = Instantiate(countryTilemap, new Vector3(0, 0, 0), Quaternion.identity, grid.transform);
                    newCountry.name = countryID.ToString();

                    newCountry.GetComponent<CountryTilemap>().MapDisplayManager = gameObject;
                    newCountry.GetComponent<CountryTilemap>().countryID = countryID;

                    countries.Add(countryID, newCountry);
                }
            }
        }

        // generate and assign colours
        int k = countries.Count;
        List<Color> colours = GenerateColours(k);
        int i = 0;
        foreach (int countryID in countries.Keys) {
            countries[countryID].GetComponent<CountryTilemap>().countryColour = colours[i];
            i++;
        }
    }

    // to delete: generate random noise as a map for testing purposes
    void GenerateRandomMap() {
        scaleFactor = Random.Range(10, 50); // change to 10, 100 later
        // scaleFactor = 1;
        map = new List<List<int>>();

        for (int row = 0; row < scaleFactor * 9; row++) {
            List<int> newRow = new List<int>();

            for (int col = 0; col < scaleFactor * 16; col++) {
                newRow.Add(Random.Range(0, 5));
            }

            map.Add(newRow);
        }

        gridSize = new Vector2Int(map[0].Count, map.Count);
    }
    
    // generates k random colours that look (more or less) good together - totally stole the idea off this btw
    // https://martin.ankerl.com/2009/12/09/how-to-create-random-colors-programmatically/
    List<Color> GenerateColours(int k) {
        List<Color> colours = new List<Color>();
        
        float golden_ratio_conjugate = 0.618033988749895f;
        float h = Random.Range(0f, 1f); // use random start value
        
        for (int i = 0; i < k; i++) {
            colours.Add(Color.HSVToRGB(h, 0.4f, 0.95f));
            h += golden_ratio_conjugate;
            h %= 1;
        }

        return colours;
    }

    // resize grid cell size based on map size. always 16:9 ratio
    void AdjustGrid() {
        // by default, scale of (1, 1) yields a 10x18 grid i think
        float newScale = 1f / scaleFactor; // yields a non exact fit but reduces chance of going out of frame

        // may scrap this: cap the in-screen cell resolution so cells don't get too small?
        newScale = Mathf.Max(newScale, 0.1f);

        grid.transform.localScale = new Vector3(newScale, newScale, 0);

        // calculate start coordinate
        startCoord = new Vector3Int(0, 0, 0);
        startCoord.x = (int) -Mathf.Floor(gridSize.x / 2);
        startCoord.y = (int) Mathf.Floor(gridSize.y / 2);
    }

    public List<List<int>> GetMap() {
        return map;
    }

    public Vector3Int GetStartCoord() {
        return startCoord;
    }
}
