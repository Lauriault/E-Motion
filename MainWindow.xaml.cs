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
        }

        public SimulationController Controller
        {
            get => this._controller;
            set => this._controller = value;
        }

        public int DotSize
        {
            get => this._controller.Simulation.NormalSize;
            set => this._controller.Simulation.NormalSize = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void MotionCanavas_Loaded(object sender, RoutedEventArgs e)
        {
            this._controller = new SimulationController(ref this.MotionCanavas);
        }
    }
}
