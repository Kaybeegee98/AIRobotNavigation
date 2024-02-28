﻿using System;
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
        static void bfs_Solution(TableMap map)
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
                    while(currentCell.parent != null)
                    {
                        Cell parent = currentCell.parent;

                        if (parent.location[0] < currentCell.location[0])
                        {
                            commandList.Insert(0, "Right");
                        }
                        else if(parent.location[0] > currentCell.location[0])
                        {
                            commandList.Insert(0, "Left");
                        }
                        else if (parent.location[1] < currentCell.location[1])
                        {
                            commandList.Insert(0, "Down");
                        }
                        else if (parent.location[1] > currentCell.location[1])
                        {
                            commandList.Insert(0, "Up");
                        }

                        currentCell = parent;
                    }

                    for (int i = 0; i < commandList.Count; i++)
                    {
                        if(i == commandList.Count - 1)
                        {
                            Console.Write(commandList[i]);
                        }
                        else
                        {
                            Console.Write(commandList[i] + "; ");
                        }
                    }
                    Console.WriteLine();
                    return;
                }

                List<Cell> possibleMoves = new List<Cell>();

                if ((agentCell.location[1] != 0) && !map.layout[agentCell.location[0]][agentCell.location[1] - 1].wall)
                {
                    possibleMoves.Add(map.moveUp());
                }
                if ((agentCell.location[0] != 0) && !map.layout[agentCell.location[0] - 1][agentCell.location[1]].wall)
                {
                    possibleMoves.Add(map.moveLeft());
                }
                if ((agentCell.location[1] != (map.layout[0].Count) - 1) && !map.layout[agentCell.location[0]][agentCell.location[1] + 1].wall)
                {
                    possibleMoves.Add(map.moveDown());
                }
                if ((agentCell.location[0] != (map.layout.Count) - 1) && !map.layout[agentCell.location[0] + 1][agentCell.location[1]].wall)
                {
                    possibleMoves.Add(map.moveRight());
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
