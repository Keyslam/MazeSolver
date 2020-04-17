using OpenToolkit.Mathematics;
using System;
using System.Collections.Generic;

namespace MazeBacktracking.Source
{
	/// <summary>
	/// Maze contains all data for a maze
	/// </summary>
	public class Maze : IDisposable
	{
		/// <summary>
		/// Friend class to give access to otherwise private fields
		/// </summary>
		public class Friend
		{
			/// <summary>
			/// Sets the start position
			/// </summary>
			/// <param name="maze">Maze to set startposition of</param>
			/// <param name="StartPosition">Startposition to set</param>
			public void SetStartPosition(Maze maze, Vector2i StartPosition)
			{
				maze.StartPosition = StartPosition;
			}

			/// <summary>
			/// Sets the end position
			/// </summary>
			/// <param name="maze">Maze to set endposition of</param>
			/// <param name="EndPosition">Endposition to set</param>
			public void SetEndPosition(Maze maze, Vector2i EndPosition)
			{
				maze.EndPosition = EndPosition;
			}
		}

		/// <summary>
		/// Size of the maze
		/// </summary>
		public readonly Vector2i size = default;

		/// <summary>
		/// Start position of maze
		/// </summary>
		public Vector2i StartPosition
		{
			get;
			private set;
		}

		/// <summary>
		/// End position of maze
		/// </summary>
		public Vector2i EndPosition
		{
			get;
			private set;
		}

		/// <summary>
		/// Map of all tiles
		/// false is empty
		/// true is solid
		/// </summary>
		private bool[,] map = null;

		/// <summary>
		/// Instance of mazeRenderer for this Maze
		/// </summary>
		private MazeRenderer mazeRenderer = null;

		/// <summary>
		/// Create a maze
		/// </summary>
		/// <param name="size">Size of the maze</param>
		public Maze(Vector2i size)
		{
			if (size.X < 3 || size.Y < 3)
				throw new ArgumentException("Both components of size need to be 3 or greater");

			this.size = size;

			map = new bool[this.size.X, this.size.Y];

			mazeRenderer = new MazeRenderer(this.size.X * this.size.Y);
		}

		/// <summary>
		/// Render the maze
		/// </summary>
		/// <param name="screenSize">Size of screen</param>
		/// <param name="visited">All visited tiles</param>
		/// <param name="solution">Solution</param>
		public void Render(Vector2i screenSize, Dictionary<Vector2i, bool> visited, List<Vector2i> solution)
		{
			mazeRenderer.RenderMaze(this, screenSize, visited, solution);
		}

		/// <summary>
		/// Gets if tile is solid
		/// </summary>
		/// <param name="position">Position of tile to check</param>
		/// <returns>True if tile is solid. False otherwise</returns>
		public bool GetTileSolid(Vector2i position)
		{
			return map[position.X, position.Y];
		}

		/// <summary>
		/// Sets the tile's solid
		/// </summary>
		/// <param name="position">Position of tile to set</param>
		/// <param name="solid">If solid</param>
		public void SetTileSolid(Vector2i position, bool solid)
		{
			map[position.X, position.Y] = solid;
		}

		/// <summary>
		/// Toggles the tile's solid
		/// </summary>
		/// <param name="position">Position of tile to toggle</param>
		public void ToggleTileSolid(Vector2i position)
		{
			SetTileSolid(position, !GetTileSolid(position));
		}

		/// <summary>
		/// Disposes the object
		/// </summary>
		public void Dispose()
		{
			// Dispose the mazeRenderer if it exists
			if (mazeRenderer != null)
				mazeRenderer.Dispose();
		}
	}
}
