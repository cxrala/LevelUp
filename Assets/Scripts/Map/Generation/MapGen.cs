using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;

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
                            unfilled.RemoveAtSwapBack(index);
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
        int[] countriesArr = new int[countries];
        for (int i = 0; i < countries; ++i) {
            countriesArr[i] = i;
        }
        int remainingWaterCountries = countries;
        while (remainingWaterCountries > countries / 2) {
            int makeLand = rng.Next(remainingWaterCountries);
            --remainingWaterCountries;
            (countriesArr[makeLand], countriesArr[remainingWaterCountries]) = (countriesArr[remainingWaterCountries], countriesArr[makeLand]);
        }
        int?[] substitutions = new int?[countries];
        for (int i = remainingWaterCountries; i < countries; ++i) {
            substitutions[countriesArr[i]] = i - remainingWaterCountries;
        }
        for (int y = 0; y < height; ++y) {
            for (int x = 0; x < width; ++x) {
                int? substitution = substitutions[(int) grid[y, x].CountryIndex];
                if (substitution == null) {
                    grid[y, x] = MapCell.WATER;
                } else if (grid[y, x].IsCapitalCity) {
                    grid[y, x] = MapCell.CapitalCity((int) substitution);
                } else {
                    grid[y, x] = MapCell.CountryCell((int) substitution);
                }
            }
        }
        return new WorldMap(grid, ComputeGraph(grid));
    }

    private static HashSet<(int?, int?)> ComputeGraph(MapCell[,] grid) {
        int height = grid.GetLength(0);
        int width = grid.GetLength(1);
        bool[,] visited = new bool[height, width];
        var stack = new List<(int, int, int)>();
        stack.Add((0, 0, 0));
        var edges = new HashSet<(int?, int?)>();
        do {
            var (xp, yp, d) = stack.Last();
            visited[yp, xp] = true;
            var (x, y) = (xp, yp);
            switch (d) {
                case 0:
                    --x;
                    break;
                case 1:
                    ++x;
                    break;
                case 2:
                    --y;
                    break;
                case 3:
                    ++y;
                    break;
            }
            if (x >= 0 && x < width && y >= 0 && y < height) {
                int? currIndex = grid[y, x].CountryIndex;
                int? prevIndex = grid[yp, xp].CountryIndex;
                if (currIndex != prevIndex) {
                    edges.Add((currIndex, prevIndex));
                    edges.Add((prevIndex, currIndex));
                }
                if (!visited[y, x]) {
                    stack.Add((x, y, 0));
                    continue;
                }
            }
            while (stack.Any()) {
                var (xl, yl, dl) = stack.Last();
                if (dl == 3) {
                    stack.RemoveAt(stack.Count - 1);
                } else {
                    stack[stack.Count - 1] = (xl, yl, dl + 1);
                    break;
                }
            }
        } while (stack.Any());
        return edges;
    }
}