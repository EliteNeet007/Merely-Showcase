using UnityEngine;
using TMPro;

/// <summary>
/// Menu handler for the post-game screen (Win/Lose screen)
/// </summary>
public class MineSweeperPostGameScreen : MonoBehaviour
{
    [Header("Asset References")]
    [SerializeField]
    private MineSweeperEventsSO _mineSweeperEvents;

    [Header("Object References")]
    [SerializeField]
    private GameObject _postGamePanel;
    [SerializeField]
    private TextMeshProUGUI _gameCompletionStateText;

    [Header("Values")]
    [SerializeField]
    private Color _winTextColor;
    [SerializeField]
    private Color _loseTextColor;

    private void OnDisable()
    {
        _mineSweeperEvents.OnGameFinished -= ShowPostGamePanel;
    }

    private void OnEnable()
    {
        // Event Subscription: reveal post-game screen when the "game finished" event is called.
        _mineSweeperEvents.OnGameFinished += ShowPostGamePanel;
    }

    /// <summary>
    /// Reveals the post-game panel and sets it's headline based on game result.
    /// </summary>
    /// <param name="gameWon"></param>
    private void ShowPostGamePanel(bool gameWon)
    {
        if (gameWon)
        {
            _gameCompletionStateText.text = "You Won!";
            _gameCompletionStateText.color = _winTextColor;
        }
        else
        {
            _gameCompletionStateText.text = "You Lost...";
            _gameCompletionStateText.color = _loseTextColor;
        }

        _postGamePanel.SetActive(true);
    }

    /// <summary>
    /// Hides the post-game panel.
    /// </summary>
    private void HidePostGamePanel()
    {
        _postGamePanel.SetActive(false);
    }

    /// <summary>
    /// Triggers "play again" event.
    /// </summary>
    public void PlayAgain()
    {
        HidePostGamePanel();
        _mineSweeperEvents.TriggerPlayAgain();
    }

    /// <summary>
    /// Triggers "return to setup" event.
    /// </summary>
    public void ReturnToSetup()
    {
        HidePostGamePanel();
        _mineSweeperEvents.TriggerReturnToSetup();
    }


}
