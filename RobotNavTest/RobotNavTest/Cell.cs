using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotNavTest
{
    public class Cell
    {
        public Cell parent;

        public int[] location;
        public bool goal;
        public bool robotHere;
        public bool wall;
        public int moveCost;

        public List<int> goalDistance;

        //Cell only knows it's location at first. All other information comes from the TableMap
        public Cell(int[] location)
        {
            this.location = location;
            this.goal = false;
            this.robotHere = false;
            this.wall = false;
            goalDistance = new List<int>();
            moveCost = 1;
        }

        public double totalCost()
        {
            int minDistance = int.MaxValue;
            int total = 0;
            for (int i = 0; i <= goalDistance.Count - 1; i++)
            {
                total += goalDistance[i];

                if (goalDistance[i] < minDistance)
                {
                    minDistance = goalDistance[i];
                }
            }

            return minDistance + 0.5 * total;
        }
    }
}
