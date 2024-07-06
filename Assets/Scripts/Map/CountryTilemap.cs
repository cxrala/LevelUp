using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CountryTilemap : MonoBehaviour
{
    private Tilemap tilemap; // the tilemap component of this country
    [SerializeField] private Tile fillTile; // tile to fill the country's tilemap with
    
    private Vector3Int tilemapSize; // size of the tilemap
    private Vector3Int minCoord; // minimum coordinate of the tilemap

    // a binary map with 1 where the country is and 0 elsewhere
    // for now, assume 1-to-1 correspondence between mask elements and the tilemap cells
    private List<List<int>> mapMask = new List<List<int>>();

    private Color countryColour; // the colour with which the country is represented on the map

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();

        // get bounds
        // Debug.Log($"original cell bounds: {tilemap.cellBounds.min}, {tilemap.cellBounds.max}");
        tilemapSize = tilemap.cellBounds.size;
        minCoord = tilemap.cellBounds.min;

        // to delete: define the mask
        mapMask.Add(new List<int>{0, 1, 1, 0, 1, 1, 0});
        mapMask.Add(new List<int>{1, 1, 1, 1, 1, 1, 1});
        mapMask.Add(new List<int>{1, 1, 1, 1, 1, 1, 1});
        mapMask.Add(new List<int>{0, 1, 1, 1, 1, 1, 0});
        mapMask.Add(new List<int>{0, 0, 1, 1, 1, 0, 0});
        mapMask.Add(new List<int>{0, 0, 0, 1, 0, 0, 0});


        // to delete: define the colour
        countryColour = Color.green;

        // iterate over each tile and colour it 
        for (int i = 0; i < mapMask.Count; i++) {
            for (int j = 0; j < mapMask[0].Count; j++) {
                if (mapMask[i][j] == 1) {
                    int x = minCoord.x + j;
                    x = (x >= 0) ? x - 1 : x;

                    int y = minCoord.y + tilemapSize.y - i;
                    y = (y >= 0) ? y - 1 : y;

                    fillCell(new Vector3Int(x, y, 0), countryColour);
                    // Debug.Log($"filled cell at {x}, {y}");
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

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
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = tilemap.WorldToCell(mouseWorldPos);

        // check if a tile was clicked on
        if (tilemap.HasTile(cellPos)) {
            Debug.Log($"clicked on {gameObject.name}");
            // TODO: perform action associated with selecting the country associated with this tilemap
        }
    }
}
