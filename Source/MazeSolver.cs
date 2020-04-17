using OpenToolkit.Mathematics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MazeBacktracking.Source
{
	public class MazeSolver
	{
		private static readonly Vector2i[] directions = new Vector2i[]
		{
			new Vector2i( 0, -1), // Up
			new Vector2i( 0,  1), // Down
			new Vector2i(-1,  0), // Left
			new Vector2i( 1,  0), // Right
		};

		public static void Solve(Maze maze, out Dictionary<Vector2i, bool> visited, out List<Vector2i> solution)
		{
			Vector2i startPosition = maze.StartPosition;

			visited = new Dictionary<Vector2i, bool>();
			solution = new List<Vector2i>();

			IEnumerator coroutine = SolveStep(maze, startPosition, visited, solution);
			
			while (coroutine.MoveNext()) { }
		}

		public static IEnumerator SolveRoutine(Maze maze, out Dictionary<Vector2i, bool> visited, out List<Vector2i> solution)
		{
			Vector2i startPosition = maze.StartPosition;

			visited = new Dictionary<Vector2i, bool>();
			solution = new List<Vector2i>();

			return SolveStep(maze, startPosition, visited, solution);
		}

		public static IEnumerator SolveStep(Maze maze, Vector2i currentPosition, Dictionary<Vector2i, bool> visited, List<Vector2i> solution)
		{
			visited[currentPosition] = true;

			foreach (Vector2i direction in directions)
			{
				Vector2i newPosition = currentPosition + direction;

				if (newPosition == maze.EndPosition)
					yield return true;
					
				if (newPosition.X < 0 || newPosition.X >= maze.size.X || newPosition.Y < 0 || newPosition.Y >= maze.size.Y)
					continue;

				if (visited.ContainsKey(newPosition) && visited[newPosition])
					continue;

				if (maze.GetTileSolid(newPosition))
					continue;

				yield return null;

				IEnumerator branch = SolveStep(maze, newPosition, visited, solution);

				while (branch.MoveNext())
				{
					object result = branch.Current;

					if (result == null)
					{
						yield return null;
						continue;
					}

					if ((bool)result)
					{
						solution.Add(newPosition);
						yield return true;
					}

					yield return false;
				}
			}
		}
	}
}
