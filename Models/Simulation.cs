using E_Motion.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace E_Motion.Models
{
    public class Simulation
    {
        private List<Point> _heatPoints;
        private SynchronizedCollection<Dot> _dots;
        private Stopwatch sw;
        private PointsService pointsService;

        private Random _random;
        private DispatcherTimer _timer;


        /// <summary>
        /// Initialise a simulation entity with the simulation data
        /// </summary>
        /// <param name="pMaxX">The canvas max x size</param>
        /// <param name="pMaxY">The canvas max y size</param>
        /// <param name="pColor">The color of the dots</param>
        /// <param name="pDotSize">The size of the dots in pixel</param>
        /// <param name="pDotSizeVariation">The delta variation of the dots in pixel</param>
        /// <param name="pLifespan">The lifespan of the dots in ms</param>
        /// <param name="pLifespanVariation">The lifespan variation of the dots in ms</param>
        public Simulation(int pMaxDotCount, Color pColor, int pDotSize, int pDotSizeVariation, int pLifespan, int pLifespanVariation, double pConcentrationFactor)
        {
            this._dots = new SynchronizedCollection<Dot>();
            this._heatPoints = new List<Point>();
            this._random = new Random();
            this._timer = new DispatcherTimer();
            this._timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            this._timer.Tick += _timer_Tick;
            this.sw = new Stopwatch();
            this.pointsService = new PointsService();
            this.ConcentrationFactor = pConcentrationFactor;

            this.SimulationStatus = false;
            //this.MaxHorizontalSize = pMaxX;
            //this.MaxVerticalSize = pMaxY;
            this.MaxDotCount = pMaxDotCount;
            this.DotColor = pColor;

            this.NormalSize = pDotSize;
            this.SizeDeltaVariation = pDotSizeVariation;

            this.LifeSpanDelta = pLifespanVariation;
            this.NormalLifeSpan = pLifespan;
        }

        private void _timer_Tick(object sender, EventArgs e) => this.GenerateDots();

        public event EventHandler<EventArgs> DotAreGenerated;

        protected virtual void OnDotAreGenerated(EventArgs e)
        {
            EventHandler<EventArgs> handler = DotAreGenerated;
            if (handler != null)
                handler(this, e);
        }

        public void Reset()
        {
            this._dots.Clear();
        }

        private void CleanUpDots()
        {
            for (int i = this._dots.Count - 1; i > 0; i--)
            {
                if (!this._dots.ElementAt(i).PointIsValid)
                    this._dots.RemoveAt(i);
            }
        }

        private void GenerateDots()
        {
            this.CleanUpDots();

            for (int i = 0; i < (this.MaxDotCount - this._dots.Count); i++)
            {
                int lifespan = this._random.Next(this.NormalLifeSpan - this.LifeSpanDelta, this.NormalLifeSpan + 1);
                double size = (new Random()).NextDouble()*((this.NormalSize+1)-(this.NormalSize - this.SizeDeltaVariation))+(this.NormalSize - this.SizeDeltaVariation);
                this._dots.Add(new Dot(lifespan, size, this.pointsService.GetNewPoint(this.MaxHorizontalSize,this.MaxVerticalSize,this._heatPoints,this.NormalSize, this.ConcentrationFactor)));
            }

            this.OnDotAreGenerated(new EventArgs());
        }

        /// <summary>
        /// Adds a heat point on the grid as center of mass that attracts more dots
        /// </summary>
        /// <param name="pPoint"></param>
        public void AddHeatpoint(Point pPoint)
        {
            this._heatPoints.Add(pPoint);
        }

        public void StartSimulation()
        {
            this._dots.Clear();
            this.SimulationStatus = true;
            this.GenerateDots();
            this._timer.Start();
        }

        public void StopSimulation()
        {
            this.SimulationStatus = false;
            this._timer.Stop();
        }

        public SynchronizedCollection<Dot> Dots { get => this._dots; }

        public bool SimulationStatus { get; private set; }

        /// <summary>
        /// Gets and sets the dot color
        /// </summary>
        public Color DotColor { get; set; }

        /// <summary>
        /// Gets and sets the max number of dots of the simulation
        /// </summary>
        public int MaxDotCount { get; set; }

        /// <summary>
        /// Gets and sets the normal dot size
        /// </summary>
        public double NormalSize { get; set; }

        /// <summary>
        /// Gets and sets the size variation of the dots
        /// </summary>
        public double SizeDeltaVariation { get; set; }

        /// <summary>
        /// Gets and sets the max vertical canvas size
        /// </summary>
        public int MaxVerticalSize { get; set; }

        /// <summary>
        /// Gets and sets the max horizontal canvas size
        /// </summary>
        public int MaxHorizontalSize { get; set; }

        /// <summary>
        ///Gets and sets The normal lifespan of the dots
        /// </summary>
        public int NormalLifeSpan { get; set; }

        /// <summary>
        /// Gets and sets the lifespan variation
        /// </summary>
        public int LifeSpanDelta { get; set; }

        /// <summary>
        /// Specifies the way the heat points concentrate dots
        /// </summary>
        public double ConcentrationFactor { get; set; }

    }
}
