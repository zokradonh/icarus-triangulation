using System.Windows;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace IcarusTriangulation;

public class AngleCalibration : ObservableObject
{
    
    public static readonly AngleCalibration DefaultAngleCalibration = new()
    {
        Equation = LineEquation.FromTwoPoints(84.3, 572.97, 112.1, 774.3),
        Name = "DefaultZ9"
    };
    
    private string _name;
    private LineEquation _equation;

    public LineEquation Equation
    {
        get => _equation;
        set => SetProperty(ref _equation, value);
    }

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public override string ToString()
    {
        return Equation.ToString();
    }

    public static AngleCalibration FromMeasurement(double angle1, double distance1, double angle2, double distance2)
    {
        return new AngleCalibration()
        {
            Equation = LineEquation.FromTwoPoints(angle1, distance1, angle2, distance2)
        };
    }

    public static AngleCalibration FromAngles(AngleMeasure measure1, AngleMeasure measure2, Point foundDeposit)
    {
        return FromMeasurement(
            measure1.Angle, 
            (foundDeposit - measure1.StartPoint).Length,
            measure2.Angle,
            (foundDeposit - measure2.StartPoint).Length
            );
    }
}