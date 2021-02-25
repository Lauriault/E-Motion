using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace E_Motion.Models
{
    public class Dot
    {
        public Point Coordinate { get;private set; }
        public bool PointIsValid
        {
            get
            {
                return DateTime.Compare(DateTime.Now,this.PassDate) <=0;
            }
        }
        public double Size { get; set; }
        public DateTime PassDate { get; private set; }

        public Dot(int pLifespan, double pSize, Point pPoint)
        {
            this.PassDate = DateTime.Now.AddMilliseconds(pLifespan);
            this.Size = pSize;
            this.Coordinate = pPoint;
        }
    }
}
