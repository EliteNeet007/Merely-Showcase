using UnityEngine;
using RightNowGames.Basic;
using System.Collections.Generic;

/// <summary>
/// Overarching manager, used to easily access relevant data.<br/>
/// Can be extended to support saving player preferences, high scores, etc.
/// </summary>
public class MineSweeperManager : Singleton<MineSweeperManager>
{
    [Header("Number Text Colors")]
    [SerializeField]
    private List<Color> _numberTextColors;

    private int _levelGridWidth;
    private int _levelGridHeight;
    private int _levelMineCount;

    public List<Color> NumberTextColor { get { return _numberTextColors; } }
    public int LevelGridWidth { get { return _levelGridWidth; } }
    public int LevelGridHeight { get { return _levelGridHeight; } }
    public int LevelMineCount { get { return _levelMineCount; } }

    // Calls base awake implementation & sets default values.
    protected override void Awake()
    {
        base.Awake();

        _levelGridWidth = 10;
        _levelGridHeight = 10;
        _levelMineCount = 10;
    }

    /// <summary>
    /// Called to sets the level parameters that were chosen in the setup screen.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="mineCount"></param>
    public void SetLevelParameters(int width, int height, int mineCount)
    {
        _levelGridWidth = width;
        _levelGridHeight = height;
        _levelMineCount = mineCount;
    }

    
}
