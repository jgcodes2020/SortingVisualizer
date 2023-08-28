using System.Collections.Immutable;
using System.Reflection;
using ImGuiNET;
using Silk.NET.Maths;
using SortingVisualizer.Sorting;

namespace SortingVisualizer.Rendering;

public class UIManager
{
    public static readonly int DockWidth = 200;
    
    static UIManager()
    {
        var tSortingAlgorithm = typeof(SortingAlgorithm);
        Algorithms = tSortingAlgorithm.Assembly.GetTypes()
            .Where(x => 
                x.Namespace!.StartsWith(tSortingAlgorithm.Namespace!) &&
                x.IsSubclassOf(tSortingAlgorithm))
            .ToArray();
        AlgorithmNames = Algorithms.Select(x => x.Name).ToArray();
    }
    
    private static readonly Type[] Algorithms;
    private static readonly string[] AlgorithmNames;

    private static bool TestAndReset(ref bool value)
    {
        var res = value;
        value = false;
        return res;
    }
    
    private int _currAlg;
    private int _dataLen = 20;
    
    private bool _shouldUpdate;
    private bool _shufflePressed;
    private bool _startPressed;

    public Type CurrentAlgorithm => Algorithms[_currAlg];
    public int DataLength => _dataLen;

    public bool ParametersUpdated()
    {
        var res = _shouldUpdate;
        _shouldUpdate = false;
        return res;
    }

    public void SetDrawList(Vector2D<int> screenSize)
    {
        using (ImGuiExt.DockViewport(DockWidth))
        {
            ImGui.Text("Algorithm");
            _shouldUpdate |= ImGui.ListBox("##Algorithm", ref _currAlg, AlgorithmNames, AlgorithmNames.Length, 10);
            ImGui.Spacing();
            
            ImGui.Text("Length");
            _shouldUpdate |= ImGui.InputInt("##Length", ref _dataLen, 5, 10);
            ImGui.Spacing();
            
            _shufflePressed = ImGui.Button("Shuffle");
            ImGui.SameLine();
            _startPressed = ImGui.Button("Start");
        }
        
        _dataLen = Math.Clamp(_dataLen, 5, screenSize.X - DockWidth);
    }

    public bool StartPressed()
    {
        return TestAndReset(ref _startPressed);
    }

    public bool ShufflePressed()
    {
        return TestAndReset(ref _shufflePressed);
    }
}