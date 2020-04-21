using OpenToolkit.Mathematics;
using System;

namespace MazeBacktracking.Source
{
	/// <summary>
	/// Loads mazes
	/// </summary>
	public static class MazeLoader
	{
		/// <summary>
		/// All predefined mazes
		/// </summary>
		public enum MazeType
		{
			Test1,
			Test2,
			Test3,
		}

		/// <summary>
		/// Default map
		/// </summary>
		private static readonly int[,] mapDefault = new int[,]
		{
			{ 0, 0, 0 },
			{ 0, 0, 0 },
			{ 0, 0, 0 },
		};

		/// <summary>
		/// Test map 1
		/// </summary>
		private static readonly int[,] mapTest1 = new int[,]
		{
			{ 0, 2, 0, 0, 0 },
			{ 0, 0, 0, 0, 0 },
			{ 1, 0, 1, 0, 1 },
			{ 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 0, 3 },
		};

		/// <summary>
		/// Test map 2
		/// </summary>
		private static readonly int[,] mapTest2 = new int[,]
		{
			{ 0, 0, 2, 0, 1, 0, 1, 0, 0, 0, 3},
			{ 0, 1, 0, 1, 1, 0, 1, 0, 1, 1, 1},
			{ 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0},
			{ 0, 1, 0, 1, 1, 0, 1, 0, 1, 0, 1},
			{ 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
			{ 1, 1, 0, 1, 1, 0, 1, 1, 1, 0, 1},
			{ 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
			{ 0, 1, 1, 1, 0, 1, 1, 0, 1, 0, 1},
			{ 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0},
			{ 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0},
			{ 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
		};

		/// <summary>
		/// Test map 3
		/// </summary>
		private static readonly int[,] mapTest3 = new int[,]
		{
			{ 2, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0},
			{ 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0},
			{ 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0},
			{ 0, 0, 0, 1, 0, 1, 1, 1, 0, 1, 1},
			{ 0, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0},
			{ 1, 1, 0, 1, 0, 1, 1, 1, 0, 1, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 3},
		};

		/// <summary>
		/// Loads a predefined Maze
		/// </summary>
		/// <param name="mazeType">Type of maze to load</param>
		/// <returns>Loaded Maze</returns>
		public static void LoadMaze(Maze maze, MazeType mazeType)
		{
			int[,] mazeData; // mazeData to read from

			// Select mazeData
			switch (mazeType)
			{
				case MazeType.Test1:
					mazeData = mapTest1;
					break;
				case MazeType.Test2:
					mazeData = mapTest2;
					break;
				case MazeType.Test3:
					mazeData = mapTest3;
					break;
				default:
					mazeData = mapDefault;
					break;
			};

			// Create maze
			Vector2i mazeSize = new Vector2i(mazeData.GetLength(0), mazeData.GetLength(1));
			maze.SetSize(mazeSize);

			// Build maze from mazeData
			for (int i = 0; i < mazeData.Length; i++)
			{
				Vector2i tilePosition = new Vector2i(
					i % maze.size.X,
					(int)Math.Floor((double)(i / maze.size.Y))
				);

				int tileData = mazeData[tilePosition.Y, tilePosition.X]; // Inverse since rows and columns are inversed when typing it as code

				switch (tileData)
				{
					case 0:
						maze.SetTileSolid(tilePosition, false);
						break;
					case 1:
						maze.SetTileSolid(tilePosition, true);
						break;
					case 2:
						maze.startPosition = tilePosition;
						break;
					case 3:
						maze.endPosition = tilePosition;
						break;
				}
			}
		}
	}
}
