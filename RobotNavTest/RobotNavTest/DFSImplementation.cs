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
        static void dfs_Solution(TableMap map)
        {
            HashSet<Cell> visitedCells = new HashSet<Cell>();
            Stack<Cell> cellStack = new Stack<Cell>();
            List<string> commandList = new List<string>();

            int nodes = 0;
            Cell startPoint = map.agentCell;
            cellStack.Push(startPoint);

            while (cellStack.Count > 0)
            {
                nodes++;
                Cell agentCell = cellStack.Pop();
                visitedCells.Add(agentCell);

                map.updateAgent(agentCell);

                if (map.goalCells.Contains(agentCell))
                {
                    Console.Write(nodes.ToString() + "\n");
                    Cell currentCell = agentCell;
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

                List<Cell> selectedMoves = new List<Cell>();

                if ((agentCell.location[0] != (map.layout.Count) - 1) && !map.layout[agentCell.location[0] + 1][agentCell.location[1]].wall)
                {
                    selectedMoves.Add(map.moveRight());
                }
                if ((agentCell.location[1] != (map.layout[0].Count) - 1) && !map.layout[agentCell.location[0]][agentCell.location[1] + 1].wall)
                {
                    selectedMoves.Add(map.moveDown());
                }
                if ((agentCell.location[0] != 0) && !map.layout[agentCell.location[0] - 1][agentCell.location[1]].wall)
                {
                    selectedMoves.Add(map.moveLeft());
                }
                if ((agentCell.location[1] != 0) && !map.layout[agentCell.location[0]][agentCell.location[1] - 1].wall)
                {
                    selectedMoves.Add(map.moveUp());
                }



                if (selectedMoves != null)
                {
                    foreach(Cell c in selectedMoves)
                    {
                        if (!(visitedCells.Contains(c)))
                        {
                            c.parent = agentCell;
                            cellStack.Push(c);
                        }
                    }
                }
            }

            Console.WriteLine("No Solution Found");
        }

    }
}
