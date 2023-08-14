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
            ImGui.PopStyleVar();
        }
    }

    /// <summary>
    /// Sets up ImGui to draw to a given viewport.
    /// </summary>
    /// <returns>An IDisposable, for use inside a <c>using</c> block. </returns>
    /// <remarks>
    /// This code is sourced from a GitHub issue: <a href="https://github.com/ocornut/imgui/issues/3541#issuecomment-712248014">ocornut/imgui#3541</a>
    /// </remarks>
    public static IDisposable FillViewport()
    {
        var viewport = ImGui.GetWindowViewport();
        ImGui.SetNextWindowPos(viewport.WorkPos);
        ImGui.SetNextWindowSize(viewport.WorkSize with
        {
            X = 200
        });
        ImGui.SetNextWindowViewport(viewport.ID);

        ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0.0f);
        ImGui.Begin("", ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoResize);
        return new FillViewportCleanup();
    }
}