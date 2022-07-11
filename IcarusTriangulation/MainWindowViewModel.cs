using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace IcarusTriangulation;

public class MainWindowViewModel : ObservableRecipient
{
    private AngleMeasure? _currentMeasure;
    private Modes _mode;
    private Screenshot? _currentScreenshot;
    public ObservableCollection<AngleCalibration> Calibrations { get; } = new();
    public ObservableCollection<Screenshot> Screenshots { get; } = new();
    public ObservableCollection<AngleMeasure> SelectedMeasures { get; } = new();

    public Screenshot? CurrentScreenshot
    {
        get => _currentScreenshot;
        set => SetProperty(ref _currentScreenshot, value);
    }

    public AngleMeasure? CurrentMeasure
    {
        get => _currentMeasure;
        set => SetProperty(ref _currentMeasure, value);
    }

    public Modes Mode
    {
        get => _mode;
        set => SetProperty(ref _mode, value);
    }
    
    public RelayCommand NewMeasureCommand => new(NewMeasure);
    public RelayCommand ImportBaseImageCommand => new(ImportBaseImage);
    public RelayCommand<AngleMeasure> RemoveMeasureCommand => new(RemoveMeasure);
    public RelayCommand<AngleMeasure> RespecifyMeasureCommand => new(RespecifyMeasure);
    public RelayCommand<Screenshot> RemoveScreenshotCommand => new(RemoveScreenshot);
    public RelayCommand<AngleCalibration> RemoveCalibrationCommand => new(RemoveCalibration);
    public RelayCommand<AngleCalibration> AssignCalibrationCommand => new(AssignCalibration);
    public RelayCommand<Screenshot> CalibrateGridCommand => new(CalibrateGrid);

    public RelayCommand NewCalibrationCommand { get; }

    public MainWindowViewModel()
    {
        NewCalibrationCommand = new RelayCommand(NewCalibration, CanCreateNewCalibration);
        NewCalibrationCommand.NotifyCanExecuteChanged();
        
        Calibrations.Add(AngleCalibration.DefaultAngleCalibration);
    }

    private void CalibrateGrid(Screenshot? obj)
    {
        if (CurrentScreenshot == null) return;
        
        CurrentScreenshot.MapGridView = new MapGridView();
        Mode = Modes.GridEdge;
    }

    public void AssignCalibration(AngleCalibration? calibration)
    {
        if (CurrentScreenshot != null && calibration != null)
        {
            foreach (var currentScreenshotAngleMeasure in CurrentScreenshot.AngleMeasures)
            {
                currentScreenshotAngleMeasure.AngleCalibration = calibration;
            }

            CurrentScreenshot.DefaultCalibration = calibration;
        }
    }
    
    private bool CanCreateNewCalibration()
    {
        return SelectedMeasures.Count() == 2;
    }

    private void NewCalibration()
    {
        if (CurrentScreenshot == null) 
            return;

        Mode = Modes.Calibrate;
    }

    private void RemoveCalibration(AngleCalibration? obj)
    {
        if (obj != null) Calibrations.Remove(obj);
    }

    private void RemoveScreenshot(Screenshot? obj)
    {
        if (obj != null) Screenshots.Remove(obj);
    }

    private void RespecifyMeasure(AngleMeasure? measure)
    {
        if (measure != null && CurrentScreenshot != null)
        {
            var angleMeasure = new AngleMeasure
            {
                RespecifyParent = measure
            };
            _currentMeasure = angleMeasure;
            Mode = Modes.PlaceNewStart;
        }
    }

    private void RemoveMeasure(AngleMeasure? angleMeasure)
    {
        if (angleMeasure != null)
        {
            CurrentScreenshot?.AngleMeasures.Remove(angleMeasure);
        }
    }

    private void ImportBaseImage()
    {
        Screenshots.Add(new Screenshot(){Image = GetImageFromClipBoard()});
    }

    private void NewMeasure()
    {
        if (CurrentScreenshot == null) 
            return;
        
        var angleMeasure = new AngleMeasure();
        CurrentScreenshot.AngleMeasures.Add(angleMeasure);
        angleMeasure.AngleCalibration = CurrentScreenshot.DefaultCalibration;
        _currentMeasure = angleMeasure;
        Mode = Modes.PlaceNewStart;

    }
    
    public static ImageSource? GetImageFromClipBoard()
    {
        return Clipboard.ContainsImage() ? Clipboard.GetImage() : null;
    }
}