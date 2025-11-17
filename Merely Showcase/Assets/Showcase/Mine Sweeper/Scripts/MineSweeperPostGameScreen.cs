using UnityEngine;
using TMPro;

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
        _mineSweeperEvents.OnGameFinished += ShowPostGamePanel;
    }

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

    private void HidePostGamePanel()
    {
        _postGamePanel.SetActive(false);
    }

    public void PlayAgain()
    {
        HidePostGamePanel();
        _mineSweeperEvents.TriggerPlayAgain();
    }

    public void ReturnToSetup()
    {
        HidePostGamePanel();
        _mineSweeperEvents.TriggerReturnToSetup();
    }


}
