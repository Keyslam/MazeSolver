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
			// Set up game window
			GameWindowSettings gameWindowSettings = new GameWindowSettings
			{
				// Lower frequency for easy stepping
				RenderFrequency = 10,
				UpdateFrequency = 10
			};

			NativeWindowSettings nativeWindowSettings = new NativeWindowSettings
			{
				Size = new Vector2i(1280, 1280),
				Title = "Maze Backtracking"
			};

			Window window = new Window(gameWindowSettings, nativeWindowSettings);

			// Run game window
			window.Run();
		}
	}

	public class Window : GameWindow
	{
		/// <summary>
		/// Shader to render with
		/// </summary>
		private Shader shader = null;

		/// <summary>
		/// Perspective matrix to render with
		/// </summary>
		private Matrix4 perspectiveMatrix = default;


		/// <summary>
		/// Current maze
		/// </summary>
		private Maze maze = null;

		public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) { }

		protected override void OnLoad()
		{
			base.OnLoad();

			// Enable GL debugging
			GL.Enable(EnableCap.DebugOutput);
			DebugProc openGLDebugDelegate = new DebugProc(OpenGLDebugCallback);
			GL.DebugMessageCallback(openGLDebugDelegate, IntPtr.Zero);

			// Create shader
			shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");

			// Create orthographic perspective matrix
			perspectiveMatrix = Matrix4.CreateOrthographicOffCenter(0.0f, Size.X, Size.Y, 0.0f, -100.0f, 100.0f);

			maze = new Maze(MazeLoader.MazeType.Test3);
		}

		protected override void OnUpdateFrame(FrameEventArgs args)
		{
			base.OnUpdateFrame(args);

			// Progress solve routine
			if (KeyboardState.IsKeyDown(Key.Space))
				maze.SolveStep();
		}

		protected override void OnRenderFrame(FrameEventArgs args)
		{
			base.OnRenderFrame(args);

			// Clear screenbuffer
			GL.Clear(ClearBufferMask.ColorBufferBit);

			// Active shader
			shader.Use();

			// Send perspective matrix to shader
			int perspectiveMatrixLocation = shader.GetUniformLocation("perspectiveMatrix");
			GL.UniformMatrix4(perspectiveMatrixLocation, true, ref perspectiveMatrix);

			// Render maze
			if (maze != null)
				maze.Render(Size);

			// Display screen contents
			SwapBuffers();
		}

		protected override void OnKeyDown(KeyboardKeyEventArgs e)
		{
			base.OnKeyDown(e);

			// Load test maze 1
			if (e.Key == Key.F1)
				maze = new Maze(MazeLoader.MazeType.Test1);

			// Load test maze 2
			if (e.Key == Key.F2)
				maze = new Maze(MazeLoader.MazeType.Test2);

			// Load test maze 3
			if (e.Key == Key.F3)
				maze = new Maze(MazeLoader.MazeType.Test3);

			// Solve maze
			if (e.Key == Key.S)
			{
				if (maze != null)
					maze.Solve();
			}

			// Clear solution
			if (e.Key == Key.C)
			{
				if (maze != null)
					maze.ClearSolution();
			}
		}

		protected override void OnUnload()
		{
			// Dispose shader
			if (shader != null)
				shader.Dispose();

			// Dispose maze
			if (maze != null)
				maze.Dispose();

			base.OnUnload();
		}

		private void OpenGLDebugCallback(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
		{
			string msg = Marshal.PtrToStringAnsi(message, length);
			Console.WriteLine(msg);
		}
	}
}
