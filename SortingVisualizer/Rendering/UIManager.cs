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
    private int _dataLen = 100;
    private int _speed = 50;
    
    private bool _algorithmUpdated;
    private bool _lengthUpdated;
    private bool _speedUpdated;
    
    private bool _shufflePressed;
    private bool _startPressed;
    private bool _resetPressed;

    public Type CurrentAlgorithm => Algorithms[_currAlg];
    public int DataLength => _dataLen;
    public int Speed => _speed;

    public bool AlgorithmUpdated => TestAndReset(ref _algorithmUpdated);
    public bool LengthUpdated => TestAndReset(ref _lengthUpdated);
    public bool SpeedUpdated => TestAndReset(ref _speedUpdated);
    
    public bool StartPressed => TestAndReset(ref _startPressed);
    public bool ShufflePressed => TestAndReset(ref _shufflePressed);
    public bool ResetPressed => TestAndReset(ref _resetPressed);

    public void SetDrawList(Vector2D<int> screenSize)
    {
        using (ImGuiExt.DockViewport(DockWidth))
        {
            ImGui.Text("Algorithm");
            _algorithmUpdated |= ImGui.ListBox("##Algorithm", ref _currAlg, AlgorithmNames, AlgorithmNames.Length, 10);
            ImGui.Spacing();
            
            ImGui.Text("Length");
            _lengthUpdated |= ImGui.InputInt("##Length", ref _dataLen, 5, 10);
            ImGui.Spacing();
            
            ImGui.Text("Speed (updates/s)");
            _speedUpdated |= ImGui.InputInt("##Speed", ref _speed, 2, 10);
            ImGui.Spacing();
            
            _shufflePressed = ImGui.Button("Shuffle");
            ImGui.SameLine();
            _startPressed = ImGui.Button("Start");

            _resetPressed = ImGui.Button("Reset");
        }
        
        _dataLen = Math.Clamp(_dataLen, 5, screenSize.X - DockWidth);
        _speed = Math.Clamp(_speed, 2, 1200);
    }
}