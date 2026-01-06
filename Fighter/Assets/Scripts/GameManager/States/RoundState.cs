using System.Collections.Generic;
using UnityEngine;
public class RoundState : State
{
    private GameManager _gm;
    private float _internalTimer;
    private int _lastSecond;

    public RoundState(GameManager stateMachine) : base(stateMachine) { _gm = stateMachine; }

    public override void Enter()
    {
        _internalTimer = _gm.roundDuration;
        _lastSecond = Mathf.CeilToInt(_internalTimer);
        if (_gm.timerUI != null) _gm.timerUI.UpdateSeconds(_lastSecond);
    }

    public override void Update()
    {
        _internalTimer -= Time.deltaTime;
        int currentSecond = Mathf.CeilToInt(_internalTimer);

        if (currentSecond < _lastSecond)
        {
            _lastSecond = currentSecond;
            if (_gm.timerUI != null) _gm.timerUI.UpdateSeconds(Mathf.Max(0, _lastSecond));
        }

        // Check if timer ended OR if any player died
        if (_internalTimer <= 0 || _gm.p1Health.GetCurrentHealth() <= 0 || _gm.p2Health.GetCurrentHealth() <= 0)
        {
            _gm.SetState(new PostRoundState(_gm));
        }
    }
}