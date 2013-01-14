using System;

using System.Text;
using System.Diagnostics;

namespace GameOfTrolls
{
	public struct element
	{
		public int Value;
		public UInt16 Cost;
		public UInt16 Row;
		public UInt16 Col;
		public bool Right, Up, Left, Down; 
		public int ValueCoefficient;
		public bool isRased;

		public element(int val, UInt16 cost, UInt16 row, UInt16 col, bool pIsRased = false,
		               bool right = false, bool up = false, bool left = false, bool down = false)
	   	{
			Value = val;
			Cost = cost;
			Row = row;
			Col = col;
			Right = right;
			Up = up;
			Left = left;
			Down = down;
			isRased = pIsRased;
	   		if (Cost != 0)
				ValueCoefficient = Value/Cost;
			else
				ValueCoefficient = 0;
		}
	}

	class MainClass
	{
		static UInt16 turns, dim;
		static StringBuilder resultString;

		public static void Main (string[] args)
		{
			UserInterface userInterface = new UserInterface();

			resultString = new StringBuilder ();
			int[,] userRawMatrix;
			userRawMatrix = UserInterface.ColectInformation();
			dim = userInterface.getDimention();
			turns = userInterface.getTurns();
	        int n = turns;
	        int[,] startMatrix = new int[dim, dim];
	        Array.Copy(userRawMatrix, startMatrix, userRawMatrix.Length);

			UserInterface.DrowHtmlStart (userRawMatrix);

			new Algorithm(resultString, turns, dim);

			DateTime startTime = DateTime.Now; // start timer

			if (dim == 1)
			{
				Algorithm.SolveOneByOneMatrix(userRawMatrix);
			}
			else
			{
				Algorithm.Calculate (userRawMatrix);
			}

			Console.WriteLine(resultString);

	        UserInterface.DrowTurns(startMatrix, resultString, n);
	        DateTime endTime = DateTime.Now; // end timer
	        UserInterface.DrowHtmlEnd();
	        Process.Start("index.html");
	        Console.WriteLine(endTime.Subtract(startTime));
		}
	}
}
