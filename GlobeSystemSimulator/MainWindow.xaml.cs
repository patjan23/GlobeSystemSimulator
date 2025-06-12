using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace GlobeSystemSimulator
{
    public partial class MainWindow : Window
    {
        private CatheterSimulator? catheterSimulator;
        private ModelVisual3D? heartVisual;
        private ModelVisual3D? catheterVisual;
        private bool isMapping = false;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeSimulation();
        }

        private void InitializeSimulation()
        {
            catheterSimulator = new CatheterSimulator();

            // Create heart model
            heartVisual = CreateHeartModel();
            viewport.Children.Add(heartVisual);

            // Create catheter visualization
            catheterVisual = catheterSimulator.CreateCatheterVisual();
            viewport.Children.Add(catheterVisual);

            // Initialize UI bindings
            electrodeSignalsList.ItemsSource = catheterSimulator.Electrodes;

            UpdateUI();
        }

        private ModelVisual3D CreateHeartModel()
        {
            // Create a simple spherical heart
            var heartSphere = new SphereVisual3D
            {
                Center = new Point3D(0, 0, 0),
                Radius = 2.0,
                Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.DarkRed)
            };

            return heartSphere;
        }

        private void Slider_ValueChanged(object sender, RoutedEventArgs e)
        {
            if (catheterSimulator != null)
            {
                catheterSimulator.MoveCatheter(sliderX.Value, sliderY.Value, sliderZ.Value);
                UpdateCatheterVisual();
                UpdateUI();
            }
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            sliderX.Value = 0;
            sliderY.Value = 0;
            sliderZ.Value = 3;
        }

        private async void BtnAutoMap_Click(object sender, RoutedEventArgs e)
        {
            if (!isMapping)
            {
                StartAutoMapping();
            }
            else
            {
                StopAutoMapping();
            }
        }

        private async void StartAutoMapping()
        {
            if (catheterSimulator == null) return;

            isMapping = true;
            btnAutoMap.Content = "Stop Auto-Mapping";

            // Simple automated mapping pattern
            double radius = 3.0;
            int steps = 36;

            for (int i = 0; i < steps && isMapping; i++)
            {
                double angle = 2 * Math.PI * i / steps;
                double x = radius * Math.Cos(angle);
                double z = 3 + Math.Sin(angle * 2) * 1.5;

                sliderX.Value = x;
                sliderZ.Value = z;

                // Add await to make it truly async
                await Task.Delay(200).ConfigureAwait(true);
            }

            isMapping = false;
            btnAutoMap.Content = "Start Auto-Mapping";
        }

        private void StopAutoMapping()
        {
            isMapping = false;
            btnAutoMap.Content = "Start Auto-Mapping";
        }

        private void UpdateCatheterVisual()
        {
            if (catheterSimulator == null) return;

            if (catheterVisual != null)
            {
                viewport.Children.Remove(catheterVisual);
            }
            catheterVisual = catheterSimulator.CreateCatheterVisual();
            viewport.Children.Add(catheterVisual);
        }

        private void UpdateUI()
        {
            if (catheterSimulator == null) return;

            // Update electrode signals display
            electrodeSignalsList.Items.Refresh();

            // Update contact information
            int contactingElectrodes = catheterSimulator.GetContactingElectrodesCount();
            txtContactInfo.Text = $"Electrodes in contact: {contactingElectrodes}/{catheterSimulator.Electrodes.Count}";

            // Update mapping progress
            double progress = catheterSimulator.GetMappingProgress();
            txtMappingProgress.Text = $"Mapping progress: {progress:P0}";
            progressMapping.Value = progress * 100;
        }
    }
}