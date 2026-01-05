using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMachine : StateMachine
{
    // States
    public CharacterStandState StandState { get; private set; }



    private void Awake()
    {
        // Initialize all states
        StandState = new CharacterStandState(this);

    }

    public override State InitialState() => StandState;
}


