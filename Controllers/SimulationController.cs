using E_Motion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace E_Motion.Controllers
{
    public class SimulationController
    {
        private Canvas _canvas;

        public Simulation Simulation { get; set; }
        

        /// <summary>
        /// Initialise a new controller with the passed data
        /// </summary>
        public SimulationController(ref Canvas pCanvas)
        {
            this._canvas = pCanvas;
            this.Simulation = new Simulation((int)pCanvas.ActualWidth, (int)pCanvas.ActualHeight,3500,Color.FromRgb(255,0,0),3,0,500,200);
            this.Simulation.DotAreGenerated += Simulation_DotAreGenerated;
            this.Start();
        }

        private void Simulation_DotAreGenerated(object sender, EventArgs e) => this.Draw();

        public void Start()
        {
            this.Simulation.StartSimulation();
        }

        public void Draw() 
        {           
            Application.Current.Dispatcher.Invoke((Action)delegate {
                this._canvas.Children.Clear();

                this.Simulation.Dots.ForEach(dot =>
                {
                    Ellipse circle = new Ellipse()
                    {
                        Width = dot.Size,
                        Height = dot.Size,
                        Stroke = new SolidColorBrush(this.Simulation.DotColor),
                        StrokeThickness = dot.Size
                    };
                    _canvas.Children.Add(circle);

                    circle.SetValue(Canvas.LeftProperty, (double)dot.Coordinate.X);
                    circle.SetValue(Canvas.TopProperty, (double)dot.Coordinate.Y);
                });
                this.Simulation.ContinueSimulation();
            });         
        }

        public void Pause() { }

        public void Restart() { }
    }
}
