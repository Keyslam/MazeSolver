using OpenToolkit.Graphics.OpenGL4;
using OpenToolkit.Mathematics;
using System;
using System.Collections.Generic;

namespace MazeBacktracking.Source
{
	public class MazeRenderer : IDisposable
	{
		private const int VERTEX_ATTRIBUTE_COUNT = 5;                        // vec2 position, vec3 color
		private const int TILE_ATTRIBUTE_COUNT = 6 * VERTEX_ATTRIBUTE_COUNT; // 6 vertices per tile              

		private static readonly Color4 solidColor = new Color4(46, 52, 64, 255);
		private static readonly Color4 emptyColor = new Color4(236, 239, 244, 255);
		private static readonly Color4 startColor = new Color4(163, 190, 140, 255);
		private static readonly Color4 endColor = new Color4(191, 97, 106, 255);
		private static readonly Color4 solutionColor = new Color4(235, 203, 139, 255);
		private static readonly Color4 visitedColor = new Color4(208, 135, 112, 255);

		private float[] vertices = null;
		private int vertexCount = 0;

		private int vboHandle = -1;
		private int vaoHandle = -1;

		public MazeRenderer(int tileCount)
		{
			vertices = new float[tileCount * TILE_ATTRIBUTE_COUNT];

			vboHandle = GL.GenBuffer();
			vaoHandle = GL.GenVertexArray();

			GL.BindVertexArray(vaoHandle);
			GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandle);

			GL.EnableVertexAttribArray(0); // TODO: Get position from shader
			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

			GL.EnableVertexAttribArray(1); // TODO: Get position from shader
			GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 2 * sizeof(float));
		}

		public void RenderMaze(Maze maze, Vector2i screenSize, Dictionary<Vector2i, bool> visited, List<Vector2i> solution)
		{
			Vector2 tileSize = new Vector2(screenSize.X / maze.size.X, screenSize.Y / maze.size.X);

			CreateVertices(maze, tileSize, visited, solution);
			UpdateVAO();
			DrawVAO();
		}

		private void AddVertex(Vector2 position, Color4 color)
		{
			int offset = vertexCount * VERTEX_ATTRIBUTE_COUNT;

			vertices[offset + 0] = position.X;
			vertices[offset + 1] = position.Y;
			vertices[offset + 2] = color.R;
			vertices[offset + 3] = color.G;
			vertices[offset + 4] = color.B;

			vertexCount++;
		}

		private void AddTile(Vector2 position, Vector2 size, Color4 color)
		{
			// Top-left triangle
			AddVertex(position + new Vector2(0, 0), color);
			AddVertex(position + new Vector2(size.X, 0), color);
			AddVertex(position + new Vector2(0, size.Y), color);

			// Bottom-right triangle
			AddVertex(position + new Vector2(size.X, 0), color);
			AddVertex(position + new Vector2(size.X, size.X), color);
			AddVertex(position + new Vector2(0, size.Y), color);
		}

		private void CreateVertices(Maze maze, Vector2 tileSize, Dictionary<Vector2i, bool> visited, List<Vector2i> solution)
		{
			vertexCount = 0;

			for (int i = 0; i < maze.size.X * maze.size.Y; i++)
			{
				Vector2i tilePosition = new Vector2i(
					i % maze.size.X,
					(int)Math.Floor((double)(i / maze.size.Y))
				);

				Vector2 drawPosition = new Vector2(
					tilePosition.X * tileSize.X,
					tilePosition.Y * tileSize.Y
				);

				Color4 color;

				if (solution.Contains(tilePosition))
					color = solutionColor;
				else if (maze.StartPosition == tilePosition)
					color = startColor;
				else if (maze.EndPosition == tilePosition)
					color = endColor;
				else if (visited.ContainsKey(tilePosition) && visited[tilePosition])
					color = visitedColor;
				else if (maze.GetTileSolid(tilePosition))
					color = solidColor;
				else
					color = emptyColor;

				AddTile(drawPosition, tileSize, color);
			}
		}

		private void UpdateVAO()
		{
			GL.BindVertexArray(vaoHandle);
			GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StreamDraw);

			GL.BindVertexArray(0);
		}

		private void DrawVAO()
		{
			GL.BindVertexArray(vaoHandle);
			GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);

			GL.BindVertexArray(0);
		}

		public void Dispose()
		{
			GL.DeleteBuffer(vboHandle);
			GL.DeleteBuffer(vaoHandle);
		}
	}
}
