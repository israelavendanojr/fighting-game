using System.Collections.Generic;
using UnityEngine;

public class PostRoundState : State
{
    private GameManager _gm;
    private float _waitTimer;

    public PostRoundState(GameManager stateMachine) : base(stateMachine) { _gm = stateMachine; }

    public override void Enter()
    {
        _waitTimer = _gm.postRoundDelay;

        // Determine winner and trigger events
        if (_gm.p1Health.GetCurrentHealth() > _gm.p2Health.GetCurrentHealth())
        {
            _gm.p1Score++;
            _gm.onP1ScoreIncreased?.Raise(); 
        }
        else if (_gm.p2Health.GetCurrentHealth() > _gm.p1Health.GetCurrentHealth())
        {
            _gm.p2Score++;
            _gm.onP2ScoreIncreased?.Raise();
        }
    }

    public override void Update()
    {
        _waitTimer -= Time.deltaTime;

        if (_waitTimer <= 0)
        {
            if (_gm.p1Score >= _gm.pointsToWin || _gm.p2Score >= _gm.pointsToWin)
            {
                _gm.SetState(new EndState(_gm));
            }
            else
            {
                _gm.ResetRound(); // Manual reset instead of scene load
            }
        }
    }
}