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
            this.Simulation = new Simulation((int)pCanvas.ActualWidth, (int)pCanvas.ActualHeight, 3500, Color.FromRgb(255, 0, 0), 3, 0, 400, 200);
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
            DrawingVisual drawingVisual = new DrawingVisual();

            // Retrieve the DrawingContext in order to create new drawing content.
            DrawingContext drawingContext = drawingVisual.RenderOpen();

            this._canvas.Children.Clear();

            foreach (var dot in this.Simulation.Dots)
                drawingContext.DrawEllipse(new SolidColorBrush(this.Simulation.DotColor), null, dot.Coordinate, dot.Size/2, dot.Size/2);

            this._canvas.Children.Add(new VisualHost { Visual = drawingVisual });

            // Persist the drawing content.
            drawingContext.Close();
        }

        public void Pause() { }

        public void Restart() { }
    }
}
