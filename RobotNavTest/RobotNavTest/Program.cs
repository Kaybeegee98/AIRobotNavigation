namespace RobotNavTest
{
    public partial class Program
    {
        static void Main(string[] args)
        {
            if(args.Length != 2)        //Check if 2 Arguements are being accepted
            {
                Console.WriteLine("Error: Incorrect number of Arguments. Require 1 File name and 1 Method option.");
                return;
            }

            int counter = 1;                                            // Used to keep track of which line we are on in the file
            char[] delimiters = { '[', ',', ']', '(', ')', '|' , ' '};       // Array of delimiters needed since there isn't just one


            List<List<Cell>> tempMap = new List<List<Cell>>();

            // Read through each line of the file
            foreach (string line in File.ReadLines(args[0]))
            {
                List<string> list = new List<string>();

                //Switch to detect how far through the file we are.
                switch(counter)
                {
                    case 1:
                        //The first line is always the Length and Width of the Table.
                        foreach (string word in line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries))
                        {
                            list.Add(word);
                        }

                        for(int i = 0; i < short.Parse(list[1]); i++)       //Creates two loops. The first loop is for the Y coordinates, and the second loop is for the X coordinates.
                        {
                            List<Cell> tempRow = new List<Cell>();
                            for(int j = 0; j < short.Parse(list[0]); j++)
                            {
                                int[] position = { i, j };
                                tempRow.Add(new Cell(position));            //Creates a new cell at the place of (i, j)
                            }
                            tempMap.Add(tempRow);                           //Adds the entire row to the List<List<Cells>>
                        }
                        counter++;
                        break;

                    case 2:
                        //The second line is the location of the Agent
                        foreach (string word in line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries))
                        {
                            list.Add(word);
                        }
                        tempMap[short.Parse(list[0])][short.Parse(list[1])].robotHere = true;
                        counter++;
                        break;

                    case 3:
                        //The third line is the Goal Locations
                        foreach (string word in line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries))
                        {
                            list.Add(word);
                        }
                        while (list.Count > 0)      //Loops through the list until no Goals remain
                        {
                            tempMap[short.Parse(list[0])][short.Parse(list[1])].goal = true;
                            list.RemoveRange(0, 2);         //Removes the first two objects in the list, meaning it should avoid repeat goals
                        }

                        counter++;
                        break;

                    case 4:
                        //Every other line describes a different wall
                        foreach (string word in line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries))
                        {
                            list.Add(word);
                        }
                        int w = short.Parse(list[0]);
                        int x = short.Parse(list[1]);
                        int y = (short.Parse(list[2]) - 1);
                        int z = (short.Parse(list[3]) - 1);

                        tempMap[w][x].wall = true;

                        if (y != 0)
                        {
                            for (int i = 1; i < y + 1; i++)
                            {
                                tempMap[w + i][x].wall = true;      
                            }

                            if (z != 0)
                            {
                                tempMap[w + y][x + z].wall = true;
                            }
                        } 
                        
                        if (z != 0)
                        {
                            for (int i = 1; i  < z + 1; i++)
                            {
                                tempMap[w][x + i].wall = true;
                            }
                        }

                        break;
                }
            }

            TableMap tableMap = new TableMap(tempMap);          //Use tempMap to create the proper TableMap

            Console.Write(args[0]);

            PerformSearch(args[1], tableMap);       //Perform the search using the entered command and the TableMap
        }
    }
}