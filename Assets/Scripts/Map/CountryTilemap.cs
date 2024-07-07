using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CountryTilemap : MonoBehaviour
{
    private Tilemap tilemap; // the tilemap component of this country
    [SerializeField] private Tile fillTile; // tile to fill the country's tilemap with

    // the following probably should not be public but this is the life we live
    public int countryID; // the id representing this country in the map
    public Color countryColour; // the colour with which the country is represented on the map

    private GameObject mapDisplayManager; // handles the generation of all tilemaps
    public GameObject MapDisplayManager { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();

        List<List<int>> map = MapDisplayManager.GetComponent<MapDisplayManager>().GetMap();
        Vector3Int startCoord = MapDisplayManager.GetComponent<MapDisplayManager>().GetStartCoord();

        Debug.Log($"start coordinate: {startCoord}");

        // iterate over each tile and colour it 
        for (int i = 0; i < map.Count; i++) {
            for (int j = 0; j < map[0].Count; j++) {
                if (map[i][j] == countryID) {
                    int x = startCoord.x + j;
                    int y = startCoord.y - i;

                    fillCell(new Vector3Int(x, y, 0), countryColour);
                    // Debug.Log($"filled cell at {x}, {y}");
                }
            }
        }
    }

    // fill the cell at `position` with a tile of solid colour `color`
    void fillCell(Vector3Int position, Color color) {
        // fill the tile at `position` with some designated tile
        tilemap.SetTile(position, fillTile); 

        // note that flags must be turned off right before setting colour!!
        tilemap.SetTileFlags(position, TileFlags.None);
        tilemap.SetColor(position, color);
    }

    // OnMouseDown is called when the user has pressed the mouse button while over the Collider.
    // .... for some reason it works with Box Collider 2D but not Tilemap Collider 2D
    void OnMouseDown() {
        Debug.Log($"clicked on {gameObject.name}");
        // TODO: perform action associated with selecting the country associated with this tilemap
    }
}
