using OpenToolkit.Windowing.Desktop;
using OpenToolkit.Mathematics;
using System;
using OpenToolkit.Windowing.Common;
using OpenToolkit.Graphics.OpenGL4;
using System.Runtime.InteropServices;
using MazeBacktracking.Core;
using MazeBacktracking.Source;
using OpenToolkit.Windowing.Common.Input;
using System.Collections;
using System.Collections.Generic;

namespace MazeBacktracking
{
	public static class Program
	{
		static void Main(string[] args)
		{
			GameWindowSettings gameWindowSettings = new GameWindowSettings
			{
				RenderFrequency = 60,
				UpdateFrequency = 60
			};

			NativeWindowSettings nativeWindowSettings = new NativeWindowSettings
			{
				Size = new Vector2i(1280, 1280),
				Title = "Maze Backtracking"
			};

			Window window = new Window(gameWindowSettings, nativeWindowSettings);

			window.Run();

		}
	}

	public class Window : GameWindow
	{
		private Shader shader = null;
		private Matrix4 perspectiveMatrix = Matrix4.Identity;

		private Maze maze = null;

		private IEnumerator solveRoutine = null;
		private Dictionary<Vector2i, bool> visited = null;
		private List<Vector2i> solution = null;

		public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) { }

		

		protected override void OnLoad()
		{
			base.OnLoad();

			GL.Enable(EnableCap.DebugOutput);
			DebugProc openGLDebugDelegate = new DebugProc(OpenGLDebugCallback);
			GL.DebugMessageCallback(openGLDebugDelegate, IntPtr.Zero);

			shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");

			perspectiveMatrix = Matrix4.CreateOrthographicOffCenter(0.0f, Size.X, Size.Y, 0.0f, -100.0f, 100.0f);

			GL.ClearColor(new Color4(30, 30, 30, 255));

			maze = MazeLoader.LoadMaze(MazeLoader.MazeType.MapTest3);

			visited = new Dictionary<Vector2i, bool>();
			solution = new List<Vector2i>();

			/*solveRoutine = */MazeSolver.Solve(maze, out visited, out solution);
		}

		protected override void OnRenderFrame(FrameEventArgs args)
		{
			base.OnRenderFrame(args);

			GL.Clear(ClearBufferMask.ColorBufferBit);

			shader.Use();

			int perspectiveMatrixLocation = shader.GetUniformLocation("perspectiveMatrix");
			GL.UniformMatrix4(perspectiveMatrixLocation, true, ref perspectiveMatrix);

			maze.Render(Size, visited, solution);

			SwapBuffers();
		}

		private void OpenGLDebugCallback(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
		{
			string msg = Marshal.PtrToStringAnsi(message, length);
			Console.WriteLine(msg);
		}

		protected override void OnKeyDown(KeyboardKeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (e.Key == Key.Space)
			{
				solveRoutine.MoveNext();
			}
		}

		protected override void OnUnload()
		{
			if (shader != null)
				shader.Dispose();

			if (maze != null)
				maze.Dispose();

			base.OnUnload();
		}
	}
}
