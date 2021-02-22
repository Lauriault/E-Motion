using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Media;

namespace E_Motion.Models
{
    public class Simulation
    {
        private List<Point> _heatPoints;
        private List<Dot> _dots;
        private Random _random;
        private Timer _timer;


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
        public Simulation(int pMaxX, int pMaxY, int pMaxDotCount ,Color pColor, int pDotSize, int pDotSizeVariation, int pLifespan, int pLifespanVariation)
        {
            this.Dots = new List<Dot>();
            this._heatPoints = new List<Point>();
            this._random = new Random();
            this._timer = new Timer(100);
            this._timer.Elapsed += _timer_Elapsed;
            

            this.SimulationStatus = false;
            this.MaxHorizontalSize = pMaxX;
            this.MaxVerticalSize = pMaxY;
            this.MaxDotCount = pMaxDotCount;
            this.DotColor = pColor;

            this.NormalSize = pDotSize;
            this.SizeDeltaVariation = pDotSizeVariation;

            this.LifeSpanDelta = pLifespanVariation;
            this.NormalLifeSpan = pLifespan;
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.GenerateDots();
        }

        public event EventHandler<EventArgs> DotAreGenerated;

        protected virtual void OnDotAreGenerated(EventArgs e)
        {
            EventHandler<EventArgs> handler = DotAreGenerated;
            if (handler != null)
                handler(this, e);
        }


        private void CleanUpDots()
        {
            for (int i = 0; i < this.Dots.Count; i++)
            {
                if (!this.Dots[i].PointIsValid)
                    this.Dots.Remove(this.Dots[i]);
            }
        }

        private void GenerateDots()
        {
            //this._timer.Stop();
            this.CleanUpDots();
            while (this.Dots.Count < (this.MaxDotCount))
            {
                int lifespan = this._random.Next(this.NormalLifeSpan - this.LifeSpanDelta, this.NormalLifeSpan + 1);
                int size = this._random.Next(this.NormalSize - this.SizeDeltaVariation, this.NormalSize + 1);
                this.Dots.Add(new Dot(lifespan, size, this.GetRandomCoordinates()));
            }          
            this.OnDotAreGenerated(new EventArgs());
        }

        /// <summary>
        /// Adds a heat point on the grid as center of mass that attracts more dots
        /// </summary>
        /// <param name="pPoint"></param>
        public void AddHeatpoint(Point pPoint)
        {
            throw new NotImplementedException();
        }

        public void StartSimulation()
        {
            this.SimulationStatus = true;
            this.GenerateDots();
            this._timer.Start();
        }

        public void ContinueSimulation() => Console.WriteLine("Yep"); /*this._timer.Start();*/

        private Point GetRandomCoordinates() => new Point(_random.Next(0, (this.MaxHorizontalSize - this.NormalSize) + 1), _random.Next(0, (this.MaxVerticalSize - this.NormalSize) + 1));

        public List<Dot> Dots { get => new List<Dot>(this._dots); private set => this._dots = value; }

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
        public int NormalSize { get; set; }

        /// <summary>
        /// Gets and sets the size variation of the dots
        /// </summary>
        public int SizeDeltaVariation { get; set; }

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

    }
}
