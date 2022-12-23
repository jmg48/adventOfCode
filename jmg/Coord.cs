namespace AdventOfCode;
public record Coord(int X, int Y)
{
    public static Coord operator +(Coord a, Coord b) => new(a.X + b.X, a.Y + b.Y);
}

public record Coord<T>(T X, T Y);
