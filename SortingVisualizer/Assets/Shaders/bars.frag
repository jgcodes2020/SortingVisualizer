#version 450

in vec4 barColor;

layout(location = 0) out vec4 color;

void main() {
    color = barColor;
}
