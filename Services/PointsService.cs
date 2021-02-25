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
                    //if (radiusDistribution <= 0.1)
                    //    p = this.GetRandomRoundPoint(selectedHeatPoint, (int)(radius * Math.Pow(2, concentrationFactor+4)), (int)(radius * Math.Pow(2, concentrationFactor+7)));
                    //else if (radiusDistribution > 0.1 && radiusDistribution <= 0.3)
                    //    p = this.GetRandomRoundPoint(selectedHeatPoint, (int)(radius * Math.Pow(2, concentrationFactor+2)), (int)(radius * Math.Pow(2, concentrationFactor+4)));
                    //else if (radiusDistribution > 0.3 && radiusDistribution <= 0.6)
                    //    p = this.GetRandomRoundPoint(selectedHeatPoint, (int)(radius * Math.Pow(2, concentrationFactor)), (int)(radius * Math.Pow(2, concentrationFactor+2)));
                    //else if (radiusDistribution > 0.6 && radiusDistribution <= 1)
                    //    p = this.GetRandomRoundPoint(selectedHeatPoint, 0, (int)(radius * Math.Pow(2, concentrationFactor)));

                    if (radiusDistribution <= 0.1)
                        p = this.GetRandomRoundPoint(selectedHeatPoint, (int)(750 * Math.Exp(-pConcentrationFactor)), (int)(1000 * Math.Exp(-pConcentrationFactor)),pMaxXSize,pMaxYSize);
                    else if (radiusDistribution > 0.1 && radiusDistribution <= 0.3)
                        p = this.GetRandomRoundPoint(selectedHeatPoint, (int)(500 * Math.Exp(-pConcentrationFactor)), (int)(750 * Math.Exp(-pConcentrationFactor)), pMaxXSize, pMaxYSize);
                    else if (radiusDistribution > 0.3 && radiusDistribution <= 0.5)
                        p = this.GetRandomRoundPoint(selectedHeatPoint, (int)(250 * Math.Exp(-pConcentrationFactor)), (int)(500 * Math.Exp(-pConcentrationFactor)), pMaxXSize, pMaxYSize);
                    else if (radiusDistribution > 0.5 && radiusDistribution <= 1)
                        p = this.GetRandomRoundPoint(selectedHeatPoint, 0, (int)(250*Math.Exp(-pConcentrationFactor)), pMaxXSize, pMaxYSize);
                }
            }
            else
            {
                p = this.GetRandomPoint(pMaxXSize, pMaxYSize, pNormalSize);
            }
            return p;
        }

        private Point GetRandomPoint(int pMaxXSize, int pMaxYSize, double pNormalSize) => new Point(_random.NextDouble()*(((pMaxXSize - pNormalSize) + 1)), _random.NextDouble() * (((pMaxYSize - pNormalSize) + 1)));

        private Point GetRandomRoundPoint(Point pCenter,int pInnerRadius, int pOutRadius, int pMaxX, int pMaxy)
        {
            int randomRadius = this._random.Next(pInnerRadius, pOutRadius+1);
            int randomAngle = this._random.Next(0, 360);
            double x = Math.Abs((pCenter.X + (randomRadius * Math.Cos((double)(randomAngle * Math.PI) / 180)))%pMaxX);
            double y = Math.Abs((pCenter.Y + (randomRadius * Math.Sin((double)(randomAngle * Math.PI) / 180)))%pMaxy);
            return new Point(x, y);
        }
    }
}
