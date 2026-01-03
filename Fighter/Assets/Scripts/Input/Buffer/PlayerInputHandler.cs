using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private int _playerIndex; // 0 for P1, 1 for P2
    [SerializeField] private CharacterStateMachine _characterStateMachine;
    
    private PlayerInputBuffer _inputBuffer;
    private PlayerInput _playerInput;
    
    // Input Action references
    private InputAction _moveAction;
    private InputAction _lightAction;
    private InputAction _mediumAction;
    private InputAction _heavyAction;
    private InputAction _dashAction;
    
    // Current directional input
    private Vector2 _currentMoveInput;
    
    public PlayerInputBuffer InputBuffer => _inputBuffer;
    
    private void Awake()
    {
        _inputBuffer = new PlayerInputBuffer(30);
        _playerInput = GetComponent<PlayerInput>();
        
        // Get references to input actions
        _moveAction = _playerInput.actions["Move"];
        _lightAction = _playerInput.actions["Light"];
        _mediumAction = _playerInput.actions["Medium"];
        _heavyAction = _playerInput.actions["Heavy"];
        _dashAction = _playerInput.actions["Dash"];
        
        
        // Subscribe to input events
        _lightAction.performed += ctx => OnButtonPressed(BufferInput.Light);
        _lightAction.canceled += ctx => OnButtonReleased(BufferInput.Light);
        
        _mediumAction.performed += ctx => OnButtonPressed(BufferInput.Medium);
        _mediumAction.canceled += ctx => OnButtonReleased(BufferInput.Medium);
        
        _heavyAction.performed += ctx => OnButtonPressed(BufferInput.Heavy);
        _heavyAction.canceled += ctx => OnButtonReleased(BufferInput.Heavy);
        
        _dashAction.performed += ctx => OnButtonPressed(BufferInput.Dash);
        _dashAction.canceled += ctx => OnButtonReleased(BufferInput.Dash);
    }
    
    private void Update()
    {
        // Update directional input
        _currentMoveInput = _moveAction.ReadValue<Vector2>();
        HandleDirectionalInput();
        
        // Update buffer to remove old inputs
        _inputBuffer.Update();
    }
    
    private void HandleDirectionalInput()
    {
        // Handle the 4 directions as separate inputs for motion input detection
        // This handles transitions properly (e.g., moving from left to right)
        
        bool left = _currentMoveInput.x < -0.5f;
        bool right = _currentMoveInput.x > 0.5f;
        bool up = _currentMoveInput.y > 0.5f;
        bool down = _currentMoveInput.y < -0.5f;
        
        // Check for neutral: no directional input OR opposing directions cancel out
        bool horizontalNeutral = (!left && !right) || (left && right);
        bool verticalNeutral = (!up && !down) || (up && down);
        bool isNeutral = horizontalNeutral && verticalNeutral;
        
        // Neutral state
        if (isNeutral && !_inputBuffer.IsInputHeld(BufferInput.Neutral))
        {
            _inputBuffer.OnInputPressed(BufferInput.Neutral);
        }
        else if (!isNeutral && _inputBuffer.IsInputHeld(BufferInput.Neutral))
        {
            _inputBuffer.OnInputReleased(BufferInput.Neutral);
        }
        
        // Left
        if (left && !right && !_inputBuffer.IsInputHeld(BufferInput.Left))
            _inputBuffer.OnInputPressed(BufferInput.Left);
        else if ((!left || right) && _inputBuffer.IsInputHeld(BufferInput.Left))
            _inputBuffer.OnInputReleased(BufferInput.Left);
        
        // Right
        if (right && !left && !_inputBuffer.IsInputHeld(BufferInput.Right))
            _inputBuffer.OnInputPressed(BufferInput.Right);
        else if ((!right || left) && _inputBuffer.IsInputHeld(BufferInput.Right))
            _inputBuffer.OnInputReleased(BufferInput.Right);
        
        // Up
        if (up && !down && !_inputBuffer.IsInputHeld(BufferInput.Up))
            _inputBuffer.OnInputPressed(BufferInput.Up);
        else if ((!up || down) && _inputBuffer.IsInputHeld(BufferInput.Up))
            _inputBuffer.OnInputReleased(BufferInput.Up);
        
        // Down
        if (down && !up && !_inputBuffer.IsInputHeld(BufferInput.Down))
            _inputBuffer.OnInputPressed(BufferInput.Down);
        else if ((!down || up) && _inputBuffer.IsInputHeld(BufferInput.Down))
            _inputBuffer.OnInputReleased(BufferInput.Down);
    }
    
    private void OnButtonPressed(BufferInput input)
    {
        _inputBuffer.OnInputPressed(input);
    }
    
    private void OnButtonReleased(BufferInput input)
    {
        _inputBuffer.OnInputReleased(input);
    }
    
    public Vector2 GetMoveInput()
    {
        return _currentMoveInput;
    }
    
    // For AI or replay: you can inject inputs directly into the buffer
    public void InjectInput(BufferInput input, bool pressed)
    {
        if (pressed)
            _inputBuffer.OnInputPressed(input);
        else
            _inputBuffer.OnInputReleased(input);
    }
}