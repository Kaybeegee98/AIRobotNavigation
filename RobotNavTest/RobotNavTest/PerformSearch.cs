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

        static void PerformSearch(String command, TableMap tableMap)                    //Go through all possible known Search Implementations, if none selected return error
        {
            //Each line checks to see if the second arguement is one of the possible searchs (ToUpper to make sure that caps don't matter)
            if (command.ToUpper().Contains("DFS") && command.ToUpper().Contains("JUMP"))
            {
                Console.Write(" DFS Jump ");
                dfs_Jump_Solution(tableMap);
            }
            else if (command.ToUpper().Contains("DFS"))
            {
                Console.Write(" DFS ");
                dfs_Solution(tableMap);
            }
            else if (command.ToUpper().Contains("BFS") && command.ToUpper().Contains("JUMP"))
            {
                Console.Write(" BFS Jump ");
                bfs_Jump_Solution(tableMap);
            }
            else if (command.ToUpper().Contains("BFS"))
            {
                Console.Write(" BFS ");
                bfs_Solution(tableMap);
            }
            else if (command.ToUpper().Contains("GREEDY") && command.ToUpper().Contains("JUMP"))
            {
                Console.Write(" Greedy Best First Search Jump ");
                greedySearch_Jump_Solution(tableMap);
            }
            else if (command.ToUpper().Contains("GREEDY"))
            {
                Console.Write(" Greedy Best First Search ");
                greedySearch_Solution(tableMap);
            }
            else if ((command.ToUpper().Contains("A*") || command.ToUpper().Contains("ASTAR")) && command.ToUpper().Contains("JUMP"))
            {
                Console.Write(" A* Jump ");
                aStar_Jump_Solution(tableMap);
            }
            else if (command.ToUpper().Contains("A*") || command.ToUpper().Contains("ASTAR"))
            {
                Console.Write(" A* ");
                aStar_Solution(tableMap);
            }
            else if (command.ToUpper().Contains("BIDIRECTIONAL"))
            {
                Console.Write(" Bidirectional ");
                bidirectional_Solution(tableMap);
            }
            else
            {
                Console.WriteLine("Error: Incompatible Method type. Check spelling and try again.");
            }
        }


    }
}
