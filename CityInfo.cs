using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TSP
{
    class CityInfo
    {
        Random rand = new Random();
        public PointF[] cities;
        public CityInfo(int numCities)
        {
            this.cities = new PointF[numCities];
            GenerateCities(numCities);
        }

        public CityInfo()
        {

        }

        // Generates a given number of randomly placed cities
        private void GenerateCities(int num)
        {
            for (int i = 0; i < num; i++)
            {
                float x = rand.Next(683);
                float y = rand.Next(568);
                
                this.cities[i] = new PointF(x, y);
            }
        }

        public void AddCities(PointF[] c)
        {
            this.cities = c;
        }

        // Plots the cities on the form
        public void PlotCities(Graphics g)
        {
            for (int i = 0; i < this.cities.Count(); i++)
            {
                g.FillEllipse(Brushes.Black, new RectangleF(this.cities[i], new SizeF(5, 5)));
            }
        }

        // Returns the distance between two cities
        public double Distance(PointF one, PointF two)
        {
            float x = (two.X - one.X) * 
                (two.X - one.X);

            float y = (two.Y - one.Y) *
                (two.Y - one.Y);

            return (float)Math.Sqrt((double)x + (double)y);
        }
    }
}
