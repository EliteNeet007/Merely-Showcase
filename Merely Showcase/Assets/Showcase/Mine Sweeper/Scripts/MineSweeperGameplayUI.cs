using UnityEngine;
using TMPro;
using RightNowGames.Basic;

public class MineSweeperGameplayUI : StaticInstance<MineSweeperGameplayUI>
{
    [SerializeField]
    private GameObject _gameplayUIParent;
    [SerializeField]
    private TextMeshProUGUI _flagsLeftUI;

    protected override void Awake()
    {
        base.Awake();
    }

    public void ShowGameplayUI()
    {
        _gameplayUIParent.SetActive(true);
    }

    public void HideGameplayUI()
    {
        _gameplayUIParent.SetActive(false);
    }

    public void SetFlagsLeftText(int flagsLeft)
    {
        _flagsLeftUI.text = $" {flagsLeft}";
    }
}
