using OpenToolkit.Graphics.OpenGL4;
using System;
using System.IO;
using System.Text;

namespace MazeBacktracking.Core
{
	public class Shader : IDisposable
	{
		public int Handle
		{
			get;
			private set;
		} = -1;

		private bool disposed = false;

		public Shader(string vertexPath, string fragmentPath)
		{
			string vertexShaderSource;
			using (StreamReader reader = new StreamReader(vertexPath, Encoding.UTF8))
				vertexShaderSource = reader.ReadToEnd();

			string fragmentShaderSource;
			using (StreamReader reader = new StreamReader(fragmentPath, Encoding.UTF8))
				fragmentShaderSource = reader.ReadToEnd();

			int vertexShader = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(vertexShader, vertexShaderSource);

			int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(fragmentShader, fragmentShaderSource);


			GL.CompileShader(vertexShader);

			string infoLogVert = GL.GetShaderInfoLog(vertexShader);
			if (infoLogVert != string.Empty)
				Console.WriteLine(infoLogVert);


			GL.CompileShader(fragmentShader);

			string infoLogFrag = GL.GetShaderInfoLog(fragmentShader);
			if (infoLogFrag != string.Empty)
				Console.WriteLine(infoLogFrag);


			Handle = GL.CreateProgram();

			GL.AttachShader(Handle, vertexShader);
			GL.AttachShader(Handle, fragmentShader);

			GL.LinkProgram(Handle);



			GL.DetachShader(Handle, vertexShader);
			GL.DetachShader(Handle, fragmentShader);

			GL.DeleteShader(vertexShader);
			GL.DeleteShader(fragmentShader);
		}

		~Shader()
		{
			GL.DeleteProgram(Handle);
		}

		public void Use()
		{
			GL.UseProgram(Handle);
		}

		public int GetAttributeLocation(string attributeName)
		{
			return GL.GetAttribLocation(Handle, attributeName);
		}

		public int GetUniformLocation(string uniformName)
		{
			return GL.GetUniformLocation(Handle, uniformName);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
				GL.DeleteProgram(Handle);

			disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
