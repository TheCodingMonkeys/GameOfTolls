using System;

using System.Diagnostics;
using System.IO;
using System.Text;

namespace GameOfTrolls
{
	public class UserInterface
	{
		static UInt16 turns, dim;

		public UserInterface ()
		{
		
		}

				public UInt16 getDimention()
		{
			return dim;
		}

		public UInt16 getTurns ()
		{
			return turns;
		}

		public static void DrowHtmlStart(int[,] startMatrix)
	    {
	        StreamWriter writer = new StreamWriter("index.html");
	        using (writer)
	        {
	            writer.WriteLine("<!DOCTYPE html><html><head><meta charset=\"utf-8\"><LINK REL=\"SHORTCUT ICON\" HREF=\"lib/icon.png\"><title>Game of Trolls turn </title>"
	                + "<link rel=\"stylesheet\" href=\"lib/jquery.mobile-1.2.0.min.css\" />"
	                + "<link rel=\"stylesheet\" href=\"lib/style.css\" /><script src=\"lib/jquery-1.8.2.min.js\"></script>"
	                + "<script src=\"lib/jquery.mobile-1.2.0.min.js\"></script></head><body>"
	                + "<div data-role=\"page\" id=\"a\" data-theme=\"b\"><div data-role=\"header\"  data-theme=\"b\"><h1>"
	                + "<a data-icon=\"arrow-l\" href=\"index.html\" data-role=\"button\" data-inline=\"true\" data-theme=\"c\" class=\"ui-disabled\">Back</a>"
	                + "Start Matrix <a data-iconpos=\"right\" data-icon=\"arrow-r\" href=\"#b\" data-role=\"button\" data-inline=\"true\" data-theme=\"c\">Next</a>"
	                + "</h1></div><div data-role=\"content\" ><table class=\"bordered\">", turns, turns);
	            writer.WriteLine("<table class=\"bordered\">");
	            writer.WriteLine("<tr>");
	            writer.WriteLine("<th></th>");
	            int matrixDim = (int)Math.Sqrt(startMatrix.Length);
	            for (int row = 0; row < matrixDim; row++)
	            {
	                writer.WriteLine("<th>{0}</th>", row);
	            }
	            writer.WriteLine("</tr>");
	            for (int row = 0; row < matrixDim; row++)
	            {
	                writer.WriteLine("<tr>");
	                writer.WriteLine("<th>{0}</th>", row);
	                for (int col = 0; col < matrixDim; col++)
	                {
	                    writer.Write("<td>{0}</td>", startMatrix[row, col]);
	                }
	                writer.WriteLine("</tr>");
	            }
	            writer.WriteLine("</table></div></div>");
	        }
	    }

	    public static void DrowTurns(int[,] matrix, StringBuilder builder, int n)
	    {
	        string[] lines = builder.ToString().Split('\n');

	        for (int i = 0; i < lines.Length - 1; i++)
	        {
	            string[] tokens = lines[i].Split(' ');
	            bool put;
	            if (tokens[0].CompareTo("put") == 0)
	            {
	                put = true;
	            }
	            else
	            {
	                put = false;
	            }

	            int row, col;
	            row = int.Parse(tokens[1]);
	            col = int.Parse(tokens[2]);

	            if (put == true)
	            {
	                matrix[row, col]++;
	            }
	            else
	            {
	                matrix[row, col]--;
	            }
	            Algorithm.TurnIteration(matrix, row, col);
	            int letter = 98 + i;

	            StreamWriter writer = File.AppendText("index.html");
	            using (writer)
	            {
	                writer.WriteLine("<div data-theme=\"b\" data-role=\"page\" id=\"{0}\">", (char)letter);
	                writer.WriteLine("<div data-role=\"header\"  data-theme=\"b\">" +
	                                    "<h1>" +
	                                        "<a data-icon=\"arrow-l\" href=\"#{0}\" data-role=\"button\" data-inline=\"true\" data-theme=\"c\">Back</a>", (char)(letter - 1));
	                writer.Write(lines[i]);
	                writer.Write("<a data-iconpos=\"right\" data-icon=\"arrow-r\" href=\"#{1}\" data-role=\"button\" data-inline=\"true\" data-theme=\"c\" ", (char)letter, (char)(letter + 1));
	                if (i == n - 1)
	                    writer.Write("class=\"ui-disabled\" ");

	                writer.WriteLine(">Next</a>" +
	                                     "</h1>" +
	                                      "</div>" +
	                                  "<div data-role=\"content\">");
	                writer.WriteLine("<table class=\"bordered\">");
	                writer.WriteLine("<tr>");
	                writer.WriteLine("<th></th>");
	                int matrixDim = (int)Math.Sqrt(matrix.Length);
	                for (int row1 = 0; row1 < matrixDim; row1++)
	                {
	                    writer.WriteLine("<th>{0}</th>", row1);
	                }
	                writer.WriteLine("</tr>");
	                for (int row1 = 0; row1 < matrixDim; row1++)
	                {
	                    writer.WriteLine("<tr>");
	                    writer.WriteLine("<th>{0}</th>", row1);
	                    for (int col1 = 0; col1 < matrixDim; col1++)
	                    {
	                        writer.Write("<td ");
	                        if (row == row1 && col == col1)
	                        {
	                            if (put == true)
	                            {
	                                writer.Write("class=\"plus\"");
	                            }
	                            else
	                            {
	                                writer.Write("class=\"minus\"");
	                            }
	                        }
	                        writer.Write(" >{0}</td>", matrix[row1, col1]);
	                    }
	                    writer.WriteLine("</tr>");
	                }
	                writer.WriteLine("</table></div></div>");
	            }
	        }
	    }

	    public static void DrowHtmlEnd()
	    {
	        StreamWriter writer = File.AppendText("index.html");
	        using (writer)
	        {
	            writer.WriteLine("</body></html>");
	        }
	    }

		public static int[,] ColectInformation()
	    {
	        turns = UInt16.Parse(Console.ReadLine());
	        dim = UInt16.Parse(Console.ReadLine());
	        int[,] gameField = new int[dim, dim];
	        for (int row = 0; row < dim; row++)
	        {
	            string rowData = Console.ReadLine();
	            string[] rowCells = rowData.Split(' ');
	            for (int col = 0; col < dim; col++)
	            {
	                gameField[row, col] = int.Parse(rowCells[col]);
	            }
	        }

	        return gameField;
	    }
	}
}

