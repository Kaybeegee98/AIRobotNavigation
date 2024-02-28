using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotNavTest
{
    public partial class Program
    {
        static void bfs_Jump_Solution(TableMap map)
        {
            HashSet<Cell> visitedCells = new HashSet<Cell>();
            Queue<Cell> queue = new Queue<Cell>();
            List<string> commandList = new List<string>();

            int nodes = 0;
            Cell startPoint = map.agentCell;
            visitedCells.Add(startPoint);
            queue.Enqueue(startPoint);

            while (queue.Count > 0)
            {
                nodes++;
                Cell agentCell = queue.Dequeue();

                map.updateAgent(agentCell);


                if (map.goalCells.Contains(agentCell))
                {
                    Console.Write(nodes.ToString() + "\n");
                    Cell currentCell = agentCell;
                    while (currentCell.parent != null)      //Work backwards, from where the Agent Cell ended back to the start by checking if it has a Parent
                    {                                       //Since we never gave the starting point a parent, when it no longer has a parent we have reached the start
                        Cell parent = currentCell.parent;

                        if (parent.location[0] < currentCell.location[0])
                        {
                            if (currentCell.location[0] - parent.location[0] == 1)
                            {
                                commandList.Insert(0, "Right");
                            }
                            else
                            {
                                commandList.Insert(0, "Right Jump Distance of " + (currentCell.location[0] - parent.location[0]).ToString());
                            }
                        }
                        else if (parent.location[0] > currentCell.location[0])
                        {
                            if (parent.location[0] - currentCell.location[0] == 1)
                            {
                                commandList.Insert(0, "Left");
                            }
                            else
                            {
                                commandList.Insert(0, "Left Jump Distance of " + (parent.location[0] - currentCell.location[0]).ToString());
                            }
                        }
                        else if (parent.location[1] < currentCell.location[1])
                        {
                            if (currentCell.location[1] - parent.location[1] == 1)
                            {
                                commandList.Insert(0, "Down");
                            }
                            else
                            {
                                commandList.Insert(0, "Down Jump Distance of " + (currentCell.location[1] - parent.location[1]).ToString());
                            }
                        }
                        else if (parent.location[1] > currentCell.location[1])
                        {
                            if (parent.location[1] - currentCell.location[1] == 1)
                            {
                                commandList.Insert(0, "Up");
                            }
                            else
                            {
                                commandList.Insert(0, "Up Jump Distance of " + (parent.location[1] - currentCell.location[1]).ToString());
                            }
                        }

                        currentCell = parent;
                    }

                    for (int i = 0; i < commandList.Count; i++)
                    {
                        if (i == commandList.Count - 1)         //Check to see if we are at the last item in the array, to make sure the write is clean
                        {
                            Console.Write(commandList[i]);
                        }
                        else
                        {
                            Console.Write(commandList[i] + "; ");       //Write out the command, so that they all stay on the same line
                        }
                    }
                    Console.WriteLine();
                    return;
                }

                List<Cell> possibleMoves = new List<Cell>();
                Cell tempCell;

                if (agentCell.location[1] != 0)
                {
                    tempCell = map.jumpUp();
                    if(tempCell != null)
                    {
                        possibleMoves.Add(tempCell);
                    }
                }
                if (agentCell.location[0] != 0)
                {
                    tempCell = map.jumpLeft();
                    if( tempCell != null)
                    {
                        possibleMoves.Add(tempCell);
                    }
                }
                if (agentCell.location[1] != (map.layout[0].Count) - 1)
                {
                    tempCell = map.jumpDown();
                    if(tempCell != null)
                    {
                        possibleMoves.Add(tempCell);
                    }
                }
                if (agentCell.location[0] != (map.layout.Count) - 1)
                {
                    tempCell = map.jumpRight();
                    if(tempCell != null)
                    {
                        possibleMoves.Add(tempCell);
                    }
                }


                foreach (Cell cell in possibleMoves)
                {
                    if (!visitedCells.Contains(cell))
                    {
                        visitedCells.Add(cell);
                        cell.parent = agentCell;
                        queue.Enqueue(cell);
                    }
                }
            }

            Console.WriteLine("No Solution Found");
        }

    }
}
