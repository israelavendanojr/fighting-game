using System.Collections.Generic;
using System.Text;
using System.Linq; // Added for sorting
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
    [SerializeField] private int _simultaneousInputFrameWindow = 2;
    
    private StringBuilder _stringBuilder = new StringBuilder();
    private Dictionary<BufferInput, string> _inputSymbols;

    private void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        // Initialize if null, otherwise just clear it
        if (_inputSymbols == null)
        {
            _inputSymbols = new Dictionary<BufferInput, string>();
        }
        else
        {
            _inputSymbols.Clear();
        }

        // Using the indexer [] instead of .Add() prevents the "Key already added" crash
        _inputSymbols[BufferInput.DownBack] = "↙";
        _inputSymbols[BufferInput.Down] = "↓";
        _inputSymbols[BufferInput.DownForward] = "↘";
        _inputSymbols[BufferInput.Back] = "←";
        _inputSymbols[BufferInput.Neutral] = "-";
        _inputSymbols[BufferInput.Forward] = "→";
        _inputSymbols[BufferInput.UpBack] = "↖";
        _inputSymbols[BufferInput.Up] = "↑";
        _inputSymbols[BufferInput.UpForward] = "↗";
        
        // Actions matching your .inputactions file
        _inputSymbols[BufferInput.Light] = "L";   // Action: Light [cite: 3]
        _inputSymbols[BufferInput.Medium] = "M";  // Action: Medium [cite: 5]
        _inputSymbols[BufferInput.Heavy] = "H";   // Action: Heavy [cite: 7]
        _inputSymbols[BufferInput.Dash] = "D";    // Action: Dash [cite: 9]
        _inputSymbols[BufferInput.Throw] = "T";   // Action: Throw [cite: 11]
    }

    private void Update() => UpdateDisplay();
    
    private void UpdateDisplay()
    {
        if (_inputHandler == null || _historyText == null || _inputSymbols == null)
            return;
        
        if (_inputHandler.InputBuffer == null) return;
        var history = _inputHandler.InputBuffer.GetInputHistory();
        
        if (history == null || history.Count == 0)
        {
            _historyText.text = "";
            return;
        }
        
        _stringBuilder.Clear();
        
        if (_combineSimultaneousInputs)
            DisplayCombinedInputs(history);
        else
            DisplayIndividualInputs(history);
        
        _historyText.text = _stringBuilder.ToString();
    }

    private void DisplayIndividualInputs(List<InputEvent> history)
    {
        int displayCount = Mathf.Min(_maxDisplayedInputs, history.Count);
        int startIndex = Mathf.Max(0, history.Count - displayCount);
        
        for (int i = startIndex; i < history.Count; i++)
        {
            AppendInputEventString(history[i]);
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
            
            // Sort group: Directions first, then buttons
            // (Assuming BufferInput enum has directions as lower values)
            var sortedGroup = group.OrderBy(e => (int)e.Input).ToList();

            foreach (var inputEvent in sortedGroup)
            {
                _stringBuilder.Append(GetSymbol(inputEvent.Input));
            }
            
            if (_showFrames && group.Count > 0)
            {
                _stringBuilder.Append($" ({group[0].HeldFrames}f)");
            }
            
            _stringBuilder.AppendLine();
        }
    }

    private void AppendInputEventString(InputEvent inputEvent)
    {
        string symbol = GetSymbol(inputEvent.Input);
        if (_showFrames)
            _stringBuilder.Append($"{symbol} ({inputEvent.HeldFrames}f)");
        else
            _stringBuilder.Append(symbol);
    }

    private string GetSymbol(BufferInput input)
    {
        return _inputSymbols.TryGetValue(input, out string s) ? s : ((int)input).ToString();
    }

    private List<List<InputEvent>> GroupSimultaneousInputs(List<InputEvent> history)
    {
        var grouped = new List<List<InputEvent>>();
        if (history.Count == 0) return grouped;
        
        var currentGroup = new List<InputEvent> { history[0] };
        
        for (int i = 1; i < history.Count; i++)
        {
            // Calculate frame difference
            int frameDiff = Mathf.Abs(history[i].FramePressed - currentGroup[0].FramePressed);
            
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
        
        if (currentGroup.Count > 0) grouped.Add(currentGroup);
        return grouped;
    }

    public void ClearDisplay()
    {
        if (_historyText != null) _historyText.text = "";
    }
}