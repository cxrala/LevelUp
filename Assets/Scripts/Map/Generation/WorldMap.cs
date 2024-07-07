using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

public class WorldMap {
    private readonly MapCell[,] _cells;
    private readonly HashSet<(int, int)> _edges;

    public WorldMap(MapCell[,] cells, HashSet<(int, int)> edges) {
        _cells = cells;
        _edges = edges;
    }

    public int Width => _cells.GetLength(1);
    public int Height => _cells.GetLength(0);
    public int Area => Width * Height;

    public ISet<(int, int)> Edges => _edges;
    
    public MapCell At(int x, int y) {
        return _cells[y, x];
    }
}