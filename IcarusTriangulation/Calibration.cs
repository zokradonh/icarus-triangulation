using System.Windows;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using PointF = System.Drawing.PointF;

namespace IcarusTriangulation;

public class Calibration : ObservableObject
{
    
    public static readonly Calibration DefaultCalibration = new()
    {
        P1 = new PointF(84.3f, 572.97f),
        P2 = new PointF(112.1f, 774.3f),
        Name = "DefaultZ9"
    };
    
    private PointF _p1;
    private string _name;
    private PointF _p2;

    public PointF P1
    {
        get => _p1;
        set {
            if (SetProperty(ref _p1, value))
            {
                OnPropertyChanged(nameof(DistanceSlope));
                OnPropertyChanged(nameof(DistanceOffset));
            }
        }
    }

    public PointF P2
    {
        get => _p2;
        set
        {
            if (SetProperty(ref _p2, value))
            {
                OnPropertyChanged(nameof(DistanceSlope));
                OnPropertyChanged(nameof(DistanceOffset));
            }
        }
    }

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public double DistanceSlope => (P2.Y - P1.Y) / (P2.X - P1.X);

    public double DistanceOffset => P1.Y - DistanceSlope * P1.X;

    public override string ToString()
    {
        return $"y = {DistanceSlope:F2}x {DistanceOffset:+ #.##;- #.##;0}";
    }

    public static Calibration FromMeasurement(float angle1, float distance1, float angle2, float distance2)
    {
        return new Calibration()
        {
            P1 = new PointF(angle1, distance1),
            P2 = new PointF(angle2, distance2)
        };
    }

    public static Calibration FromAngles(AngleMeasure measure1, AngleMeasure measure2, Point foundDeposit)
    {
        return FromMeasurement(
            (float) measure1.Angle, 
            (float) (foundDeposit - measure1.StartPoint).Length,
            (float) measure2.Angle,
            (float) (foundDeposit - measure2.StartPoint).Length
            );
    }
}