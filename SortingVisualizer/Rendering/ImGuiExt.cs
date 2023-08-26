using System.Numerics;
using ImGuiNET;
using Silk.NET.Maths;

namespace SortingVisualizer.Rendering;

public static class ImGuiExt
{
    private struct FillViewportCleanup : IDisposable
    {
        public void Dispose()
        {
            ImGui.End();
            ImGui.PopStyleColor(1);
            ImGui.PopStyleVar(1);
        }
    }
    
    /// <summary>
    /// Simple viewport dock for ImGui.
    /// </summary>
    /// <returns></returns>
    public static IDisposable DockViewport(float width)
    {
        var viewport = ImGui.GetWindowViewport();
        ImGui.SetNextWindowPos(viewport.WorkPos);
        ImGui.SetNextWindowSize(viewport.WorkSize with {X = width});
        ImGui.SetNextWindowViewport(viewport.ID);

        ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0.0f);
        ImGui.PushStyleColor(ImGuiCol.Border, new Vector4(0.0f, 0.0f, 0.0f, 0.0f));
        ImGui.Begin("", ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoResize);
        return new FillViewportCleanup();
    }
}