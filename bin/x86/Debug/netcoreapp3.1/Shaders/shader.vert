#version 420 core
layout (location = 0) in vec2 aPosition;
layout (location = 1) in vec4 aColor;

out vec3 color;

uniform mat4 perspectiveMatrix;

void main()
{
	color = aColor.xyz;

	gl_Position = vec4(aPosition, 0.0f, 1.0) * perspectiveMatrix;
}