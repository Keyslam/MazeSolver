using OpenToolkit.Mathematics;
using System;

namespace MazeBacktracking.Source
{
	public static class MazeLoader
	{
		private static readonly Maze.Friend MazeFriend = new Maze.Friend();

		public enum MazeType
		{
			Test1,
			Test2,
			MapTest3,
		}

		private static readonly int[,] mapDefault = new int[,]
		{
			{ 0, 0, 0 },
			{ 0, 0, 0 },
			{ 0, 0, 0 },
		};

		private static readonly int[,] mapTest1 = new int[,]
		{
			{ 0, 2, 0, 0, 0 },
			{ 0, 0, 0, 0, 0 },
			{ 1, 0, 1, 0, 1 },
			{ 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 0, 3 },
		};

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

		public static Maze LoadMaze(MazeType mazeType)
		{
			int[,] mazeData;

			switch (mazeType)
			{
				case MazeType.Test1:
					mazeData = mapTest1;
					break;
				case MazeType.Test2:
					mazeData = mapTest2;
					break;
				case MazeType.MapTest3:
					mazeData = mapTest3;
					break;
				default:
					mazeData = mapDefault;
					break;
			};

			Vector2i mazeSize = new Vector2i(mazeData.GetLength(0), mazeData.GetLength(1));
			Maze maze = new Maze(mazeSize);

			for (int i = 0; i < mazeData.Length; i++)
			{
				Vector2i tilePosition = new Vector2i(
					i % maze.size.X,
					(int)Math.Floor((double)(i / maze.size.Y))
				);

				int tileData = mazeData[tilePosition.Y, tilePosition.X];

				switch (tileData)
				{
					case 0:
						maze.SetTileSolid(tilePosition, false);
						break;
					case 1:
						maze.SetTileSolid(tilePosition, true);
						break;
					case 2:
						MazeFriend.SetStartPosition(maze, tilePosition);
						break;
					case 3:
						MazeFriend.SetEndPosition(maze, tilePosition);
						break;
				}
			}
			
			return maze;
		}
	}
}
