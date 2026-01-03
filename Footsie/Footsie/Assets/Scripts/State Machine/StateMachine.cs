using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class StateMachine : MonoBehaviour
{
    protected State _currentState;
    public virtual State InitialState() => null;
    void Start()
    {
        _currentState = InitialState();
        _currentState.Enter();
    }
    void Update()
    {
        _currentState.Update();
    }
    void FixedUpdate()
    {
        _currentState.FixedUpdate();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        _currentState.OnCollisionEnter(collision);
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        _currentState.OnTriggerEnter(collider);
    }
    public void SetState(State state)
    {
        _currentState.Exit();
        _currentState = state;
        _currentState.Enter();
    }
    
    #if UNITY_EDITOR
    void OnDrawGizmos() 
    {
        // Only run this code in the editor when the game is running or paused.
        if (!Application.isPlaying || _currentState == null)
            return;

        Handles.color = Color.green;
        Handles.Label(transform.position + Vector3.up * 1.5f, _currentState.ToString()); 
    }
    #endif
}
