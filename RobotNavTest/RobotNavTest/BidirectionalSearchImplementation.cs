using System;
using System.Collections;
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
        static void bidirectional_Solution(TableMap map)
        {
            HashSet<Cell> forwardVisited = new HashSet<Cell>();
            HashSet<Cell> backwardOneVisited = new HashSet<Cell>();
            HashSet<Cell> backwardTwoVisited = new HashSet<Cell>();

            TableMap backwardOneMap = new TableMap(map.layout);
            TableMap backwardTwoMap = new TableMap(map.layout);

            Queue<Cell> forwardQueue = new Queue<Cell>();
            Queue<Cell> backwardOneQueue = new Queue<Cell>();
            Queue<Cell> backwardTwoQueue = new Queue<Cell>();

            Cell forwardAgent;
            Cell backwardOneAgent;
            Cell backwardTwoAgent;

            Cell lastUniqueCell = new Cell(new int[] {0, 0}); ;

            List<string> commandList = new List<string>();

            int nodes = 0;

            Cell startPoint = map.agentCell;
            forwardVisited.Add(startPoint);
            forwardQueue.Enqueue(startPoint);

            backwardOneVisited.Add(map.goalCells[0]);
            backwardOneQueue.Enqueue(map.goalCells[0]);

            backwardTwoVisited.Add(map.goalCells[1]);
            backwardTwoQueue.Enqueue(map.goalCells[1]);

            while (forwardQueue.Count > 0 && backwardOneQueue.Count > 0 && backwardTwoQueue.Count > 0)
            {
                nodes = nodes + 3;
                forwardAgent = forwardQueue.Dequeue();
                backwardOneAgent = backwardOneQueue.Dequeue();
                backwardTwoAgent = backwardTwoQueue.Dequeue();

                map.updateAgent(forwardAgent);
                backwardOneMap.updateAgent(backwardOneAgent);
                backwardTwoMap.updateAgent(backwardTwoAgent);

                foreach (Cell c in backwardOneVisited)
                {
                    foreach (Cell d in forwardVisited)
                    {
                        if (c.location.SequenceEqual(d.location))
                        {
                            List<Cell> visitedForwardList = forwardVisited.ToList();
                            List<Cell> visitedBackwardList = backwardOneVisited.ToList();

                            Cell forwardCurrent;
                            Cell backwardCurrent;

                            foreach (Cell forCell in visitedForwardList)
                            {
                                if (forCell.location == lastUniqueCell.location)
                                {
                                    forwardCurrent = forCell;
                                }
                            }
                            int please = -1;

                            foreach (Cell backCell in visitedBackwardList)
                            {
                                please++;
                                if (backCell.location == d.location)
                                {
                                    backwardCurrent = backCell;
                                }
                            }

                            Console.Write(nodes.ToString() + "\n");

                            Cell currentCell = forwardAgent;
                            while (currentCell.parent != null)
                            {
                                Cell parent = currentCell.parent;

                                if (parent.location[0] < currentCell.location[0])
                                {
                                    commandList.Insert(0, "Right");
                                }
                                else if (parent.location[0] > currentCell.location[0])
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


                            currentCell = backwardOneAgent;
                            while (currentCell.parent != null)
                            {
                                Cell parent = currentCell.parent;

                                if (parent.location[0] < currentCell.location[0])
                                {
                                    commandList.Add("Left");
                                }
                                else if (parent.location[0] > currentCell.location[0])
                                {
                                    commandList.Add("Right");
                                }
                                else if (parent.location[1] < currentCell.location[1])
                                {
                                    commandList.Add("Up");
                                }
                                else if (parent.location[1] > currentCell.location[1])
                                {
                                    commandList.Add("Down");
                                }

                                currentCell = parent;
                            }

                            for (int i = 0; i < commandList.Count; i++)
                            {
                                if (i == commandList.Count - 1)
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
                    }
                }

                foreach (Cell c in backwardTwoVisited)
                {
                    if (forwardVisited.Contains(c))
                    {
                        Console.WriteLine("Solution Found in Backward Two Visited");
                        return;
                    }
                }

                List<Cell> forwardSelectedMoves = new List<Cell>();
                List<Cell> backwardOneSelectedMoves = new List<Cell>();
                List<Cell> backwardTwoSelectedMoves = new List<Cell>();

                if ((forwardAgent.location[1] != 0) && !map.layout[forwardAgent.location[0]][forwardAgent.location[1] - 1].wall)
                {
                    forwardSelectedMoves.Add(map.moveUp());
                }
                if ((forwardAgent.location[0] != 0) && !map.layout[forwardAgent.location[0] - 1][forwardAgent.location[1]].wall)
                {
                    forwardSelectedMoves.Add(map.moveLeft());
                }
                if ((forwardAgent.location[1] != (map.layout[0].Count) - 1) && !map.layout[forwardAgent.location[0]][forwardAgent.location[1] + 1].wall)
                {
                    forwardSelectedMoves.Add(map.moveDown());
                }
                if ((forwardAgent.location[0] != (map.layout.Count) - 1) && !map.layout[forwardAgent.location[0] + 1][forwardAgent.location[1]].wall)
                {
                    forwardSelectedMoves.Add(map.moveRight());
                }

                if ((backwardOneAgent.location[0] != (backwardOneMap.layout.Count) - 1) && !backwardOneMap.layout[backwardOneAgent.location[0] + 1][backwardOneAgent.location[1]].wall)
                {
                    backwardOneSelectedMoves.Add(backwardOneMap.moveRight());
                }
                if ((backwardOneAgent.location[1] != (backwardOneMap.layout[1].Count) - 1) && !backwardOneMap.layout[backwardOneAgent.location[0]][backwardOneAgent.location[1] + 1].wall)
                {
                    backwardOneSelectedMoves.Add(backwardOneMap.moveDown());
                }
                if ((backwardOneAgent.location[0] != 0) && !backwardOneMap.layout[backwardOneAgent.location[0] - 1][backwardOneAgent.location[1]].wall)
                {
                    backwardOneSelectedMoves.Add(backwardOneMap.moveLeft());
                }
                if ((backwardOneAgent.location[1] != 0) && !backwardOneMap.layout[backwardOneAgent.location[0]][backwardOneAgent.location[1] - 1].wall)
                {
                    backwardOneSelectedMoves.Add(backwardOneMap.moveUp());
                }

                if ((backwardTwoAgent.location[0] != (map.layout.Count) - 1) && !map.layout[backwardTwoAgent.location[0] + 1][backwardTwoAgent.location[1]].wall)
                {
                    backwardTwoSelectedMoves.Add(backwardTwoMap.moveRight());
                }
                if ((backwardTwoAgent.location[1] != (backwardTwoMap.layout[1].Count) - 1) && !backwardTwoMap.layout[backwardTwoAgent.location[0]][backwardTwoAgent.location[1] + 1].wall)
                {
                    backwardTwoSelectedMoves.Add(backwardTwoMap.moveDown());
                }
                if ((backwardTwoAgent.location[0] != 0) && !backwardTwoMap.layout[backwardTwoAgent.location[0] - 1][backwardTwoAgent.location[1]].wall)
                {
                    backwardTwoSelectedMoves.Add(backwardTwoMap.moveLeft());
                }
                if ((backwardTwoAgent.location[1] != 0) && !backwardTwoMap.layout[backwardTwoAgent.location[0]][backwardTwoAgent.location[1] - 1].wall)
                {
                    backwardTwoSelectedMoves.Add(backwardTwoMap.moveUp());
                }

                foreach (Cell vFC in forwardSelectedMoves)
                {
                    if (!forwardVisited.Contains(vFC))
                    {
                        forwardVisited.Add(vFC);
                        if(vFC.parent == null)
                        {
                            vFC.parent = forwardAgent;
                        }
                        else
                        {
                            lastUniqueCell = forwardAgent;
                        }
                        forwardQueue.Enqueue(vFC);
                    }
                }

                foreach (Cell vBCO in backwardOneSelectedMoves)
                {
                    if (!backwardOneVisited.Contains(vBCO))
                    {
                        backwardOneVisited.Add(vBCO);
                        if(vBCO.parent == null && vBCO.location != backwardOneMap.goalCells[1].location)
                        {
                            vBCO.parent = backwardOneAgent;
                        }
                        backwardOneQueue.Enqueue(vBCO);
                    }
                }

                foreach (Cell vBCT in backwardTwoSelectedMoves)
                {
                    if (!backwardTwoVisited.Contains(vBCT))
                    {
                        backwardTwoVisited.Add(vBCT);
                        if(vBCT.parent == null && vBCT.location != backwardTwoMap.goalCells[0].location)
                        {
                            vBCT.parent = backwardTwoAgent;
                        }
                        backwardTwoQueue.Enqueue(vBCT);
                    }
                }
            }
        }

    }
}
