using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEvent Event;
    public UnityEvent Response;

    private void OnEnable()
    { Event.RegisterListener(this); }
    private void OnDisable()
    { Event.UnregisterListener(this); }

    public void OnEventRaised()
    { 

        if (this == null)
        {
            return;
        }

        if (Response != null)
        {
            Response.Invoke();
        }
        else
        {
            UnityEngine.Debug.LogWarning($"GameEventListener on {gameObject.name} has a null Response field.", this);
        } }

}