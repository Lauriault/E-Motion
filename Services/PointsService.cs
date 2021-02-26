using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace E_Motion.Services
{
    public class PointsService
    {
        private Random _random;

        public PointsService()
        {
            this._random = new Random();
        }

        /// <summary>
        /// Coordinate the point getting operation
        /// </summary>
        /// <returns></returns>
        public Point GetNewPoint(int pMaxXSize, int pMaxYSize, IList<Point> pHeatPoints, double pNormalSize, double pConcentrationFactor)
        {
            Point p = new Point();
            //If there is heat points, then impact the spawn
            if (pHeatPoints != null && pHeatPoints.Count > 0)
            {
                //select a random point in the heat points , or anything but a heatpoint
                var randomIndex = this._random.NextDouble();
                //the probability per hp of getting a point
                double distributionPerHP = (double)1 / pHeatPoints.Count;
                //the probability of getting a point no associated with a heat point
                double noHPDistribution = distributionPerHP / 10;


                if (randomIndex <= noHPDistribution)
                {
                    p = this.GetRandomPoint(pMaxXSize, pMaxYSize, pNormalSize);
                }
                else
                {
                    //the heat point we will be randomizing a point for
                    var selectedHeatPoint = pHeatPoints.ElementAt(_random.Next(0, pHeatPoints.Count));

                    //the probability for the radius
                    double radiusDistribution = this._random.NextDouble();
                    if (radiusDistribution <= 0.1)
                        p = this.GetRandomRoundPoint(selectedHeatPoint, (750 * Math.Exp(-pConcentrationFactor)), (1000 * Math.Exp(-pConcentrationFactor)), pMaxXSize, pMaxYSize);
                    else if (radiusDistribution > 0.1 && radiusDistribution <= 0.3)
                        p = this.GetRandomRoundPoint(selectedHeatPoint, (500 * Math.Exp(-pConcentrationFactor)), (750 * Math.Exp(-pConcentrationFactor)), pMaxXSize, pMaxYSize);
                    else if (radiusDistribution > 0.3 && radiusDistribution <= 0.5)
                        p = this.GetRandomRoundPoint(selectedHeatPoint, (250 * Math.Exp(-pConcentrationFactor)), (500 * Math.Exp(-pConcentrationFactor)), pMaxXSize, pMaxYSize);
                    else if (radiusDistribution > 0.5 && radiusDistribution <= 1)
                        p = this.GetRandomRoundPoint(selectedHeatPoint, 0, (250 * Math.Exp(-pConcentrationFactor)), pMaxXSize, pMaxYSize);
                }
            }
            else
            {
                p = this.GetRandomPoint(pMaxXSize, pMaxYSize, pNormalSize);
            }
            return p;
        }

        private Point GetRandomPoint(int pMaxXSize, int pMaxYSize, double pNormalSize) => new Point(_random.NextDouble() * (((pMaxXSize - pNormalSize) + 1)), _random.NextDouble() * (((pMaxYSize - pNormalSize) + 1)));

        private Point GetRandomRoundPoint(Point pCenter, double pInnerRadius, double pOutRadius, int pMaxX, int pMaxy)
        {
            double randomRadius = this._random.NextDouble() * ((pOutRadius + 1) - pInnerRadius) + pInnerRadius;
            int randomAngle = this._random.Next(0, 360);
            double x = Math.Abs(pCenter.X + (randomRadius * Math.Cos((double)(randomAngle * Math.PI) / 180)));
            double y = Math.Abs(pCenter.Y + (randomRadius * Math.Sin((double)(randomAngle * Math.PI) / 180)));
            return new Point(x, y);
        }
    }
}
