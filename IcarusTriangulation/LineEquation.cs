namespace IcarusTriangulation;

public class LineEquation
{
    public double Slope { get; set; }
    public double Offset { get; set; }

    public double Calc(double x)
    {
        return x * Slope + Offset;
    }

    public LineEquation(double slope, double offset)
    {
        Slope = slope;
        Offset = offset;
    }
    
    public static LineEquation FromTwoPoints(double x1, double y1, double x2, double y2)
    {
        var slope = (y2 - y1) / (x2 - x1);
        return new LineEquation(slope, y1 - slope * x1);
    }
    
    public override string ToString()
    {
        return $"y = {Slope:F2}x {Offset:+ #.##;- #.##;0}";
    }
}