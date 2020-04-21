using OpenToolkit.Mathematics;
using System;
using System.Collections;

namespace MazeBacktracking.Source
{
	/// <summary>
	/// Maze contains all data for a maze
	/// </summary>
	public class Maze : IDisposable
	{
		/// <summary>
		/// Size of the maze
		/// </summary>
		public Vector2i size = default;

		/// <summary>
		/// Start position of maze
		/// </summary>
		public Vector2i startPosition = default;

		/// <summary>
		/// End position of maze
		/// </summary>
		public Vector2i endPosition = default;

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
		/// Solution for this maze
		/// </summary>
		private MazeSolver.Solution solution = null;

		/// <summary>
		/// Coroutine for solving
		/// </summary>
		private IEnumerator solutionRoutine = null;

		/// <summary>
		/// Create a maze
		/// </summary>
		/// <param name="mazeType">MazeType to load</param>
		public Maze(MazeLoader.MazeType mazeType)
		{
			MazeLoader.LoadMaze(this, mazeType);

			mazeRenderer = new MazeRenderer(size.X * size.Y);
		}

		/// <summary>
		/// Sets the size of the maze
		/// Clears all previously set tiles
		/// </summary>
		/// <param name="newSize">Size for maze to become</param>
		public void SetSize(Vector2i newSize)
		{
			size = newSize;
			map = new bool[size.X, size.Y];
		}

		/// <summary>
		/// Render the maze
		/// </summary>
		/// <param name="screenSize">Size of screen</param>
		/// <param name="visited">All visited tiles</param>
		/// <param name="solution">Solution</param>
		public void Render(Vector2i screenSize)
		{
			mazeRenderer.RenderMaze(this, screenSize, solution);
		}

		/// <summary>
		/// Solve the maze
		/// </summary>
		public void Solve()
		{
			solution = new MazeSolver.Solution(this);
			MazeSolver.Solve(solution);
		}

		/// <summary>
		/// Solve one step of the maze
		/// </summary>
		/// <returns>Done</returns>
		public bool SolveStep()
		{
			if (solutionRoutine == null)
			{
				if (solution == null)
					solution = new MazeSolver.Solution(this);

				solutionRoutine = MazeSolver.SolveStep(solution);
			}

			solutionRoutine.MoveNext();

			bool done = false;
			if (solutionRoutine.Current != null)
				done = (bool)solutionRoutine.Current;

			if (done)
				solutionRoutine = null;

			return done;
		}

		/// <summary>
		/// Clear the current solution
		/// </summary>
		public void ClearSolution()
		{
			solution = null;
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
