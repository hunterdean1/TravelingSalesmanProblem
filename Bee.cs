using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TSP
{
    class Bee
    {
        public int status;
        public PointF[] memoryPath;
        public double measureOfQuality;
        public int numberOfVisits;

        public Bee(int stat, PointF[] path, double measure, int numVisits)
        {
            this.status = stat;
            this.memoryPath = path;
            this.measureOfQuality = measure;
            this.numberOfVisits = numVisits;
        }
    }
}
