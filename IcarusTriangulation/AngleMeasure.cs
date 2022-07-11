using System;
using System.Windows;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Point = System.Windows.Point;

namespace IcarusTriangulation;

public class AngleMeasure : ObservableObject
{

    public AngleCalibration AngleCalibration
    {
        get => _angleCalibration;
        set
        {
            if (SetProperty(ref _angleCalibration, value))
            {
                OnPropertyChanged(nameof(TriangulationDiameter));
                OnPropertyChanged(nameof(Left));
                OnPropertyChanged(nameof(Top));
            }
        }
    }


    public AngleMeasure? RespecifyParent { get; set; }
    
    private Point _startPoint;
    private Point _l1;
    private Point _l2;
    private bool _isComplete;
    private AngleCalibration _angleCalibration;

    public Point StartPoint
    {
        get => _startPoint;
        set => SetProperty(ref _startPoint, value);
    }

    public Point L1
    {
        get => _l1;
        set => SetProperty(ref _l1, value);
    }

    public Point L2
    {
        get => _l2;
        set
        {
            if (SetProperty(ref _l2, value))
            {
                OnPropertyChanged(nameof(Angle));
                OnPropertyChanged(nameof(TriangulationDiameter));
                OnPropertyChanged(nameof(Left));
                OnPropertyChanged(nameof(Top));
            }
        }
    }


    public bool IsComplete
    {
        get => _isComplete;
        set
        {
            if (SetProperty(ref _isComplete, value))
            {
                OnPropertyChanged(nameof(Angle));
                OnPropertyChanged(nameof(TriangulationDiameter));
                OnPropertyChanged(nameof(Left));
                OnPropertyChanged(nameof(Top));
            }
        }
    }

    public Vector V1 => new Vector(L1.X - StartPoint.X, L1.Y - StartPoint.Y);
    public Vector V2 => new Vector(L2.X - StartPoint.X, L2.Y - StartPoint.Y);
    // return the angle in degree, not radiant
    public double Angle => Math.Acos((V1 * V2) / (V1.Length * V2.Length)) * 180 / Math.PI;
    public double TriangulationDiameter => AngleCalibration.Equation.Calc(Angle) * 2;
    public double Left => StartPoint.X - TriangulationDiameter / 2;
    public double Top => StartPoint.Y - TriangulationDiameter / 2;
    
    public AngleMeasure()
    {
        _angleCalibration = AngleCalibration.DefaultAngleCalibration;
    }
}