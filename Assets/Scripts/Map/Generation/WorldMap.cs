using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

public class WorldMap {
    private readonly MapCell[,] _cells;

    public WorldMap(MapCell[,] cells) {
        _cells = cells;
    }

    public int Width => _cells.GetLength(1);
    public int Height => _cells.GetLength(0);
    public int Area => Width * Height;
    
    public MapCell At(int x, int y) {
        return _cells[y, x];
    }
}