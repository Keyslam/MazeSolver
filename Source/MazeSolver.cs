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
		/// Solution for a specific maze
		/// </summary>
		public class Solution
		{
			public readonly Maze maze = null;
			public readonly HashSet<Vector2i> visited = null;
			public readonly List<Vector2i> path = null;
			public Vector2i currentPosition = default;

			public Solution(Maze maze)
			{
				this.maze = maze;

				visited = new HashSet<Vector2i>();
				path = new List<Vector2i>();

				currentPosition = maze.startPosition;
			}
		}

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
		/// <param name="solution">Solution state for the maze</param>
		/// <returns>True if found solution. False otherwise</returns>
		public static bool Solve(Solution solution)
		{
			IEnumerator coroutine = SolveStep(solution);

			while (coroutine.MoveNext())
			{
				object result = coroutine.Current;

				if (result != null)
					return (bool)result;
			}

			return false;
		}

		/// <summary>
		/// Explores one step in the solving algortihm
		/// This is done by exploring all neighbours and checking it's validity,
		/// then calling itself recursively.
		/// If all neighbours are invalid the branch will be invalid as well.
		/// Eventually the target tile will be found (or all tiles will have been explored),
		/// after which all branches will collapse.
		/// </summary>
		/// <param name="solution">Solution state for the maze</param>
		/// <returns>IEnumerator</returns>
		public static IEnumerator SolveStep(Solution solution)
		{
			// Track as being visited
			solution.visited.Add(solution.currentPosition);

			yield return null;

			// Explore in every direction
			foreach (Vector2i direction in directions)
			{
				Vector2i prefPosition = solution.currentPosition;
				Vector2i newPosition = solution.currentPosition + direction;
				
				// Target reached
				if (newPosition == solution.maze.endPosition)
					yield return true;

				// Out of bounds
				if (newPosition.X < 0 || newPosition.X >= solution.maze.size.X || newPosition.Y < 0 || newPosition.Y >= solution.maze.size.Y)
					continue;

				// Already visited
				if (solution.visited.Contains(newPosition))
					continue;

				// Tile is non-traversable
				if (solution.maze.GetTileSolid(newPosition))
					continue;

				
				// Set new current position for this branch
				solution.currentPosition = newPosition;

				// Explore branch
				IEnumerator branch = SolveStep(solution);
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
						solution.path.Add(newPosition);
						yield return true;
					}
					else
					{
						// Branch is a dead-end
						yield return false;
					}
				}

				// Restore position when branch is done being explored
				solution.currentPosition = prefPosition;
			}
		}
	}
}
