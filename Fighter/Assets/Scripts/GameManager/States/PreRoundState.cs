using System.Collections.Generic;
using UnityEngine;
public class PreRoundState : State
{
    private GameManager _gm;
    public PreRoundState(GameManager stateMachine) : base(stateMachine) { _gm = stateMachine; }

    public override void Enter()
    {
        // Only spawn if they don't exist yet
        if (_gm.p1Health == null)
        {
            GameObject p1 = Object.Instantiate(_gm.player1Prefab, _gm.spawnPoint1.position, _gm.spawnPoint1.rotation);
            _gm.p1Health = p1.GetComponent<HealthComponent>();
            _gm.p1HealthBar.Setup(_gm.p1Health);
        }

        if (_gm.p2Health == null)
        {
            GameObject p2 = Object.Instantiate(_gm.player2Prefab, _gm.spawnPoint2.position, _gm.spawnPoint2.rotation);
            _gm.p2Health = p2.GetComponent<HealthComponent>();
            _gm.p2HealthBar.Setup(_gm.p2Health);
        }

        _gm.SetState(new RoundState(_gm));
    }
}