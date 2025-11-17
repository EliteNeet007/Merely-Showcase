using UnityEngine;
using TMPro;
using RightNowGames.Basic;

/// <summary>
/// Mini-Script to handle minesweeper gameplay UI.
/// </summary>
public class MineSweeperGameplayUI : StaticInstance<MineSweeperGameplayUI>
{
    [SerializeField]
    private GameObject _gameplayUIParent;
    [SerializeField]
    private TextMeshProUGUI _flagsLeftUI;

    // Calls base Awake implementation to set-up static Instance.
    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// Enables us to see the gameplay UI.
    /// </summary>
    public void ShowGameplayUI()
    {
        _gameplayUIParent.SetActive(true);
    }

    /// <summary>
    /// Hides the gameplay UI.
    /// </summary>
    public void HideGameplayUI()
    {
        _gameplayUIParent.SetActive(false);
    }

    /// <summary>
    /// Updates the text indicating how many mines are left (flags left to place.)
    /// </summary>
    /// <param name="flagsLeft"></param>
    public void SetFlagsLeftText(int flagsLeft)
    {
        _flagsLeftUI.text = $" {flagsLeft}";
    }
}
