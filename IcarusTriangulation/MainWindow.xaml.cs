using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace IcarusTriangulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel Vm => (MainWindowViewModel) DataContext;

        private int _gridCalibrationCount = 1;
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void MyCanvas_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left || Vm.CurrentScreenshot == null)
            {
                return;
            }

            var pos = e.GetPosition(MyCanvas);
            var mapGridView = Vm.CurrentScreenshot?.MapGridView;
            switch(Vm.Mode)
            {
                case Modes.None:
                    break;
                case Modes.PlaceNewStart:
                    if (Vm.CurrentMeasure != null)
                    {
                        Vm.CurrentMeasure.StartPoint = pos;

                        if (Vm.CurrentMeasure.RespecifyParent != null)
                        {
                            var p = Vm.CurrentMeasure.RespecifyParent;
                            Vm.CurrentScreenshot?.AngleMeasures.Add(Vm.CurrentMeasure);
                            Vm.CurrentMeasure.L1 = Vm.CurrentMeasure.StartPoint + (p.L1 - p.StartPoint);
                            Vm.CurrentMeasure.L2 = Vm.CurrentMeasure.StartPoint + (p.L2 - p.StartPoint);
                            Vm.Mode = Modes.None;
                            Vm.CurrentMeasure.IsComplete = true;
                        }
                        else
                        {
                            Vm.CurrentMeasure.L1 = pos;
                            Vm.CurrentMeasure.L2 = pos;
                            Vm.Mode = Modes.MeasureLine1;
                        }
                    }

                    break;
                case Modes.MeasureLine1:
                    if (Vm.CurrentMeasure != null)
                    {
                        Vm.CurrentMeasure.L1 = pos;
                        Vm.Mode = Modes.MeasureLine2;
                    }
                    break;
                case Modes.MeasureLine2:
                    if (Vm.CurrentMeasure != null)
                    {
                        Vm.CurrentMeasure.L2 = pos;
                        Vm.Mode = Modes.None;
                        Vm.CurrentMeasure.IsComplete = true;
                    }
                    break;
                case Modes.Calibrate:
                    var calibration = AngleCalibration.FromAngles(Vm.SelectedMeasures[0], Vm.SelectedMeasures[1], pos);
                    Vm.Calibrations.Add(calibration);
                    Vm.Mode = Modes.None;
                    break;
                case Modes.GridEdge:
                    if (mapGridView != null) mapGridView.GridSettings.GridStartEdge = pos;
                    Vm.Mode = Modes.GridOppositeEdge;
                    break;
                case Modes.GridOppositeEdge:
                    if (mapGridView != null)
                    {
                        mapGridView.GridSettings.GridEndEdge =
                            mapGridView.GridSettings.GridStartEdge 
                                with {
                                    X = mapGridView.GridSettings.GridStartEdge.X + mapGridView.GridWidth * _gridCalibrationCount
                                };
                        var angleCalibration = GridCalibration.DefaultCalibration.CreateCalibration(mapGridView.GridWidth);
                        Vm.Calibrations.Add(angleCalibration);
                        Vm.CurrentScreenshot.DefaultCalibration = angleCalibration;
                        Vm.AssignCalibration(angleCalibration);
                    }
                    Vm.Mode = Modes.None;
                    break;
            }
        }

        private void MyCanvas_OnMouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.GetPosition(MyCanvas);
            switch(Vm.Mode)
            {
                case Modes.None:
                    MyCanvas.Cursor = Cursors.Arrow;
                    break;
                case Modes.PlaceNewStart:
                    MyCanvas.Cursor = Cursors.Cross;
                    break;
                case Modes.MeasureLine1:
                    MyCanvas.Cursor = Cursors.Arrow;
                    if (Vm.CurrentMeasure != null) Vm.CurrentMeasure.L1 = pos;
                    break;
                case Modes.MeasureLine2:
                    MyCanvas.Cursor = Cursors.Arrow;
                    if (Vm.CurrentMeasure != null) Vm.CurrentMeasure.L2 = pos;
                    break;
                case Modes.Calibrate:
                    MyCanvas.Cursor = Cursors.Cross;
                    break;
                case Modes.GridEdge:
                    MyCanvas.Cursor = Cursors.Cross;
                    break;
                case Modes.GridOppositeEdge:
                    if (Vm.CurrentScreenshot != null)
                        Vm.CurrentScreenshot.MapGridView.GridWidth =
                            Math.Abs(pos.X - Vm.CurrentScreenshot.MapGridView.GridSettings.GridStartEdge.X) /
                            _gridCalibrationCount;
                    break;
            }
        }

        private void ScreenshotListDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[]?)e.Data.GetData(DataFormats.FileDrop) ?? Array.Empty<string>();

                foreach (var file in files)
                {
                    var bitmapImage = new BitmapImage(new Uri(file));
                    Vm.Screenshots.Add(new Screenshot(){Image = bitmapImage});
                }
            }
        }

        private void AngleMeasures_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var eRemovedItem in e.RemovedItems.OfType<AngleMeasure>())
            {
                Vm.SelectedMeasures.Remove(eRemovedItem);
            }

            foreach (var eAddedItem in e.AddedItems.OfType<AngleMeasure>())
            {
                Vm.SelectedMeasures.Add(eAddedItem);
            }
            Vm.NewCalibrationCommand.NotifyCanExecuteChanged();
        }

        private void MainWindow_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                _gridCalibrationCount++;
            }
            else if (e.Key == Key.Left)
            {
                if (_gridCalibrationCount > 1)
                    _gridCalibrationCount--;
            }
        }
    }
}