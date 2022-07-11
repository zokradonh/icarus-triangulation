using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace IcarusTriangulation;

public class MapGridView : ObservableObject
{
    private double _gridWidth;
    public ObservableCollection<Line> MapGridLines { get; } = new();
    private MapGrid _gridSettings = new();

    public MapGrid GridSettings
    {
        get => _gridSettings;
        set => SetProperty(ref _gridSettings, value);
    }
    
    /// <summary>
    /// Same as GridSettings GridWidth
    /// </summary>
    public double GridWidth
    {
        get => _gridWidth;
        set
        {
            if (SetProperty(ref _gridWidth, value))
            {
                for (int i = 0; i < 16; i++)
                {
                    Line horizontalLine;
                    if (MapGridLines.Count() <= i * 2)
                    {
                        horizontalLine = new Line()
                        {
                            Stroke = Brushes.Yellow,
                            StrokeThickness = 1
                        };
                        MapGridLines.Add(horizontalLine);
                    }
                    else
                    {
                        horizontalLine = MapGridLines[i * 2];
                    }
                    horizontalLine.X1 = 0 + GridSettings.GridStartEdge.X;
                    horizontalLine.Y1 = i*GridWidth + GridSettings.GridStartEdge.Y;
                    
                    horizontalLine.X2 = GridWidth * 16 + GridSettings.GridStartEdge.X;
                    horizontalLine.Y2 = i*GridWidth + GridSettings.GridStartEdge.Y;
                    
                    Line verticalLine;
                    if (MapGridLines.Count() <= i * 2 + 1)
                    {
                        verticalLine = new Line()
                        {
                            Stroke = Brushes.SkyBlue,
                            StrokeThickness = 1
                        };
                        MapGridLines.Add(verticalLine);
                    }
                    else
                    {
                        verticalLine = MapGridLines[i * 2 + 1];
                    }
                    verticalLine.X1 = i*GridWidth + GridSettings.GridStartEdge.X;
                    verticalLine.X2 = i*GridWidth + GridSettings.GridStartEdge.X;
                    verticalLine.Y1 = 0 + GridSettings.GridStartEdge.Y;
                    verticalLine.Y2 = GridWidth * 16 + GridSettings.GridStartEdge.Y;

                    if (i == 0)
                    {
                        Debug.WriteLine($"({horizontalLine.X1}|{horizontalLine.Y1})->({horizontalLine.X2}|{horizontalLine.Y2})");
                    }
                }
            }
        }
    }
}