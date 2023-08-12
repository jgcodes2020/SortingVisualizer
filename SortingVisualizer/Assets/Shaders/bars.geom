#version 450

layout(location = 0) uniform vec2 scaleFactor;

layout (points) in;
layout (triangle_strip, max_vertices = 4) out;

in VERT_GEO {
    vec4 barColor;
} geoIn[];

out vec4 barColor;

void emitPoint(vec4 pos) {
    gl_Position = pos;
    EmitVertex();
}

void main() {
    vec2 base = gl_in[0].gl_Position.xy;
    float width = 2 / scaleFactor.x;
    
    barColor = geoIn[0].barColor;
    emitPoint(vec4(base.xy, 0.0f, 1.0f));
    emitPoint(vec4(base.x + width, base.y, 0.0f, 1.0f));
    emitPoint(vec4(base.x, -1.0f, 0.0f, 1.0f));
    emitPoint(vec4(base.x + width, -1.0f, 0.0f, 1.0f));
    EndPrimitive();
}