#version 450

layout(location = 0) uniform vec2 scaleFactor;

layout(location = 0) in uint vValue;
layout(location = 1) in uint vBarColor;

out VERT_GEO {
    vec4 barColor;
} vertOut;

// Converts a uint to a vec4 colour.
vec4 computeColor(uint value) {
    uvec4 comps = uvec4(value);
    comps = (comps >> uvec4(16, 8, 0, 24)) & uvec4(0xFF);
    vec4 res = vec4(comps);
    res /= 255.0f;
    return res;
}
vec2 computePosition(uint index, uint value) {
    vec2 res = vec2(uvec2(index, value));
    return res * 2 / scaleFactor - 1;
}

void main() {
    vertOut.barColor = computeColor(vBarColor);
    gl_Position = vec4(computePosition(gl_VertexID, vValue), 0.0f, 1.0f);
}
