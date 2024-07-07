using System;
using System.Linq;

public interface IMapGen {
    public WorldMap GenerateWorld(int width, int height, int countries);
}