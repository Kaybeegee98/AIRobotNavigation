using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RobotNavTest
{
    public partial class Program
    {

        static void greedySearch_Jump_Solution(TableMap map)
        {
            //Greedy Search: f(n) = h(n), where h(n) is how close the Agent is to the Goals
            HashSet<Cell> visitedCells = new HashSet<Cell>();       //Store Visited Cells in a Hashset to ensure no Duplicates
            Stack<Cell> cellStack = new Stack<Cell>();              //Using stack for LIFO
            List<string> commandList = new List<string>();

            int nodes = 0;                                          //Count the amount of Nodes Visited before Solution was found
            Cell startPoint = map.agentCell;                        //StartPoint for the search is equal to the Agents location
            cellStack.Push(startPoint);

            while (cellStack.Count > 0)
            {
                //Increase the amount of nodes visited. Make the agentCell equal to the last element in the Stack. Add the agentCell to visitedCells.
                nodes++;
                Cell agentCell = cellStack.Pop();
                visitedCells.Add(agentCell);

                map.updateAgent(agentCell);

                //Check to see if AgentCell is currently located in one of the maps Goals.
                if (map.goalCells.Contains(agentCell))
                {
                    Console.Write(nodes.ToString() + "\n");
                    Cell currentCell = agentCell;
                    while (currentCell.parent != null)      //Work backwards, from where the Agent Cell ended back to the start by checking if it has a Parent
                    {                                       //Since we never gave the starting point a parent, when it no longer has a parent we have reached the start
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

                List<Cell> selectedMoves = new List<Cell>();                //Create a Temp List to store our possible moves in
                Cell tempCell;

                if (agentCell.location[1] != 0)
                {
                    tempCell = map.jumpUp();
                    if (tempCell != null)
                    {
                        selectedMoves.Add(tempCell);
                    }
                }
                if ((agentCell.location[0] != 0) && !map.layout[agentCell.location[0] - 1][agentCell.location[1]].wall)
                {
                    tempCell = map.jumpLeft();
                    if (tempCell != null)
                    {
                        selectedMoves.Add(tempCell);
                    }
                }
                if ((agentCell.location[1] != (map.layout[0].Count) - 1) && !map.layout[agentCell.location[0]][agentCell.location[1] + 1].wall)
                {
                    tempCell = map.jumpDown();
                    if (tempCell != null)
                    {
                        selectedMoves.Add(tempCell);
                    }
                }
                if ((agentCell.location[0] != (map.layout.Count) - 1) && !map.layout[agentCell.location[0] + 1][agentCell.location[1]].wall)
                {
                    tempCell = map.jumpRight();
                    if (tempCell != null)
                    {
                        selectedMoves.Add(tempCell);
                    }
                }

                //Order the possible moves by whatever goalDistance is Largest, then add the move cost to determine what move is truly the best (since using LIFO, best option needs to be last)
                selectedMoves = selectedMoves.OrderBy(Cell => Cell.totalCost()).ToList();
                selectedMoves.Reverse();

                if (selectedMoves != null)
                {
                    foreach (Cell c in selectedMoves)
                    {
                        if (!(visitedCells.Contains(c)))        //As long as the possible move does not place us a visited cell, add it to the Stack.
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
