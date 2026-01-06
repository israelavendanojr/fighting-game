using System.Collections.Generic;
using UnityEngine;


public class GameManager : StateMachine
{
    [Header("Spawn Settings")]
    public GameObject player1Prefab;
    public GameObject player2Prefab;
    public Transform spawnPoint1;
    public Transform spawnPoint2;

    [Header("Round Settings")]
    public float roundDuration = 60f;
    public float postRoundDelay = 3f;
    public int pointsToWin = 3;

    [Header("UI Direct References")]
    public TimerUI timerUI;
    public HealthBar p1HealthBar;
    public HealthBar p2HealthBar;

    [Header("Score Events")]
    public GameEvent onP1ScoreIncreased;
    public GameEvent onP2ScoreIncreased;

    [Header("Runtime Data")]
    public int p1Score;
    public int p2Score;
    [HideInInspector] public HealthComponent p1Health;
    [HideInInspector] public HealthComponent p2Health;

    public override State InitialState() => new PreRoundState(this);

    public void ResetRound()
    {
        // 1. Move players back to spawn points
        p1Health.transform.position = spawnPoint1.position;
        p2Health.transform.position = spawnPoint2.position;

        // 2. Reset Health (Heal takes an int)
        p1Health.Heal(p1Health.GetMaxHealth());
        p2Health.Heal(p2Health.GetMaxHealth());

        // 3. Restart the state machine into the Round state
        SetState(new RoundState(this));
    }
}