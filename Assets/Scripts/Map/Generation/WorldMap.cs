using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

public class WorldMap {
    private readonly MapCell[] _cells;
    private readonly int _width;
    private readonly int _height;

    public WorldMap(int width, int height, MapCell[] cells) {
        _width = width;
        _height = height;
        _cells = cells;
    }

    public int Width => _width;
    public int Height => _height;
    public int Area => Width * Height;
    
    public MapCell At(int x, int y) {
        return _cells[_width * y + x];
    }
}