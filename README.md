
```markdown
# Cardiac Electrophysiology Mapping Simulator

🏥 **Medical Device Simulation** | 🎯 **WPF 3D Visualization** | ⚡ **Real-time Signal Processing**

An interactive 3D simulator that demonstrates advanced cardiac mapping catheter technology used in electrophysiology procedures like atrial fibrillation ablation. This project simulates the functionality of a Globe catheter system for real-time cardiac surface mapping and signal detection.

<img width="887" height="596" alt="image" src="https://github.com/user-attachments/assets/9bf15e6f-f5e5-41ba-89d5-c8c8501565ae" />


## ✨ Key Features

- **3D Heart Chamber Model** with realistic surface geometry
- **Multi-electrode Catheter** simulating real medical devices (8+ electrodes)
- **Real-time Signal Detection** based on proximity to heart wall
- **Dynamic Color Mapping** - electrodes change color based on signal strength (Blue → Red)
- **Professional Medical UI** with live electrode data and signal visualization
- **Interactive 3D Controls** for catheter navigation and positioning
- **Auto-Mapping Mode** for automated demonstration of mapping capabilities
- **Live Progress Tracking** showing mapping completion percentage

## 🛠️ Tech Stack

- **C# / WPF / .NET 8** - Modern Windows desktop development
- **Helix Toolkit** - High-performance 3D graphics rendering
- **MVVM Architecture** - Clean, maintainable code structure
- **Real-time Data Binding** - Live UI updates
- **3D Mathematical Models** - Accurate signal strength calculations

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022 or later
- Windows 10/11

### Installation & Running

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/cardiac-mapping-simulator.git
   cd cardiac-mapping-simulator
   ```

2. **Open in Visual Studio**
   - Open `GlobeSystemSimulator.sln`
   - Restore NuGet packages
   - Build the solution (Ctrl+Shift+B)

3. **Run the application**
   - Press F5 to start debugging
   - Or Ctrl+F5 to run without debugging

### Using the Simulator

1. **Manual Control**:
   - Use the X, Y, Z sliders to position the catheter
   - Observe real-time signal changes in electrode displays
   - Watch electrode colors change based on contact quality

2. **Auto-Mapping**:
   - Click "Start Auto-Mapping" for automated demonstration
   - The system will automatically navigate around the heart surface
   - Monitor mapping progress in real-time

3. **Signal Interpretation**:
   - **Red Electrodes**: Strong signal (close to heart surface)
   - **Blue Electrodes**: Weak signal (far from heart surface)
   - **Progress Bars**: Visual representation of signal strength

## 📁 Project Structure

```
GlobeSystemSimulator/
├── MainWindow.xaml          # Main application UI with 3D viewport
├── MainWindow.xaml.cs       # Application logic and event handlers
├── CatheterSimulator.cs     # Core simulation engine and 3D modeling
├── Electrode.cs            # Electrode data model with INotifyPropertyChanged
├── ProgressConverter.cs    # UI data binding converter for signal visualization
└── GlobeSystemSimulator.csproj
```

## 🔬 Technical Implementation

### Core Components

**3D Heart Model**
- Procedurally generated spherical geometry with organic variations
- Realistic surface for distance-based signal calculations
- Optimized mesh for smooth rendering

**Catheter System**
- 8-electrode circular array simulating real catheter designs
- Real-time position and signal calculations
- Visual connections between catheter body and electrodes

**Signal Processing**
- Distance-based signal strength algorithms
- Quadratic falloff for realistic signal degradation
- Real-time updates with property change notifications

## 🎯 Use Cases

- **Medical Device Development** - Prototype and demonstrate catheter technology
- **Electrophysiology Education** - Train medical professionals on mapping principles
- **Software Engineering Portfolio** - Showcase advanced WPF 3D development skills
- **Research & Development** - Test mapping algorithms and user interfaces

## 🏥 Medical Context

This simulator represents the next generation of **cardiac electrophysiology mapping systems** used for:
- Atrial Fibrillation (AFib) ablation procedures
- Ventricular Tachycardia (VT) mapping
- Complex arrhythmia diagnosis
- Cardiac anatomy reconstruction

## 🤝 Contributing

Contributions are welcome! Areas for enhancement:
- Additional catheter designs and configurations
- More sophisticated heart chamber geometries
- Advanced signal processing algorithms
- Export capabilities for mapping data
- Multi-chamber heart support

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🙏 Acknowledgments

- **Helix Toolkit** team for excellent 3D WPF components
- Medical device industry for inspiration in electrophysiology technology
- Cardiac electrophysiologists advancing the field of arrhythmia treatment

---

**Built with 💓 using C# and WPF** | *Demonstrating the future of cardiac mapping technology*
```
