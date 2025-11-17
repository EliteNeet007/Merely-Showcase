using TMPro;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

/// <summary>
/// Responsible for the visual representation of a single board cell in the minesweeper game.
/// </summary>
public class MineSweeperCellVisual : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField]
    private SpriteRenderer _cellForegroundVisual;
    [SerializeField]
    private SpriteRenderer _cellBackgroundVisual;
    [SerializeField]
    private GameObject _flagObject;
    [SerializeField]
    private TextMeshPro _numberText;

    [Header("Values")]
    [SerializeField]
    private Sprite _mineSprite;
    [SerializeField]
    private Sprite _cellForegroundSprite;
    [SerializeField]
    private Color _mineBackgroundColor;

    private bool _isFlagged;
    private bool _isMine;
    private bool _isRevealed;
    private int _number;

    public bool IsFlagged => _isFlagged;
    public bool IsRevealed => _isRevealed;
    public int Number => _number;

    /// <summary>
    /// Marks this cell as a mine.
    /// </summary>
    public void SetMine() { _isMine = true; }

    /// <summary>
    /// Flips and sets the cell's flagged state + visual.
    /// </summary>
    public void FlipFlaggedState()
    {
        // Set visual active state.
        if (_isFlagged) _flagObject.SetActive(false);
        else _flagObject.SetActive(true);

        // Flip bool indicator.
        _isFlagged = !_isFlagged;
    }

    /// <summary>
    /// Sets the cell's number-related parameters to the appropriate value.
    /// </summary>
    /// <param name="value"></param>
    public void SetNumber(int value)
    {
        // Cache number value.
        _number = value;
        // Assign number text & color.
        if (_number > 0)
        {
            _numberText.text = _number.ToString();
            _numberText.color = MineSweeperManager.Instance.NumberTextColor[_number - 1];
        }
    }

    /// <summary>
    /// Reveal the cell's contents.
    /// </summary>
    public void RevealCell(bool markMineBackground = false)
    {
        // Removes the cell's flag marking if the is currently flagged.
        if (_isFlagged)
        {
            _flagObject.SetActive(false);
            _isFlagged = false;
        }

        // Swaps the cell's foreground to a mine sprite, if the cell contains a mine.
        if (_isMine)
        {
            _cellForegroundVisual.sprite = _mineSprite;
            if (markMineBackground) _cellBackgroundVisual.color = _mineBackgroundColor;
        }
        // Disables the cell's foreground and enables the cell's number visual.
        else
        {
            _cellForegroundVisual.gameObject.SetActive(false);
            if (_number > 0) _numberText.gameObject.SetActive(true);
        }

        // Sets revealed state indicator.
        _isRevealed = true;
    }

    /// <summary>
    /// Sets the cell to it's starting (default) values.
    /// </summary>
    public void ResetCell()
    {
        _number = 0;
        _isMine = false;
        _isFlagged = false;
        _isRevealed = false;
        _numberText.text = "";
        _numberText.color = Color.white;
        _flagObject.SetActive(false);
        _cellBackgroundVisual.color = Color.white;
        _cellForegroundVisual.sprite = _cellForegroundSprite;
    }

    
}
