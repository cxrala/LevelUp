using System;
using System.Collections.Generic;
using System.Linq;

public class MapGen : IMapGen
{
    private static readonly double MAGIC_BACKTRACK_CONSTANT = 5;
    private static readonly int MAGIC_SWITCH_ALGO_AT = 10_000_000;

    private readonly Random rng;

    public MapGen(Random rng) => this.rng = rng;

    private double randomAngle() {
        double angle;
        do {
            angle = rng.NextDouble() * (2 * Math.PI);
        } while (angle == 0 || angle == Math.PI || angle == Math.PI / 2 || angle == 3 * Math.PI / 2);
        return angle;
    }

    public WorldMap GenerateWorld(int width, int height, int countries)
    {
        var grid = new MapCell[height, width];
        for (int y = 0; y < height; ++y) {
            for (int x = 0; x < width; ++x) {
                grid[y, x] = MapCell.WATER;
            }
        }
        var capitals = new (double x, double y)[countries];
        double rowHeight = height / (double) countries;
        for (int i = 0; i < countries; ++i) {
            capitals[i].x = rng.NextDouble() * width;
            capitals[i].y = rowHeight * i + rng.NextDouble() * rowHeight;
            if (grid[(int) capitals[i].y, (int) capitals[i].x] != MapCell.WATER) {
                --i;
                continue;
            }
            grid[(int) capitals[i].y, (int) capitals[i].x] = MapCell.CapitalCity(i);
        }
        var workingCentres = new (double x, double y)[countries];
        for (int i = 0; i < countries; ++i) {
            workingCentres[i] = capitals[i];
        }
        var points = new List<(int x, int y)>[countries];
        for (int i = 0; i < countries; ++i) {
            points[i] = new List<(int x, int y)>();
            points[i].Add(((int) capitals[i].x, (int) capitals[i].y));
        }
        int filled = countries;
        int country = 0;
        int iterations = 0;
        while (filled < width * height) {
            if (iterations == MAGIC_SWITCH_ALGO_AT) {
                var unfilled = new List<(int x, int y)>();
                for (int y = 0; y < height; ++y) {
                    for (int x = 0; x < width; ++x) {
                        if (grid[y, x] == MapCell.WATER) {
                            unfilled.Add((x, y));
                        }
                    }
                }
                while (unfilled.Any()) {
                    int index = rng.Next(unfilled.Count);
                    var source = unfilled[index];
                    var ray2 = new Ray(source.x + rng.NextDouble(), source.y + rng.NextDouble(), randomAngle(), rng);
                    var prev = (x: source.x, y: source.y);
                    foreach (var square in ray2.Squares()) {
                        if (square.x >= width || square.x < 0 || square.y >= height || square.y < 0) {
                            break;
                        }
                        MapCell countryStatus = grid[square.y, square.x];
                        if (countryStatus != MapCell.WATER) {
                            grid[prev.y, prev.x] = MapCell.CountryCell((int) countryStatus.CountryIndex);
                            index = unfilled.FindIndex(sq => sq == prev);
                            unfilled[index] = unfilled.Last();
                            unfilled.RemoveAt(unfilled.Count - 1);
                            break;
                        }
                        prev = square;
                    }
                }
                break;
            }
            ++country;
            country %= countries;

            var ray = new Ray(workingCentres[country].x, workingCentres[country].y, randomAngle(), rng);
            foreach (var square in ray.Squares()) {
                if (square.x >= width || square.x < 0 || square.y >= height || square.y < 0) {
                    break;
                }
                var prev = grid[square.y, square.x];
                if (prev == MapCell.CountryCell(country) || prev == MapCell.CapitalCity(country)) {
                    continue;
                }
                if (prev != MapCell.WATER) {
                    break;
                }
                grid[square.y, square.x] = MapCell.CountryCell(country);
                ++filled;
                points[country].Add(square);
                int newCentreIndex = points[country].Count - 1;
                while (newCentreIndex > 0 && rng.NextDouble() > 1 / MAGIC_BACKTRACK_CONSTANT) {
                    --newCentreIndex;
                }
                var newCentre = points[country][newCentreIndex];
                workingCentres[country].x = newCentre.x + rng.NextDouble();
                workingCentres[country].y = newCentre.y + rng.NextDouble();
                break;
            }
            ++iterations;
        }
        return new WorldMap(grid);
    }
}