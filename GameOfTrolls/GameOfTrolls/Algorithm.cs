using System;
using System.Collections.Generic;
using System.Text;

namespace GameOfTrolls
{
	public class Algorithm
	{
		static StringBuilder resultString;
		static UInt16 turns, dim;

		public Algorithm ()
		{
		
		}

		public Algorithm (StringBuilder sb, UInt16 t, UInt16 d)
		{
			resultString = sb;
			turns = t;
			dim = d;
		}

		public static void SolveOneByOneMatrix(int[,] matrix)
		{
			for (int turn = 0; turn < turns; turn++)
			{
				resultString.Append("take ").Append(0).Append(" ").Append(0).Append("\n");
			}
		}

		private static element findMinElement (int[,] matrix)
		{
			for (UInt16 i = 0; i < dim; i++)
			{
				for (UInt16 j = 0; j < dim; j++)
				{
					if (matrix[i,j] >= turns)
						return new element(turns, (UInt16) turns, i, j, false, false, false, false, false);
				}	
			}

	       return new element(0, 0, 0, 0, false, false, false, false, false);
		}

		public static void Calculate (int[,] matrix)
		{
			int n = dim;

			List<element> turnTable = new List<element> ();

			for (int i = 0; i < n; i++) {
				for (int j = 0; j < n; j++) {
					int maxRow = i;
					int maxCol = j;

					int[,] croppedMatrix = new int[3, 3];

					if (maxRow == 0)
						croppedMatrix [0, 1] = 0;
					else
						croppedMatrix [0, 1] = matrix [maxRow - 1, maxCol];

					if (maxRow == n - 1)
						croppedMatrix [2, 1] = 0;
					else
						croppedMatrix [2, 1] = matrix [maxRow + 1, maxCol];

					if (maxCol == 0)
						croppedMatrix [1, 0] = 0;
					else
						croppedMatrix [1, 0] = matrix [maxRow, maxCol - 1];

					if (maxCol == n - 1)
						croppedMatrix [1, 2] = 0;
					else
						croppedMatrix [1, 2] = matrix [maxRow, maxCol + 1];

					croppedMatrix [1, 1] = matrix [maxRow, maxCol];

					croppedMatrix [0, 0] = 0;
					croppedMatrix [0, 2] = 0;
					croppedMatrix [2, 0] = 0;
					croppedMatrix [2, 2] = 0;

					element[] moves = ResultInMatrixForTurnsAtLocation (croppedMatrix, turns, (UInt16)maxRow, (UInt16)maxCol);
					turnTable.Add (moves [0]);
					turnTable.Add (moves [1]);
				}
			}

			turnTable.Sort
			(
			    delegate(element el1, element el2)
				{
					int compareElements = el2.ValueCoefficient.CompareTo (el1.ValueCoefficient);
					return compareElements;
				}
			);



			for (int i = 0; i < turnTable.Capacity; i++)
			{
				element e;
				try {
					e = turnTable [0];
					if (e.ValueCoefficient == 0)
						throw new Exception();
				} catch {
					element winMove = findMinElement (matrix);
					if (winMove.Value == 0)
						break;
					for (int turnsLeft = 0; turnsLeft < turns; turnsLeft++) {
						resultString.Append ("take ").Append (winMove.Row).Append (" ").Append (winMove.Col).Append ("\n");	
					}
					break;
				}
				if (e.Cost <= turns && e.Value != 0)
				{
					if (isCollisionAffected (matrix, e) == false)
					{
						ExecuteTurn (matrix, e);
						turns -= e.Cost;
						if (e.isRased) {
							for (int turn = 0; turn < (int)e.Cost; turn++) {
								resultString.Append ("put ").Append (e.Row).Append (" ").Append (e.Col).Append ("\n");
							}
						} else {
							for (int turn = 0; turn < (int)e.Cost; turn++) {
								resultString.Append ("take ").Append (e.Row).Append (" ").Append (e.Col).Append ("\n");
							}
						}

					}
				} else {
					if (turns == 0)
						break;
				}

				turnTable.RemoveAt (0);
			}
	    }

		private static element ElementAfterTurn (int[,] matrix, UInt16 row, UInt16 col, int Moves)
		{
			element elementPack = new element ();
			elementPack.Row = row;
			elementPack.Col = col;
			elementPack.Cost = (UInt16)Math.Abs(Moves);
			elementPack.Value = CalculateSum (matrix);
			elementPack.isRased = (Moves > 0);
			matrix [1, 1] += Moves;

			bool killLastTouchedTower = false;

			UInt16 i = 1 , j = 1;

			if (matrix [i, j] == matrix [i + 1, j] && matrix[i,j] != 0)
			{
				matrix [i + 1, j] = 0;
				killLastTouchedTower = true;
				elementPack.Down = true;
			}
			if (matrix[i, j] == matrix[i - 1, j] && matrix[i,j] != 0)
	        {
	            matrix[i - 1, j] = 0;
	            killLastTouchedTower = true;
				elementPack.Up = true;
			}
	        if (matrix[i, j] == matrix[i, j - 1] && matrix[i,j] != 0)
	        {
	            matrix[i, j - 1] = 0;
	            killLastTouchedTower = true;
				elementPack.Left = true;
	        }
	        if (matrix[i, j] == matrix[i, j + 1] && matrix[i,j] != 0)
	        {
	            matrix[i, j + 1] = 0;
	            killLastTouchedTower = true;
				elementPack.Right = true;
			}

	        if (killLastTouchedTower == true)
	            matrix[i, j] = 0;

			elementPack.Value -= CalculateSum(matrix);
			elementPack.ValueCoefficient = elementPack.Value/elementPack.Cost;

			return elementPack;
		}
	    
		private static bool isCollisionAffected (int[,] matrix, element targetElement)
		{
			UInt16 upElementCordinatesRow = (UInt16)(targetElement.Row - 1);
			UInt16 upElementCordinatesCol = targetElement.Col;
			UInt16 downElementCordinatesRow = (UInt16)(targetElement.Row + 1);
			UInt16 downElementCordinatesCol = targetElement.Col;
			UInt16 leftElementCordinatesRow = targetElement.Row;
			UInt16 leftElementCordinatesCol = (UInt16)(targetElement.Col - 1);
			UInt16 rightElementCordinatesRow = targetElement.Row;
			UInt16 rightElementCordinatesCol = (UInt16)(targetElement.Col + 1);

			if (matrix [targetElement.Row, targetElement.Col] == 0)
				return true;

			if (targetElement.Up)
				if (matrix [upElementCordinatesRow, upElementCordinatesCol] == 0)
					return true;

			if (targetElement.Down)
				if (matrix [downElementCordinatesRow, downElementCordinatesCol] == 0)
					return true;

			if (targetElement.Left)
				if (matrix [leftElementCordinatesRow, leftElementCordinatesCol] == 0)
					return true;

			if (targetElement.Right)
				if (matrix [rightElementCordinatesRow, rightElementCordinatesCol] == 0)
					return true;

			return false;
		}

		public static element[] ResultInMatrixForTurnsAtLocation (int[,] matrix, UInt16 turnsCount, UInt16 row, UInt16 col)
		{
			element rasePack = new element();
			element dropPack = new element();

			int midValue = matrix[1, 1];

			int leftSum = matrix[1, 0] - midValue;
			int rightSum = matrix[1, 2] - midValue;
			int upSum = matrix[0, 1] - midValue;
			int downSum = matrix[2, 1] - midValue;

			int[] localValues = new int []
			{
				rightSum,
				upSum,
				leftSum,
				downSum
			};

			int upValue = 0, downValue = 0;
			foreach (var el in localValues)
			{
				if (Math.Abs(el) > turns)
					continue;

				if( el < 0 )
				{
					if (Math.Abs(el) < downValue || downValue == 0)
						downValue = Math.Abs(el);
				}
				else
				{
					if (el < upValue || upValue == 0)
						upValue = el;
				}
			}

			if(upValue != 0)
			{
				int [,] rasedMatrix = new int [3,3];
				Array.Copy(matrix, rasedMatrix, matrix.Length);

				rasePack = ElementAfterTurn(rasedMatrix, row, col, upValue);
			}


			if(downValue != 0)
			{
				int [,] dropMatrix = new int [3,3];
				Array.Copy(matrix, dropMatrix, matrix.Length);

				dropPack = ElementAfterTurn(dropMatrix, row, col, -downValue);
			}

			element[] result = new element [2];
			result[0] = rasePack;
			result[1] = dropPack;

			return result;
		}


		private static void ExecuteTurn (int[,] matrix, element targetElement)
		{
			UInt16 upElementCordinatesRow = (UInt16)(targetElement.Row - 1);
			UInt16 upElementCordinatesCol = (UInt16)targetElement.Col;
			UInt16 downElementCordinatesRow = (UInt16)(targetElement.Row + 1);
			UInt16 downElementCordinatesCol = (UInt16)targetElement.Col;
			UInt16 leftElementCordinatesRow = (UInt16)targetElement.Row;
			UInt16 leftElementCordinatesCol = (UInt16)(targetElement.Col - 1);
			UInt16 rightElementCordinatesRow = (UInt16)targetElement.Row;
			UInt16 rightElementCordinatesCol = (UInt16)(targetElement.Col + 1);

			if (targetElement.Up == true)
				matrix [upElementCordinatesRow, upElementCordinatesCol] = 0;

			if (targetElement.Down == true)
				matrix [downElementCordinatesRow, downElementCordinatesCol] = 0;

			if (targetElement.Left == true)
				matrix [leftElementCordinatesRow, leftElementCordinatesCol] = 0;
		
			if (targetElement.Right == true)
				matrix [rightElementCordinatesRow, rightElementCordinatesCol] = 0;

			matrix[targetElement.Row, targetElement.Col] = 0;

		}

	    public static int CalculateSum(int[,] matix)
	    {
	        int sum = 0;
			int n = (int)Math.Sqrt(matix.Length);

			for (int row = 0; row < n; row++)
	        {
	            for (int col = 0; col < n; col++)
	            {
	                sum = sum + matix[row, col];
	            }
	        }
	        return sum;
	    }

		public static int[,] TurnIteration(int[,] matrix, int lastTurnRow, int lastTurnCol)
	    {
	        int len = dim;
	        int i = lastTurnRow;
	        int j = lastTurnCol;

	        bool killLastTouchedTower = false;

	        if (i > 0)
	            if (matrix[i, j] == matrix[i - 1, j])
	            {
	                matrix[i - 1, j] = 0;
	                killLastTouchedTower = true;
	            }
	        if (i < len - 1)
	            if (matrix[i, j] == matrix[i + 1, j])
	            {
	                matrix[i + 1, j] = 0;
	                killLastTouchedTower = true;
	            }
	        if (j > 0)
	            if (matrix[i, j] == matrix[i, j - 1])
	            {
	                matrix[i, j - 1] = 0;
	                killLastTouchedTower = true;
	            }

	        if (j < len - 1)
	            if (matrix[i, j] == matrix[i, j + 1])
	            {
	                matrix[i, j + 1] = 0;
	                killLastTouchedTower = true;
	            }

	        if (killLastTouchedTower == true)
	            matrix[i, j] = 0;

	        return matrix;
	    }
	}
}

