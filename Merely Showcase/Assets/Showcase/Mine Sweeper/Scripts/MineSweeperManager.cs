using UnityEngine;
using RightNowGames.Basic;

public class MineSweeperManager : Singleton<MineSweeperManager>
{
    [SerializeField]
    private MineSweeperEventsSO _mineSweeperEvents;

    private int _levelGridWidth;
    private int _levelGridHeight;
    private int _levelMineCount;

    public int LevelGridWidth { get { return _levelGridWidth; } }
    public int LevelGridHeight { get { return _levelGridHeight; } }
    public int LevelMineCount { get { return _levelMineCount; } }


    protected override void Awake()
    {
        base.Awake();

        _levelGridWidth = 10;
        _levelGridHeight = 10;
        _levelMineCount = 10;
    }

    public void SetLevelParameters(int width, int height, int mineCount)
    {
        _levelGridWidth = width;
        _levelGridHeight = height;
        _levelMineCount = mineCount;
    }

    public void StartGame()
    {
        _mineSweeperEvents.TriggerStartGame();
    }
}
