using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;
public class InputHistoryDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInputHandler _inputHandler;
    [SerializeField] private TextMeshProUGUI _historyText;
    
    [Header("Display Settings")]
    [SerializeField] private int _maxDisplayedInputs = 15;
    [SerializeField] private bool _showFrames = true;
    [SerializeField] private bool _combineSimultaneousInputs = true;
    [SerializeField] private int _simultaneousInputFrameWindow = 2; // 2 frames window for "simultaneous"
    
    private StringBuilder _stringBuilder = new StringBuilder();
    
    private Dictionary<BufferInput, string> _inputSymbols = new Dictionary<BufferInput, string>()
    {
        { BufferInput.Left, "←" },
        { BufferInput.Down, "↓" },
        { BufferInput.Right, "→" },
        { BufferInput.Up, "↑" },
        { BufferInput.Light, "L" },
        { BufferInput.Medium, "M" },
        { BufferInput.Heavy, "H" },
        { BufferInput.Dash, "D" },
        { BufferInput.None, "-" }
    };
    
    private void Update()
    {
        UpdateDisplay();
    }
    
    private void UpdateDisplay()
    {
        if (_inputHandler == null || _historyText == null)
            return;
        
        var history = _inputHandler.InputBuffer.GetInputHistory();
        
        if (history.Count == 0)
        {
            _historyText.text = "";
            return;
        }
        
        _stringBuilder.Clear();
        
        if (_combineSimultaneousInputs)
        {
            DisplayCombinedInputs(history);
        }
        else
        {
            DisplayIndividualInputs(history);
        }
        
        _historyText.text = _stringBuilder.ToString();
    }
    
    private void DisplayIndividualInputs(List<InputEvent> history)
    {
        int displayCount = Mathf.Min(_maxDisplayedInputs, history.Count);
        int startIndex = Mathf.Max(0, history.Count - displayCount);
        
        for (int i = startIndex; i < history.Count; i++)
        {
            var inputEvent = history[i];
            
            if (_showFrames)
            {
                int framesHeld = inputEvent.HeldFrames;
                _stringBuilder.Append($"{_inputSymbols[inputEvent.Input]} ({framesHeld}f)");
            }
            else
            {
                _stringBuilder.Append(_inputSymbols[inputEvent.Input]);
            }
            
            _stringBuilder.AppendLine();
        }
    }
    
    private void DisplayCombinedInputs(List<InputEvent> history)
    {
        List<List<InputEvent>> groupedInputs = GroupSimultaneousInputs(history);
        
        int displayCount = Mathf.Min(_maxDisplayedInputs, groupedInputs.Count);
        int startIndex = Mathf.Max(0, groupedInputs.Count - displayCount);
        
        for (int i = startIndex; i < groupedInputs.Count; i++)
        {
            var group = groupedInputs[i];
            
            foreach (var inputEvent in group)
            {
                _stringBuilder.Append(_inputSymbols[inputEvent.Input]);
            }
            
            if (_showFrames && group.Count > 0)
            {
                int framesHeld = group[0].HeldFrames;
                _stringBuilder.Append($" ({framesHeld}f)");
            }
            
            _stringBuilder.AppendLine();
        }
    }
    
    private List<List<InputEvent>> GroupSimultaneousInputs(List<InputEvent> history)
    {
        var grouped = new List<List<InputEvent>>();
        
        if (history.Count == 0)
            return grouped;
        
        var currentGroup = new List<InputEvent> { history[0] };
        
        for (int i = 1; i < history.Count; i++)
        {
            int frameDiff = history[i].FramePressed - currentGroup[0].FramePressed;
            
            if (frameDiff <= _simultaneousInputFrameWindow)
            {
                currentGroup.Add(history[i]);
            }
            else
            {
                grouped.Add(currentGroup);
                currentGroup = new List<InputEvent> { history[i] };
            }
        }
        
        if (currentGroup.Count > 0)
        {
            grouped.Add(currentGroup);
        }
        
        return grouped;
    }
    
    public void ClearDisplay()
    {
        if (_historyText != null)
            _historyText.text = "";
    }
}

