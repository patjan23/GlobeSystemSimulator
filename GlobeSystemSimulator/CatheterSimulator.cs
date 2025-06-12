using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace GlobeSystemSimulator
{
    public class Electrode : INotifyPropertyChanged
    {
        private double signalStrength;

        public string Name { get; set; } = string.Empty;
        public Point3D LocalPosition { get; set; }
        public Point3D WorldPosition { get; set; }

        public double SignalStrength
        {
            get => signalStrength;
            set
            {
                signalStrength = Math.Max(0, Math.Min(1, value)); // Clamp between 0-1
                OnPropertyChanged(nameof(SignalStrength));
                OnPropertyChanged(nameof(SignalPercent));
                OnPropertyChanged(nameof(Color));
            }
        }

        public string SignalPercent => $"{SignalStrength * 100:F0}%";

        public Color Color
        {
            get
            {
                // Gradient from Blue (weak) to Red (strong)
                byte red = (byte)(255 * SignalStrength);
                byte green = (byte)(100 * (1 - SignalStrength));
                byte blue = (byte)(255 * (1 - SignalStrength));
                return Color.FromRgb(red, green, blue);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CatheterSimulator
    {
        public Point3D CatheterPosition { get; private set; }
        public ObservableCollection<Electrode> Electrodes { get; private set; }

        public CatheterSimulator()
        {
            CatheterPosition = new Point3D(0, 0, 3);
            Electrodes = new ObservableCollection<Electrode>();
            InitializeElectrodes();
        }

        private void InitializeElectrodes()
        {
            // Create electrodes in a circular pattern (like the Globe catheter)
            int electrodeCount = 8;
            double radius = 0.5;

            for (int i = 0; i < electrodeCount; i++)
            {
                double angle = 2 * Math.PI * i / electrodeCount;
                double x = radius * Math.Cos(angle);
                double y = radius * Math.Sin(angle);
                double z = 0;

                Electrodes.Add(new Electrode
                {
                    Name = $"E{i + 1}",
                    LocalPosition = new Point3D(x, y, z),
                    WorldPosition = new Point3D(x + CatheterPosition.X, y + CatheterPosition.Y, z + CatheterPosition.Z)
                });
            }

            UpdateElectrodeSignals();
        }

        public void MoveCatheter(double x, double y, double z)
        {
            CatheterPosition = new Point3D(x, y, z);
            UpdateElectrodeSignals();
        }

        private void UpdateElectrodeSignals()
        {
            foreach (var electrode in Electrodes)
            {
                // Update world position
                electrode.WorldPosition = new Point3D(
                    electrode.LocalPosition.X + CatheterPosition.X,
                    electrode.LocalPosition.Y + CatheterPosition.Y,
                    electrode.LocalPosition.Z + CatheterPosition.Z);

                // Calculate signal strength based on distance to heart surface
                double distance = CalculateDistanceToHeart(electrode.WorldPosition);
                electrode.SignalStrength = CalculateSignalStrength(distance);
            }
        }

        private double CalculateDistanceToHeart(Point3D point)
        {
            // Simple distance to a sphere (heart model)
            double heartRadius = 2.0;
            double distanceToCenter = Math.Sqrt(point.X * point.X + point.Y * point.Y + point.Z * point.Z);
            return Math.Abs(distanceToCenter - heartRadius);
        }

        private double CalculateSignalStrength(double distance)
        {
            // Signal strength is strongest when close to surface
            // Using exponential decay for more realistic signal drop-off
            double maxDistance = 2.0;
            double normalizedDistance = Math.Min(distance, maxDistance) / maxDistance;
            return Math.Pow(1.0 - normalizedDistance, 2); // Quadratic falloff for more realistic behavior
        }

        public ModelVisual3D CreateCatheterVisual()
        {
            var group = new Model3DGroup();

            // Create catheter body (central hub)
            var catheterBodyGeometry = new MeshGeometry3D();
            AddSphere(catheterBodyGeometry, CatheterPosition, 0.15, 10);
            var catheterBodyMaterial = new DiffuseMaterial(Brushes.SteelBlue);
            var catheterBodyModel = new GeometryModel3D(catheterBodyGeometry, catheterBodyMaterial);
            group.Children.Add(catheterBodyModel);

            // Create electrodes and connections
            foreach (var electrode in Electrodes)
            {
                // Create electrode sphere
                var electrodeGeometry = new MeshGeometry3D();
                AddSphere(electrodeGeometry, electrode.WorldPosition, 0.08, 8);
                var electrodeMaterial = new DiffuseMaterial(new SolidColorBrush(electrode.Color));
                var electrodeModel = new GeometryModel3D(electrodeGeometry, electrodeMaterial);
                group.Children.Add(electrodeModel);

                // Create connection line from catheter to electrode
                var lineGeometry = new MeshGeometry3D();
                AddCylinder(lineGeometry, CatheterPosition, electrode.WorldPosition, 0.02, 6);
                var lineMaterial = new DiffuseMaterial(Brushes.LightGray);
                var lineModel = new GeometryModel3D(lineGeometry, lineMaterial);
                group.Children.Add(lineModel);
            }

            return new ModelVisual3D { Content = group };
        }

        private void AddSphere(MeshGeometry3D mesh, Point3D center, double radius, int divisions)
        {
            // Simple sphere implementation
            for (int i = 0; i <= divisions; i++)
            {
                double phi = Math.PI * i / divisions;
                for (int j = 0; j <= divisions; j++)
                {
                    double theta = 2 * Math.PI * j / divisions;

                    double x = center.X + radius * Math.Sin(phi) * Math.Cos(theta);
                    double y = center.Y + radius * Math.Sin(phi) * Math.Sin(theta);
                    double z = center.Z + radius * Math.Cos(phi);

                    mesh.Positions.Add(new Point3D(x, y, z));
                }
            }
        }

        private void AddCylinder(MeshGeometry3D mesh, Point3D p1, Point3D p2, double diameter, int divisions)
        {
            // Simple cylinder implementation between two points
            Vector3D direction = p2 - p1;
            double length = direction.Length;
            direction.Normalize();

            // Find an arbitrary perpendicular vector
            Vector3D perp = FindPerpendicular(direction);
            Vector3D perp2 = Vector3D.CrossProduct(direction, perp);

            for (int i = 0; i < divisions; i++)
            {
                double angle1 = 2 * Math.PI * i / divisions;
                double angle2 = 2 * Math.PI * (i + 1) / divisions;

                Vector3D v1 = perp * Math.Cos(angle1) + perp2 * Math.Sin(angle1);
                Vector3D v2 = perp * Math.Cos(angle2) + perp2 * Math.Sin(angle2);

                Point3D a1 = p1 + v1 * diameter;
                Point3D a2 = p1 + v2 * diameter;
                Point3D b1 = p2 + v1 * diameter;
                Point3D b2 = p2 + v2 * diameter;

                // Add triangles for the cylinder side
                AddTriangle(mesh, a1, b1, a2);
                AddTriangle(mesh, a2, b1, b2);
            }
        }

        private Vector3D FindPerpendicular(Vector3D v)
        {
            if (Math.Abs(v.X) < 0.1 && Math.Abs(v.Y) < 0.1)
                return new Vector3D(0, 1, 0);
            return new Vector3D(-v.Y, v.X, 0);
        }

        private void AddTriangle(MeshGeometry3D mesh, Point3D p1, Point3D p2, Point3D p3)
        {
            int index = mesh.Positions.Count;
            mesh.Positions.Add(p1);
            mesh.Positions.Add(p2);
            mesh.Positions.Add(p3);
            mesh.TriangleIndices.Add(index);
            mesh.TriangleIndices.Add(index + 1);
            mesh.TriangleIndices.Add(index + 2);
        }

        public int GetContactingElectrodesCount(double threshold = 0.7)
        {
            int count = 0;
            foreach (var electrode in Electrodes)
            {
                if (electrode.SignalStrength > threshold)
                    count++;
            }
            return count;
        }

        public double GetMappingProgress()
        {
            double totalSignal = 0;
            foreach (var electrode in Electrodes)
            {
                totalSignal += electrode.SignalStrength;
            }
            return Math.Min(1.0, totalSignal / Electrodes.Count);
        }
    }
}