using System;
using System.Runtime.ConstrainedExecution;
using Unity.Burst.CompilerServices;

public struct MapCell : IEquatable<MapCell> {
    private int raw;

    public bool IsWater => raw == 0;

    public bool IsCapitalCity => raw < 0;

    public bool IsLand => raw != 0;

    public int? CountryIndex => IsWater ? null : Math.Abs(raw) - 1;

    public bool Equals(MapCell other) => raw == other.raw;

    public override bool Equals(object obj) => obj is MapCell other && raw == other.raw;

    public override int GetHashCode() => raw.GetHashCode();

    public static bool operator ==(MapCell lhs, MapCell rhs) => lhs.Equals(rhs);
    public static bool operator !=(MapCell lhs, MapCell rhs) => !(lhs == rhs);

    public static readonly MapCell WATER = new MapCell{raw = 0};

    public static MapCell CapitalCity(int countryIndex) => new MapCell{raw = ~countryIndex};

    public static MapCell CountryCell(int countryIndex) => new MapCell{raw = countryIndex + 1};
}