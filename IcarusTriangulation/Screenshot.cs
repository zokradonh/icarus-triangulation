using System.Collections.ObjectModel;
using System.Windows.Media;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace IcarusTriangulation;

public class Screenshot : ObservableObject
{
    private ImageSource? _image;

    public ImageSource? Image
    {
        get => _image;
        set => SetProperty(ref _image, value);
    }

    public ObservableCollection<AngleMeasure> AngleMeasures { get; } = new();

    public int ZoomLevel { get; set; }
}