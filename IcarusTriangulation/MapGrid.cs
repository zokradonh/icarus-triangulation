using System;
using System.Windows;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace IcarusTriangulation;

public class MapGrid : ObservableObject
{
    private Point _gridStartEdge;
    private Point _gridEndEdge;
    public double GridWidth => Math.Abs(GridEndEdge.X - GridStartEdge.X);

    public Point GridStartEdge
    {
        get => _gridStartEdge;
        set => SetProperty(ref _gridStartEdge, value);
    }

    public Point GridEndEdge
    {
        get => _gridEndEdge;
        set => SetProperty(ref _gridEndEdge, value);
    }
}