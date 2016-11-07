using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TSP
{
    public partial class Form1 : Form
    {
        Graphics g;
        int active = 150;
        int inactive = 20;
        int scout = 30;
        int defaultCycles = 75000;
        bool clickMode = false;
        List<PointF> mouseClicks = new List<PointF>();

        public Form1()
        {
            InitializeComponent();
            this.g = pictureBox1.CreateGraphics();
            this.textBox5.Text = this.inactive.ToString();
            this.textBox4.Text = this.active.ToString();
            this.textBox6.Text = this.scout.ToString();
            this.textBox3.Text = this.defaultCycles.ToString();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            CityInfo c = new CityInfo(); // = new CityInfo(int.Parse(this.textBox1.Text));
            if (this.mouseClicks.Count > 0)
            {
                PointF[] add = new PointF[this.mouseClicks.Count];
               for (int a = 0; a < this.mouseClicks.Count; a++)
               {
                   add[a] = this.mouseClicks.ElementAt(a);
               }
               c.AddCities(add);
               this.mouseClicks.Clear();
               this.clickMode = false;
            }
            else
            {
                c = new CityInfo(int.Parse(this.textBox1.Text));
            }

            this.label4.Text = "";
            

            int maxNumVisits = c.cities.Count() * 5;
            int bees = int.Parse(this.textBox5.Text) + int.Parse(this.textBox4.Text)
                + int.Parse(this.textBox6.Text);

            Hive beehive = new Hive(bees, int.Parse(this.textBox5.Text),
                int.Parse(this.textBox4.Text), int.Parse(this.textBox6.Text),
                int.Parse(this.textBox3.Text), maxNumVisits, c);

            beehive.PlotCities(this.g);

            for (int i = 0; i < int.Parse(this.textBox3.Text); i++)
            {                
                beehive.Solve();
                if (i % 100 == 0)
                    beehive.DrawBestPath(this.g);
                this.label9.Text = (i + 1).ToString();
                this.label9.Refresh();
                this.distanceLabel.Text = beehive.BestPathDistance().ToString();
                this.distanceLabel.Refresh();
            }

            //this.label4.Text = "";
            //World w = new World(int.Parse(this.textBox1.Text), int.Parse(this.textBox2.Text));
            //w.PlotCities(this.g);
            //w.ScrambleGenomes();
            //minLabel.Text = w.FittestDistance().ToString();
            //for (int i = 0; i < int.Parse(textBox3.Text); i++)
            //{
            //    w.Algo(i);
            //    w.Draw(this.g);
            //    this.distanceLabel.Text = w.FittestDistance().ToString();
            //    this.distanceLabel.Refresh();
            //    if (int.Parse(this.distanceLabel.Text) < int.Parse(minLabel.Text))
            //    {
            //        minLabel.Text = distanceLabel.Text;
            //    }
            //    this.minLabel.Refresh();
            //    this.label9.Text = i.ToString();
            //    this.label9.Refresh();
            //}
            //w.Draw(this.g);
            //this.distanceLabel.Text = w.FittestDistance().ToString();
            //this.distanceLabel.Refresh();
            ////w.StartAlgorithm(int.Parse(this.textBox3.Text), this.g, this.label4);
            //this.label4.Text = "Algorithm Complete";           
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            //CityInfo c = new CityInfo(int.Parse(this.textBox1.Text));
            //Random r = new Random();

            //int maxNumVisits = c.cities.Count() * 5;

            //int[] bestValues = new int[3];
            //double shortestDistance = 0;

            //for (int d = 0; d < c.cities.Count() - 2; d++)
            //{
            //    shortestDistance += c.Distance(c.cities.ElementAt(d), c.cities.ElementAt(d + 1));
            //}
            //int trial = 0;




            //for (int q = 0; q < 500; q++)
            //{
            //    int numA = r.Next(1, 150);
            //    int numS = r.Next(1, 100);
            //    int numI = r.Next(1, 50);

            //    int bees = numA + numS + numI;
            //    Hive beehive = new Hive(bees, numI, numA, numS,
            //        100, maxNumVisits, c);
            //    for (int x = 0; x < 500; x++)
            //    {
            //        beehive.Solve();
            //    }

            //    if (beehive.BestPathDistance() < shortestDistance)
            //    {
            //        bestValues[0] = numA;
            //        bestValues[1] = numS;
            //        bestValues[2] = numI;
            //        shortestDistance = beehive.BestPathDistance();
            //    }

            //    if (q % 5 == 0)
            //        trial++;
            //    this.minLabel.Text = trial + "% complete";
            //    this.minLabel.Refresh();
            //}

            //    //for (int a = 1; a < 100; a++)
            //    //{

            //    //    for (int s = 1; s < 50; s++)
            //    //    {
            //    //        for (int i = 1; i < 20; i++)
            //    //        {
            //    //            int bees = s + i + a;
            //    //            Hive beehive = new Hive(bees, i, a, s,
            //    //                int.Parse(this.textBox3.Text), maxNumVisits, c);
            //    //            for (int x = 0; x < 500; x++)
            //    //            {
            //    //                beehive.Solve();
            //    //            }

            //    //            if (beehive.BestPathDistance() < shortestDistance)
            //    //            {
            //    //                bestValues[0] = a;
            //    //                bestValues[1] = s;
            //    //                bestValues[2] = i;
            //    //            }
            //    //        }
            //    //    }
            //    //    trial++;
            //    //    this.minLabel.Text = "Trial #" + trial;
            //    //    this.minLabel.Refresh();
            //    //}

            //this.active = bestValues[0];
            //this.scout = bestValues[1];
            //this.inactive = bestValues[2];
            this.textBox5.Text = this.inactive.ToString();
            this.textBox4.Text = this.active.ToString();
            this.textBox6.Text = this.scout.ToString();  

            this.minLabel.Text = "Done optimizing";
            this.minLabel.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.g.Clear(Color.WhiteSmoke);
            this.clickMode = true;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.clickMode)
            {
                this.mouseClicks.Add((PointF)e.Location);
                g.FillEllipse(Brushes.Black, new RectangleF(e.Location, new Size(5, 5)));
                this.textBox1.Text = this.mouseClicks.Count.ToString();
            }
        }
    }
}
