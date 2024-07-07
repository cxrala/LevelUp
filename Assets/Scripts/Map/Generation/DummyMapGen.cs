using System;

public class DummyMapGen : IMapGen
{
    public WorldMap GenerateWorld(int width, int height, int countries)
    {
        if (height < 100 || width < 100) {
            throw new System.NotImplementedException();
        }
        var cells = new MapCell[height, width];
        for (int y = 0; y < height; ++y) {
            for (int x = 0; x < width; ++x) {
                cells[y, x] = MapCell.WATER;
            }
        }
        for (int y = 0; y < 100; ++y) {
            for (int x = 0; x < 100; ++x) {
                cells[y, x] = MapCell.CountryCell(0);
            }
        }
        cells[50, 50] = MapCell.CapitalCity(0);
        return new WorldMap(cells);
    }
}