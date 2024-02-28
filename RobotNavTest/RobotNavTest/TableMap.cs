using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotNavTest
{
    public class TableMap
    {
        public List<List<Cell>> layout;         //Store the Cells in a List<List<Cells>>. First List<Cells is the Y Axis, the List<List<Cells>> is the X Axis
        public List<Cell> goalCells;
        public Cell agentCell;

        //Initialized TableMap with a layout of Cells.
        public TableMap(List<List<Cell>> Layout)
        {
            layout = Layout;

            List<Cell> tempCells = new List<Cell>();
            foreach (List<Cell> l in layout)
            {
                foreach (Cell c in l)
                {
                    if (c.wall)             //If cell is a wall, increase the Move Cost
                    {
                        c.moveCost = 2;
                    }
                    if (c.goal)             //Check if the cell knowns it's a goal, if so save in TableMap goalCells
                    {
                        tempCells.Add(c);
                    }
                    if (c.robotHere)        //Check if the cell knowns it's the agent, if so save in TableMap agentCell
                    {
                        agentCell = c;
                    }
                }
            }

            goalCells = tempCells;
            if (agentCell == null)          //If no cell was the agent cell, make the Agent Cell located at [0, 0]
            {
                agentCell = new Cell(new int[] { 0, 0 });
            }

            foreach (List<Cell> list in layout)     //Use to calculate the Goal distance for each goal, using Manhatten Distance. Save this information in the each Cell
            {
                foreach (Cell c in list)
                {
                    int distanceA = Math.Abs(c.location[0] - goalCells[0].location[0]) + Math.Abs(c.location[1] - goalCells[0].location[1]);
                    c.goalDistance.Add(distanceA);

                    int distanceB = Math.Abs(c.location[0] - goalCells[1].location[0]) + Math.Abs(c.location[1] - goalCells[1].location[1]);
                    c.goalDistance.Add(distanceB);
                }
            }
        }

        public void updateAgent(Cell updatedAgent)      //Takes in a cell, then changes the current agentCell so that it is equal to the updateAgent
        {
            agentCell.robotHere = false;
            layout[updatedAgent.location[0]][updatedAgent.location[1]].robotHere = true;
            agentCell = updatedAgent;
        }

        public Cell moveUp()        //Return the x y position of the cell above this one
        {
            return layout[agentCell.location[0]][(agentCell.location[1]) - 1];
        }

        public Cell moveDown()      //Return the x y position of the cell below this one
        {
            return layout[agentCell.location[0]][(agentCell.location[1]) + 1];
        }

        public Cell moveLeft()      //Return the x y position of the cell to the left this one
        {
            return layout[(agentCell.location[0]) - 1][agentCell.location[1]];
        }
        public Cell moveRight()     //Return the x y position of the cell to the right of this one
        {
            return layout[(agentCell.location[0]) + 1][agentCell.location[1]];
        }

        public Cell jumpUp()
        {
            Cell tempCell = layout[agentCell.location[0]][(agentCell.location[1]) - 1];
            double jumpNumber = 1;

            while (tempCell.wall)
            {
                jumpNumber++;
                if (tempCell.location[1] != 0)
                {
                    tempCell = layout[tempCell.location[0]][(tempCell.location[1] - 1)];
                }
                else
                {
                    break;
                }
            }
            if (!tempCell.wall)
            {
                tempCell.moveCost = Convert.ToInt32(Math.Pow(2, jumpNumber - 1));
                return tempCell;
            }
            else
            {
                return null;
            }
        }

        public Cell jumpDown()
        {
            Cell tempCell = layout[agentCell.location[0]][(agentCell.location[1]) + 1]; //updates tempCell with the move order. 
            double jumpNumber = 1;

            while (tempCell.wall)                                                       //Then, it checks if tempCell is a wall.
            {
                jumpNumber++;
                if (tempCell.location[1] != layout[0].Count - 1)                        //If it is, it redoes the move (if its not at the edge) then checks again.
                {
                    tempCell = layout[tempCell.location[0]][(tempCell.location[1]) + 1];
                }
                else
                {
                    break;
                }                                                                     //Repeats until it's not on a wall, then adds the jumpNumber all up.
            }
            if (!tempCell.wall)
            {
                tempCell.moveCost = Convert.ToInt32(Math.Pow(2, jumpNumber - 1));    //Cast 2 to the power of jumpNumber - 1, if it only jumped one square right, still counts as 1 move
                return tempCell;
            }
            else
            {
                return null;
            }
        }

        public Cell jumpLeft()
        {
            Cell tempCell = layout[(agentCell.location[0]) - 1][agentCell.location[1]];
            double jumpNumber = 1;

            while (tempCell.wall)
            {
                jumpNumber++;
                if (tempCell.location[0] != 0)
                {
                    tempCell = layout[(tempCell.location[0]) - 1][tempCell.location[1]];
                }
                else
                {
                    break;
                }
            }
            if (!tempCell.wall)
            {
                tempCell.moveCost = Convert.ToInt32(Math.Pow(2, jumpNumber - 1));
                return tempCell;
            }
            else
            {
                return null;
            }
        }

        public Cell jumpRight()
        {
            Cell tempCell = layout[(agentCell.location[0]) + 1][agentCell.location[1]];
            double jumpNumber = 1;

            while (tempCell.wall)
            {
                jumpNumber++;
                if (tempCell.location[1] != layout.Count - 1)
                {
                    tempCell = layout[(tempCell.location[0]) + 1][tempCell.location[1]];
                }
                else
                {
                    break;
                }
            }
            if (!tempCell.wall)
            {
                tempCell.moveCost = Convert.ToInt32(Math.Pow(2, jumpNumber - 1));
                return tempCell;
            }
            else
            {
                return null;
            }
        }
    }
}
