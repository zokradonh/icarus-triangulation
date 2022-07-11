namespace IcarusTriangulation;

public class GridCalibration
{
    public LineEquation Slope { get; set; }
    public LineEquation Offset { get; set; }

    public static GridCalibration DefaultCalibration = new()
    {
        Slope = new LineEquation(0.057029703f, -0.066138614f),
        Offset = new LineEquation(-0.348884836f, 7.156023971f)
    };

    public AngleCalibration CreateCalibration(double gridWidth)
    {
        return new AngleCalibration()
        {
            Equation = new LineEquation(Slope.Calc(gridWidth), Offset.Calc(gridWidth)),
            Name = $"Gen{gridWidth:F1}"
        };
    }
    
    public GridCalibration()
    {
        
    }
}