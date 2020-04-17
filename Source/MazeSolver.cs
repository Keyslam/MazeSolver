using OpenToolkit.Mathematics;
using System.Collections;
using System.Collections.Generic;

namespace MazeBacktracking.Source
{
	/// <summary>
	/// Solves mazes
	/// </summary>
	public static class MazeSolver
	{
		/// <summary>
		/// Array of all directions the algorithm can branch into
		/// </summary>
		private static readonly Vector2i[] directions = new Vector2i[]
		{
			new Vector2i( 0, -1), // Up
			new Vector2i( 0,  1), // Down
			new Vector2i(-1,  0), // Left
			new Vector2i( 1,  0), // Right
		};

		/// <summary>
		/// Solves a maze
		/// </summary>
		/// <param name="maze">Maze to solve</param>
		/// <param name="visited">All visited tiles</param>
		/// <param name="solution">Path from end to start</param>
		/// <returns>True if found solution. False otherwise</returns>
		public static bool Solve(Maze maze, out Dictionary<Vector2i, bool> visited, out List<Vector2i> solution)
		{
			Vector2i startPosition = maze.StartPosition;

			visited = new Dictionary<Vector2i, bool>();
			solution = new List<Vector2i>();

			IEnumerator coroutine = SolveStep(maze, startPosition, visited, solution);

			while (coroutine.MoveNext())
			{
				object result = coroutine.Current;

				if (result != null)
					return (bool)result;
			}

			return false;
		}

		/// <summary>
		/// Solves a maze as a coroutine
		/// </summary>
		/// <param name="maze">Maze to solve</param>
		/// <param name="visited">All visited tiles</param>
		/// <param name="solution">Path from end to start</param>
		/// <returns>IEnumerator</returns>
		public static IEnumerator SolveRoutine(Maze maze, out Dictionary<Vector2i, bool> visited, out List<Vector2i> solution)
		{
			Vector2i startPosition = maze.StartPosition;

			visited = new Dictionary<Vector2i, bool>();
			solution = new List<Vector2i>();

			return SolveStep(maze, startPosition, visited, solution);
		}

		/// <summary>
		/// Explores one step in the solving algortihm
		/// This is done by exploring all neighbours and checking it's validity,
		/// then calling itself recursively.
		/// If all neighbours are invalid the branch will be invalid as well.
		/// Eventually the target tile will be found (or all tiles will have been explored),
		/// after which all branches will collapse.
		/// </summary>
		/// <param name="maze">Maze to solve</param>
		/// <param name="currentPosition">Position to explore from</param>
		/// <param name="visited">All visited tiles so far</param>
		/// <param name="solution">Solution so far</param>
		/// <returns>IEnumerator</returns>
		public static IEnumerator SolveStep(Maze maze, Vector2i currentPosition, Dictionary<Vector2i, bool> visited, List<Vector2i> solution)
		{
			visited[currentPosition] = true;
			yield return null;

			// Explore in every direction
			foreach (Vector2i direction in directions)
			{
				Vector2i newPosition = currentPosition + direction;

				// Target reached
				if (newPosition == maze.EndPosition)
					yield return true;

				// Out of bounds
				if (newPosition.X < 0 || newPosition.X >= maze.size.X || newPosition.Y < 0 || newPosition.Y >= maze.size.Y)
					continue;

				// Already visited
				if (visited.ContainsKey(newPosition) && visited[newPosition])
					continue;

				// Tile is non-traversable
				if (maze.GetTileSolid(newPosition))
					continue;

				// Explore branch
				IEnumerator branch = SolveStep(maze, newPosition, visited, solution);
				while (branch.MoveNext())
				{
					object result = branch.Current;

					// Branch requests to yield
					if (result == null)
					{
						yield return null;
						continue;
					}

					if ((bool)result)
					{
						// Found solution. Traverse branch backwards
						solution.Add(newPosition);
						yield return true;
					}
					else
					{
						// Branch is a dead-end
						yield return false;
					}
				}
			}
		}
	}
}
