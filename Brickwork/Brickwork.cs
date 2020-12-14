using System;
using System.Collections.Generic;
using System.Linq;

namespace Brickwork2
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                // Read the dimensions of the matrix.
                int[] dimensions = Console.ReadLine().Split().Select(int.Parse).ToArray();

                // Validate the dimensions.
                if (dimensions[0] >= 100 || dimensions[0] % 2 != 0 || dimensions[1] >= 100 || dimensions[1] % 2 != 0)
                {
                    throw new ArgumentException("The dimensions of the matrix are not valid!");
                }

                int numberOfBricks = 0;

                // Create the matrix.
                int[,] firstLayer = new int[dimensions[0], dimensions[1]];
                int[,] secondLayer = new int[dimensions[0], dimensions[1]];

                // Read the matrix.
                for (int row = 0; row < firstLayer.GetLength(0); row++)
                {
                    string colEls = Console.ReadLine();
                    // Validate number of rows.
                    if (colEls.Length == 0)
                    {
                        throw new ArgumentException("The number of given rows does not match the number of rows expected in the matrix!");
                    }

                    int[] colElements = colEls.Split().Select(int.Parse).ToArray();

                    // Validate number of columns.
                    if (colElements.Length != dimensions[1])
                    {
                        throw new ArgumentException("The number of given columns does not match the number of columns expected in the matrix!");
                    }

                    for (int col = 0; col < firstLayer.GetLength(1); col++)
                    {
                        firstLayer[row, col] = colElements[col];
                        secondLayer[row, col] = colElements[col];
                        int currentNumber = colElements[col];
                        if (currentNumber > numberOfBricks)
                        {
                            numberOfBricks = currentNumber;
                        }
                    }
                }

                string colElems = Console.ReadLine();
                // Validate number of rows.
                if (colElems.Length > 0)
                {
                    throw new ArgumentException("The number of given rows does not match the number of rows expected in the matrix!");
                }

                // Validate if brick is spanning three columns
                for (int i = 0; i < firstLayer.GetLength(0); i++)
                {
                    for (int j = 0; j < firstLayer.GetLength(1) - 2; j++)
                    {
                        int currentBrickNumber = firstLayer[i, j];

                        if (firstLayer[i, j + 1] == currentBrickNumber && firstLayer[i, j + 2] == currentBrickNumber)
                        {
                            throw new ArgumentException("There is a brick spanning three columns!");
                        }
                    }
                }

                // Validate if brick is spanning three rows
                for (int i = 0; i < firstLayer.GetLength(0) - 2; i++)
                {
                    for (int j = 0; j < firstLayer.GetLength(1); j++)
                    {
                        int currentBrickNumber = firstLayer[i, j];

                        if (firstLayer[i + 1, j] == currentBrickNumber && firstLayer[i + 2, j] == currentBrickNumber)
                        {
                            throw new ArgumentException("There is a brick spanning three rows!");
                        }
                    }
                }

                // Placed bricks
                List<int> placedBricks = new List<int>();

                // Bricks to be placed
                List<int> bricksToBePlaced = new List<int>();
                bricksToBePlaced.AddRange(Enumerable.Range(1, numberOfBricks));

                // Brick at position [0, 0].
                int[] firstBrickPartPosition = { 0, 0 };

                // Number of the brick
                int brickNumber = firstLayer[firstBrickPartPosition[0], firstBrickPartPosition[1]];

                // Searching the other part of the brick
                int[] secondBrickPartPosition = { -1, -1 };

                if (firstLayer[0, 0] == firstLayer[0, 1])
                {
                    secondBrickPartPosition = new int[] { 0, 1 };
                }
                else
                {
                    secondBrickPartPosition = new int[] { 1, 0 };
                }

                // Lifting the brick
                secondLayer[firstBrickPartPosition[0], firstBrickPartPosition[1]] = -brickNumber;
                secondLayer[secondBrickPartPosition[0], secondBrickPartPosition[1]] = -brickNumber;

                // Storing data about the positions and their values over which the current brick is placed for reverting purposes
                int index = -1;
                List<int[]> firstBrickPartPositionsList = new List<int[]>();
                List<int[]> secondBrickPartPositionsList = new List<int[]>();
                List<int> numberOfFirstBrickPartList = new List<int>();
                List<int> numberOfSecondBrickPartList = new List<int>();

                // Placing the brick
                bool isSolutionExisting = PlaceBrick(firstLayer, secondLayer, placedBricks, bricksToBePlaced, firstBrickPartPosition, ref brickNumber, secondBrickPartPosition, ref index,
                    firstBrickPartPositionsList, secondBrickPartPositionsList, ref numberOfFirstBrickPartList, ref numberOfSecondBrickPartList);
                if (!isSolutionExisting)
                {
                    Console.WriteLine("-1, There is no solution!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static bool PlaceBrick(int[,] firstLayer, int[,] secondLayer, List<int> placedBricks, List<int> bricksToBePlaced, int[] firstBrickPartPosition, ref int brickNumber, int[] secondBrickPartPosition, ref int index,
             List<int[]> firstBrickPartPositionsList, List<int[]> secondBrickPartPositionsList, ref List<int> numberOfFirstBrickPartList, ref List<int> numberOfSecondBrickPartList)
        {
            // Finding if solution exists
            bool isSolutionExisting = false;

            for (int i = 0; i < secondLayer.GetLength(0); i++)
            {
                for (int j = 0; j < secondLayer.GetLength(1); j++)
                {
                    if (!placedBricks.Contains(secondLayer[i, j]))
                    {
                        firstBrickPartPosition[0] = i;
                        firstBrickPartPosition[1] = j;
                        secondBrickPartPosition[0] = -1;
                        secondBrickPartPosition[1] = -1;

                        // Checking if the second part of the brick can but put down to the first part
                        secondBrickPartPosition = CheckRight(firstLayer, secondLayer, firstBrickPartPosition, placedBricks);
                        // Placing the brick
                        PlacingTheBrick(firstLayer, secondLayer, placedBricks, bricksToBePlaced, firstBrickPartPosition, ref brickNumber, secondBrickPartPosition, ref index, firstBrickPartPositionsList, secondBrickPartPositionsList, ref numberOfFirstBrickPartList, ref numberOfSecondBrickPartList, ref isSolutionExisting);

                        // Checking if the second part of the brick can but put right to the first part
                        secondBrickPartPosition = CheckDown(firstLayer, secondLayer, firstBrickPartPosition, placedBricks);

                        // Placing the brick
                        PlacingTheBrick(firstLayer, secondLayer, placedBricks, bricksToBePlaced, firstBrickPartPosition, ref brickNumber, secondBrickPartPosition, ref index, firstBrickPartPositionsList, secondBrickPartPositionsList, ref numberOfFirstBrickPartList, ref numberOfSecondBrickPartList, ref isSolutionExisting);
                    }
                }
            }

            // Unlifting the brick -> reverting
            for (int k = 0; k < secondLayer.GetLength(0); k++)
            {
                for (int l = 0; l < secondLayer.GetLength(1); l++)
                {
                    if (secondLayer[k, l] == -brickNumber)
                    {
                        secondLayer[k, l] = brickNumber;
                    }
                }
            }

            // Going back to the previous brick
            placedBricks.Sort();
            brickNumber = placedBricks.LastOrDefault();

            // We reached starting position
            if (placedBricks.Count == 0)
            {
                return isSolutionExisting;
            }

            index = brickNumber - 1;
            secondLayer[firstBrickPartPositionsList[index][0], firstBrickPartPositionsList[index][1]] = numberOfFirstBrickPartList[index];
            secondLayer[secondBrickPartPositionsList[index][0], secondBrickPartPositionsList[index][1]] = numberOfSecondBrickPartList[index];
            firstBrickPartPositionsList.RemoveAt(index);
            secondBrickPartPositionsList.RemoveAt(index);
            numberOfFirstBrickPartList.RemoveAt(index);
            numberOfSecondBrickPartList.RemoveAt(index);
            placedBricks.Remove(brickNumber);
            bricksToBePlaced.Add(brickNumber);
            index = brickNumber - 2;


            return isSolutionExisting;
        }

        private static void PlacingTheBrick(int[,] firstLayer, int[,] secondLayer, List<int> placedBricks, List<int> bricksToBePlaced, int[] firstBrickPartPosition, ref int brickNumber, int[] secondBrickPartPosition, ref int index, List<int[]> firstBrickPartPositionsList, List<int[]> secondBrickPartPositionsList, ref List<int> numberOfFirstBrickPartList, ref List<int> numberOfSecondBrickPartList, ref bool isSolutionExisting)
        {
            if (firstBrickPartPosition[0] != -1 & firstBrickPartPosition[1] != -1 && secondBrickPartPosition[0] != -1 && secondBrickPartPosition[1] != -1)
            {
                int numberOfFirstBrickPart = secondLayer[firstBrickPartPosition[0], firstBrickPartPosition[1]];
                int numberOfSecondBrickPart = secondLayer[secondBrickPartPosition[0], secondBrickPartPosition[1]];

                index++;
                int[] firstValue = { firstBrickPartPosition[0], firstBrickPartPosition[1] };
                int[] secondValue = { secondBrickPartPosition[0], secondBrickPartPosition[1] };
                firstBrickPartPositionsList.Add(firstValue);
                secondBrickPartPositionsList.Add(secondValue);
                numberOfFirstBrickPartList.Add(numberOfFirstBrickPart);
                numberOfSecondBrickPartList.Add(numberOfSecondBrickPart);

                secondLayer[firstBrickPartPosition[0], firstBrickPartPosition[1]] = brickNumber;
                secondLayer[secondBrickPartPosition[0], secondBrickPartPosition[1]] = brickNumber;

                placedBricks.Add(brickNumber);
                bricksToBePlaced.Remove(brickNumber);

                if (bricksToBePlaced.Count == 0)
                {
                    Print(secondLayer);
                    // Unplacing the brick -> reverting
                    secondLayer[firstBrickPartPositionsList[index][0], firstBrickPartPositionsList[index][1]] = numberOfFirstBrickPartList[index];
                    secondLayer[secondBrickPartPositionsList[index][0], secondBrickPartPositionsList[index][1]] = numberOfSecondBrickPartList[index];
                    firstBrickPartPositionsList.RemoveAt(index);
                    secondBrickPartPositionsList.RemoveAt(index);
                    numberOfFirstBrickPartList.RemoveAt(index);
                    numberOfSecondBrickPartList.RemoveAt(index);
                    placedBricks.Remove(brickNumber);
                    bricksToBePlaced.Add(brickNumber);

                    isSolutionExisting = true;
                }
                else
                {
                    bricksToBePlaced.Sort();
                    brickNumber = bricksToBePlaced.FirstOrDefault();

                    LiftNextBrick(secondLayer, brickNumber);
                    firstBrickPartPosition[0] = -1;
                    firstBrickPartPosition[1] = -1;
                    secondBrickPartPosition[0] = -1;
                    secondBrickPartPosition[1] = -1;

                    isSolutionExisting = PlaceBrick(firstLayer, secondLayer, placedBricks, bricksToBePlaced, firstBrickPartPosition, ref brickNumber, secondBrickPartPosition, ref index,
                         firstBrickPartPositionsList, secondBrickPartPositionsList, ref numberOfFirstBrickPartList, ref numberOfSecondBrickPartList);
                }
            }
        }

        // Checking if the second part of the brick can but put down to the first part
        private static int[] CheckDown(int[,] firstLayer, int[,] secondLayer, int[] firstBrickPartPosition, List<int> placedBricks)
        {
            if (firstBrickPartPosition[0] + 1 < firstLayer.GetLength(0)
                && firstLayer[firstBrickPartPosition[0], firstBrickPartPosition[1]] != firstLayer[firstBrickPartPosition[0] + 1, firstBrickPartPosition[1]]
                && !placedBricks.Contains(secondLayer[firstBrickPartPosition[0] + 1, firstBrickPartPosition[1]]))
            {
                return new int[] { firstBrickPartPosition[0] + 1, firstBrickPartPosition[1] };
            }
            else
            {
                return new int[] { -1, -1 };
            }
        }

        // Checking if the second part of the brick can but put right to the first part
        private static int[] CheckRight(int[,] firstLayer, int[,] secondLayer, int[] firstBrickPartPosition, List<int> placedBricks)
        {
            if (firstBrickPartPosition[1] + 1 < firstLayer.GetLength(1)
                && firstLayer[firstBrickPartPosition[0], firstBrickPartPosition[1]] != firstLayer[firstBrickPartPosition[0], firstBrickPartPosition[1] + 1]
                && !placedBricks.Contains(secondLayer[firstBrickPartPosition[0], firstBrickPartPosition[1] + 1]))
            {
                return new int[] { firstBrickPartPosition[0], firstBrickPartPosition[1] + 1 };
            }
            else
            {
                return new int[] { -1, -1 };
            }
        }

        // Lifting the next brick
        private static void LiftNextBrick(int[,] secondLayer, int brickNumber)
        {
            for (int i = 0; i < secondLayer.GetLength(0); i++)
            {
                for (int j = 0; j < secondLayer.GetLength(1); j++)
                {
                    if (secondLayer[i, j] == brickNumber)
                    {
                        secondLayer[i, j] = -brickNumber;
                    }
                }
            }
        }

        // Printing the matrix.
        private static void Print(int[,] secondLayer)
        {
            for (int row = 0; row < secondLayer.GetLength(0); row++)
            {
                for (int col = 0; col < secondLayer.GetLength(1); col++)
                {
                    Console.Write("{0} ", secondLayer[row, col]);
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}