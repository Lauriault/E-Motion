using E_Motion.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace E_Motion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private SimulationController _controller;

        public MainWindow()
        {
            InitializeComponent();
            this._controller = new SimulationController(ref this.MotionCanavas);
            this.DataContext = this._controller;
        }

        public SimulationController Controller
        {
            get => this._controller;
            set => this._controller = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void MotionCanavas_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this._controller != null)
            {
                this._controller.Simulation.MaxVerticalSize = (int)this.MotionCanavas.ActualHeight;
                this._controller.Simulation.MaxVerticalSize = (int)this.MotionCanavas.ActualHeight;
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            this._controller.Simulation.MaxHorizontalSize = (int)this.MotionCanavas.ActualWidth;
            this._controller.Simulation.MaxVerticalSize = (int)this.MotionCanavas.ActualHeight;
            this._controller.Start();
        }

        private void DotSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.DotSizeDeltaSlider != null)
            {
                this.DotSizeDeltaSlider.Minimum = 0;
                this.DotSizeDeltaSlider.Maximum = this.DotSizeSlider.Value - 0.1;
            }
        }

        private void DotLifeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.DotLifeSlider != null)
            {
                this._controller.Simulation.Reset();
                this.DotLifeDeltaSlider.Minimum = 0;
                this.DotLifeDeltaSlider.Maximum = this.DotLifeSlider.Value-1;
            }
        }
    }
}
