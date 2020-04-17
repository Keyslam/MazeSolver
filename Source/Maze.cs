using OpenToolkit.Mathematics;
using System;
using System.Collections.Generic;

namespace MazeBacktracking.Source
{ 
	public class Maze : IDisposable
	{
		public class Friend
		{
			public void SetStartPosition(Maze maze, Vector2i StartPosition)
			{
				maze.StartPosition = StartPosition;
			}

			public void SetEndPosition(Maze maze, Vector2i EndPosition)
			{
				maze.EndPosition = EndPosition;
			}
		}

		public readonly Vector2i size = default;

		public Vector2i StartPosition
		{
			get;
			private set;
		}
		public Vector2i EndPosition
		{
			get;
			private set;
		}

		private bool[,] map = null;

		private MazeRenderer mazeRenderer = null;

		public Maze(Vector2i size)
		{
			if (size.X < 3 || size.Y < 3)
				throw new ArgumentException("Both components of size need to be 3 or greater");

			this.size = size;

			map = new bool[this.size.X, this.size.Y];


			mazeRenderer = new MazeRenderer(this.size.X * this.size.Y);
		}

		public void Render(Vector2i screenSize, Dictionary<Vector2i, bool> visited, List<Vector2i> solution)
		{
			mazeRenderer.RenderMaze(this, screenSize, visited, solution);
		}

		public bool GetTileSolid(Vector2i position)
		{
			return map[position.X, position.Y];
		}

		public void SetTileSolid(Vector2i position, bool solid)
		{
			map[position.X, position.Y] = solid;
		}

		public void ToggleTileSolid(Vector2i position)
		{
			SetTileSolid(position, !GetTileSolid(position));
		}

		public void Dispose()
		{
			if (mazeRenderer != null)
				mazeRenderer.Dispose();
		}
	}
}
