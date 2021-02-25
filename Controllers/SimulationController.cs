using E_Motion.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private Stopwatch _sw;
        private Brush _brush;
        private Pen _pen;

        public Simulation Simulation { get; set; }


        /// <summary>
        /// Initialise a new controller with the passed data
        /// </summary>
        public SimulationController(ref Canvas pCanvas)
        {
            this._sw = new Stopwatch();
            this._canvas = pCanvas;
            this.Simulation = new Simulation(5000, Color.FromRgb(255, 0, 0), 2, 0, 280, 140, 0);
            this.Simulation.DotAreGenerated += Simulation_DotAreGenerated;
            this._brush = new SolidColorBrush(this.Simulation.DotColor);
            this._pen = new Pen(this._brush, 1);
        }

        private void Simulation_DotAreGenerated(object sender, EventArgs e) => this.Draw();

        public void Start()
        {
            this.IsRunning = true;
            this.Simulation.StartSimulation();
        }

        public bool IsRunning { get; private set; }

        public void Draw()
        {
            this._sw.Start();
            DrawingVisual drawingVisual = new DrawingVisual();

            // Retrieve the DrawingContext in order to create new drawing content.
            DrawingContext drawingContext = drawingVisual.RenderOpen();

            this._canvas.Children.Clear();

            foreach (var dot in this.Simulation.Dots)
                drawingContext.DrawEllipse(new SolidColorBrush(this.Simulation.DotColor), null, dot.Coordinate, dot.Size / 2, dot.Size / 2);

            this._canvas.Children.Add(new VisualHost { Visual = drawingVisual });

            // Persist the drawing content.
            drawingContext.Close();
            this._sw.Stop();
            this.RenderTime = (int)this._sw.ElapsedMilliseconds;
            this._sw.Reset();
        }

        public int RenderTime 
        {
            get;private set;
        }

        public void Stop() 
        {
            this.IsRunning = false;
            this.Simulation.StopSimulation();
        }
    }
}
