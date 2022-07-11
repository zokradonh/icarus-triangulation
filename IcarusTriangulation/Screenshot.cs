using System.Collections.ObjectModel;
using System.Windows.Media;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace IcarusTriangulation;

public class Screenshot : ObservableObject
{
    private ImageSource? _image;
    private MapGridView _mapGridView;

    public ImageSource? Image
    {
        get => _image;
        set => SetProperty(ref _image, value);
    }
    
    public MapGridView MapGridView
    {
        get => _mapGridView;
        set => SetProperty(ref _mapGridView, value);
    }

    public AngleCalibration DefaultCalibration { get; set; } = AngleCalibration.DefaultAngleCalibration;
    
    public ObservableCollection<AngleMeasure> AngleMeasures { get; } = new();
    
    
    
}