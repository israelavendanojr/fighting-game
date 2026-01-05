using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

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
    private InputAction _throwAction;
    
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
        _throwAction = _playerInput.actions["Throw"];
        
        // Subscribe to button input events
        _lightAction.performed += ctx => OnButtonPressed(BufferInput.Light);
        _lightAction.canceled += ctx => OnButtonReleased(BufferInput.Light);
        
        _mediumAction.performed += ctx => OnButtonPressed(BufferInput.Medium);
        _mediumAction.canceled += ctx => OnButtonReleased(BufferInput.Medium);
        
        _heavyAction.performed += ctx => OnButtonPressed(BufferInput.Heavy);
        _heavyAction.canceled += ctx => OnButtonReleased(BufferInput.Heavy);
        
        _dashAction.performed += ctx => OnButtonPressed(BufferInput.Dash);
        _dashAction.canceled += ctx => OnButtonReleased(BufferInput.Dash);
        
        _throwAction.performed += ctx => OnButtonPressed(BufferInput.Throw);
        _throwAction.canceled += ctx => OnButtonReleased(BufferInput.Throw);
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
        bool left = _currentMoveInput.x < -0.5f;
        bool right = _currentMoveInput.x > 0.5f;
        bool up = _currentMoveInput.y > 0.5f;
        bool down = _currentMoveInput.y < -0.5f;
        
        // Determine current numpad direction
        BufferInput currentDirection = DetermineNumpadDirection(left, right, up, down);
        
        // Release all other directions
        for (int i = 0; i <= 8; i++)
        {
            BufferInput dir = (BufferInput)i;
            if (dir != currentDirection && _inputBuffer.IsInputHeld(dir))
            {
                _inputBuffer.OnInputReleased(dir);
            }
        }
        
        // Press current direction if not already held
        if (!_inputBuffer.IsInputHeld(currentDirection))
        {
            _inputBuffer.OnInputPressed(currentDirection);
        }
    }
    
    private BufferInput DetermineNumpadDirection(bool left, bool right, bool up, bool down)
    {
        // Cancel opposing directions
        // if (left && right) { left = false; right = false; }
        // if (up && down) { up = false; down = false; }
        
        // Numpad mapping (7 8 9 / 4 5 6 / 1 2 3)
        if (up)
        {
            if (left) return BufferInput.UpBack;      // 7
            if (right) return BufferInput.UpForward;  // 9
            return BufferInput.Up;                    // 8
        }
        else if (down)
        {
            if (left) return BufferInput.DownBack;     // 1
            if (right) return BufferInput.DownForward; // 3
            return BufferInput.Down;                   // 2
        }
        else
        {
            if (left) return BufferInput.Back;         // 4
            if (right) return BufferInput.Forward;     // 6
            return BufferInput.Neutral;                // 5
        }
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