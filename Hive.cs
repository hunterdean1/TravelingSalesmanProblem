using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TSP
{
    class Hive
    {
        Random rand = new Random(0);

        // object holding the data about cities
        CityInfo cityData;

        // information about the groups of bees
        int totalNumBees;
        int numberInactive;
        int numberActive;
        int numberScout;

        // maximum constraints for visits per path & total cycles
        int maxNumberVisits;
        int maxNumberCycles;

        // probabilities
        double probPersuasion = 0.90;
        double probMistake = 0.01;

        // information about the hive
        Bee[] bees;
        PointF[] bestMemoryPath;
        double bestMeasureOfQuality;
        int[] indexesOfInactiveBees;
        int neighborSwitches;

        public Hive(int totalBees, int numInactive, int numActive, int numScout,
            int maxCycles, int maxVisits, CityInfo cities)
        {
            //this.rand = new Random(0);

            this.totalNumBees = totalBees;
            this.numberInactive = numInactive;
            this.numberActive = numActive;
            this.numberScout = numScout;
            this.maxNumberCycles = maxCycles;
            this.maxNumberVisits = maxVisits;
            this.cityData = cities;

            this.neighborSwitches = (this.cityData.cities.Count() / 10) + 1;

            this.bees = new Bee[totalBees];
            
            // initializes the best path & quality randomly
            this.bestMemoryPath = GenerateRandomPath();
            this.bestMeasureOfQuality = MeasureOfQuality(this.bestMemoryPath);

            this.indexesOfInactiveBees = new int[numberInactive];

            // Creates bee objects and randomly assigns them a path,
            // replaces best path & measure if new bee is more effective
            for (int i = 0; i < this.totalNumBees; i++)
            {
                int currStatus;
                if (i < numberInactive)
                {
                    currStatus = 0;
                    indexesOfInactiveBees[i] = i;
                }
                else if (i < numberInactive + numberScout)
                {
                    currStatus = 2;
                }
                else
                {
                    currStatus = 1;
                }

                PointF[] randomPath = GenerateRandomPath();
                double mq = MeasureOfQuality(randomPath);

                this.bees[i] = new Bee(currStatus, randomPath, mq, 0);

                if (bees[i].measureOfQuality < this.bestMeasureOfQuality)
                {
                    Array.Copy(bees[i].memoryPath, this.bestMemoryPath, this.bees[i].memoryPath.Length);
                    this.bestMeasureOfQuality = this.bees[i].measureOfQuality;
                }
            }
        }

        // Plots the cities on the form
        public void PlotCities(Graphics g)
        {
            this.cityData.PlotCities(g);
        }

        // returns the shortest distance out of all the paths
        public double BestPathDistance()
        {
            return this.bestMeasureOfQuality;
        }

        // draws the route that travels the shortest distance
        public void DrawBestPath(Graphics g)
        {
            g.Clear(Color.WhiteSmoke);
            PlotCities(g);
            for (int i = 0; i < this.bestMemoryPath.Length - 1; i++)
            {
                g.DrawLine(Pens.Red, this.bestMemoryPath[i], this.bestMemoryPath[i + 1]);
            }
            g.DrawLine(Pens.Red, this.bestMemoryPath[this.bestMemoryPath.Length - 1], this.bestMemoryPath[0]); 
        }

        // returns a randomly generated path
        private PointF[] GenerateRandomPath()
        {
            PointF[] result = new PointF[this.cityData.cities.Length];
            Array.Copy(this.cityData.cities, result, this.cityData.cities.Length);
            for (int i = 0; i < result.Length; i++)
            {
                int r = rand.Next(i, result.Length);
                PointF temp = result[r];
                result[r] = result[i];
                result[i] = temp;
            }
            return result;
        }

        // Takes a path and swaps one index with another to create
        // a similar but different path
        private PointF[] GenerateNeighborPath(PointF[] path)
        {
            PointF[] result = new PointF[path.Length];
            Array.Copy(path, result, path.Length);
            for (int i = 0; i < rand.Next(1, 4); i++)
            {
                int ranIndex = rand.Next(0, result.Length);
                int adjIndex;
                if (ranIndex == result.Length - 1)
                    adjIndex = 0;
                else
                    adjIndex = ranIndex + 1;

                PointF temp = result[ranIndex];
                result[ranIndex] = result[adjIndex];
                result[adjIndex] = temp;
            }

            return result;
        }

        // returns the distance travelled in a given path
        private double MeasureOfQuality(PointF[] path)
        {
            double answer = 0.0;
            for (int i = 0; i < path.Length - 1; i++)
            {
                PointF p1 = path[i];
                PointF p2 = path[i + 1];
                double d = this.cityData.Distance(p1, p2);
                answer += d;
            }
            answer += this.cityData.Distance(path[path.Length - 1], path[0]);
            return answer;
        }

        // Processes each bee and calls the designated method
        // for the bees status
        public void Solve()
        {
            for (int i = 0; i < this.bees.Length; i++)
            {
                if (this.bees[i].status == 1)
                {
                    ProcessActiveBee(i);
                }                    
                else if (this.bees[i].status == 2)
                {
                    ProcessScoutBee(i);
                }
                else if (this.bees[i].status == 0)
                {
                    // Do nothing
                }
            }
        }

        // Generates a neighbor path and determines if it is better or
        // worse. if better it tries to switch but allows a chance for 
        // a mistake. if worse it tries to stay the same but allows a
        // chance to mistakenly swap
        // chance for error helps explore more options
        // if the number of visits for the bee is at it's max,
        // the bee goes inactive and is replaced by a new one.
        // also compares the path with the inactive bees
        private void ProcessActiveBee(int i)
        {
            PointF[] neighbor = GenerateNeighborPath(this.bees[i].memoryPath);
            double neighborQual = MeasureOfQuality(neighbor);
            double prob = rand.NextDouble();
            bool pathWasUpdated = false;
            bool numVisitsOverLimit = false;

            if (neighborQual < this.bees[i].measureOfQuality)
            {
                if (prob < this.probMistake)
                {
                    ++this.bees[i].numberOfVisits;
                    if (this.bees[i].numberOfVisits > this.maxNumberVisits)
                        numVisitsOverLimit = true;
                }
                else
                {
                    Array.Copy(neighbor, this.bees[i].memoryPath, neighbor.Length);
                    this.bees[i].measureOfQuality = neighborQual;
                    this.bees[i].numberOfVisits = 0;
                    pathWasUpdated = true;
                }
            }
            else
            {
                if (prob < this.probMistake)
                {
                    Array.Copy(neighbor, this.bees[i].memoryPath, neighbor.Length);
                    this.bees[i].measureOfQuality = neighborQual;
                    this.bees[i].numberOfVisits = 0;
                    pathWasUpdated = true;
                }
                else
                {
                    ++this.bees[i].numberOfVisits;
                    if (this.bees[i].numberOfVisits > this.maxNumberVisits)
                        numVisitsOverLimit = true;
                }
            }

            if (numVisitsOverLimit)
            {
                this.bees[i].status = 0;
                this.bees[i].numberOfVisits = 0;
                int x = rand.Next(this.numberInactive);
                this.bees[this.indexesOfInactiveBees[x]].status = 1;
                this.bees[this.indexesOfInactiveBees[x]].numberOfVisits = 0;
                indexesOfInactiveBees[x] = i;
            }
            else if (pathWasUpdated)
            {
                if (this.bees[i].measureOfQuality < this.bestMeasureOfQuality)
                {
                    Array.Copy(this.bees[i].memoryPath, this.bestMemoryPath, this.bees[i].memoryPath.Length);
                    this.bestMeasureOfQuality = this.bees[i].measureOfQuality;
                }
                DoWaggleDance(i);
            }
            else
            {
                return;
            }
        }

        // creates a completely new path and compares it
        // with the current one. if better, the new path is 
        // the chosen path for the bee. 
        // also communicates with the inactive bees
        private void ProcessScoutBee(int i)
        {
            PointF[] randomFoodSource = GenerateRandomPath();
            double randomQuality = MeasureOfQuality(randomFoodSource);
            if (randomQuality < this.bees[i].measureOfQuality)
            {
                Array.Copy(randomFoodSource, this.bees[i].memoryPath, randomFoodSource.Length);
                this.bees[i].measureOfQuality = randomQuality;
                if (this.bees[i].measureOfQuality < this.bestMeasureOfQuality)
                {
                    Array.Copy(this.bees[i].memoryPath, this.bestMemoryPath, this.bestMemoryPath.Length);
                    this.bestMeasureOfQuality = this.bees[i].measureOfQuality;
                }
                DoWaggleDance(i);
            }
        }

        // Compares the path of the given bee with all
        // the inactive ones. if an inactive bee is 
        // 'persuaded' by the active bee it accepts the
        // new path
        private void DoWaggleDance(int i)
        {
            for (int x = 0; x < this.numberInactive; x++)
            {
                int b = this.indexesOfInactiveBees[x];
                if (this.bees[i].measureOfQuality < this.bees[b].measureOfQuality)
                {
                    double p = rand.NextDouble();
                    if (this.probPersuasion > p)
                    {
                        Array.Copy(this.bees[i].memoryPath, this.bees[b].memoryPath,
                            this.bees[i].memoryPath.Length);
                        this.bees[b].measureOfQuality = this.bees[i].measureOfQuality;
                    }
                }
            }
        }
    }
}
