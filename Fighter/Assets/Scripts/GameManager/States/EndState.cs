using System.Collections.Generic;
using UnityEngine;

public class EndState : State
{
    private GameManager _gm;
    public EndState(GameManager stateMachine) : base(stateMachine) { _gm = stateMachine; }

    public override void Enter()
    {
        string winner = _gm.p1Score >= _gm.pointsToWin ? "Player 1" : "Player 2";
        Debug.Log("***************************");
        Debug.Log($"MATCH OVER: {winner} IS THE CHAMPION!");
        Debug.Log("***************************");
    }
}