using System;
using UnityEngine;

/// <summary>
/// Centralized event hub for minesweeper.
/// </summary>
[CreateAssetMenu(fileName = "MineSweeperEventsSO", menuName = "Scriptable Objects/MineSweeperEventsSO")]
public class MineSweeperEventsSO : ScriptableObject
{
    public event Action OnStartGame;
    public void TriggerStartGame()
    {
        OnStartGame?.Invoke();
    }

    public event Action<bool> OnGameFinished;
    public void TriggerGameFinished(bool gameWon)
    {
        OnGameFinished?.Invoke(gameWon);
    }

    public event Action OnPlayAgain;
    public void TriggerPlayAgain()
    {
        OnPlayAgain?.Invoke();
    }

    public event Action OnReturnToSetup;
    public void TriggerReturnToSetup()
    {
        OnReturnToSetup?.Invoke();
    }


}
