using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages the menu screen, where players can choose the type of puzzle the system will generate for them.
/// </summary>
public class MineSweeperMenuScreen : MonoBehaviour
{
    [Header("Asset References")]
    [SerializeField]
    private MineSweeperEventsSO _mineSweeperEvents;

    [Header("Object References")]
    [SerializeField]
    private GameObject _menuPanel;
    [SerializeField]
    private GameObject _heightSliderParent;

    [Header("Script References")]
    [SerializeField]
    private Toggle _heightEqualsWidthToggle;
    [SerializeField]
    private Slider _widthSlider;
    [SerializeField]
    private Slider _heightSlider;
    [SerializeField]
    private Toggle _mineSoftEnforcementToggle;
    [SerializeField]
    private Slider _mineCountSlider;

    [Header("Level Setup Parameters")]
    [SerializeField]
    private float _minMineCountMultiplier;
    [SerializeField]
    private float _maxMineCountMultiplier;
    [SerializeField]
    private float _easyRangeMinMultiplier;
    [SerializeField]
    private float _easyRangeMaxMultiplier;
    [SerializeField]
    private float _mediumRangeMinMultiplier;
    [SerializeField]
    private float _mediumRangeMaxMultiplier;
    [SerializeField]
    private float _hardRangeMinMultiplier;
    [SerializeField]
    private float _hardRangeMaxMultiplier;


    [Header("Level Setup Text Displays")]
    [SerializeField]
    private TextMeshProUGUI _widthSliderTextDisplay;
    [SerializeField]
    private TextMeshProUGUI _heightSliderTextDisplay;
    [SerializeField]
    private TextMeshProUGUI _mineCountSliderTextDisplay;

    private void OnDisable()
    {
        _mineSweeperEvents.OnReturnToSetup -= ShowMenuPanel;
    }

    private void OnEnable()
    {
        // Event Subscription: When the "return to setup" event is called - show the menu screen.
        _mineSweeperEvents.OnReturnToSetup += ShowMenuPanel;
    }

    void Start()
    {
        UpdateLevelSetupVisuals();
    }

    /// <summary>
    /// Updates all menu visuals to ensure the "planned" level parameters are accurately presented to the player.
    /// </summary>
    public void UpdateLevelSetupVisuals()
    {
        // 1. Show/Hide height slider.
        if (_heightEqualsWidthToggle.isOn) _heightSliderParent.SetActive(false);
        else _heightSliderParent.SetActive(true);

        // 2. Get slider values.
        int widthSliderValue = (int)_widthSlider.value;
        int heightSliderValue = _heightEqualsWidthToggle.isOn ? (int)_widthSlider.value : (int)_heightSlider.value;
        int mineSliderValue = (int)_mineCountSlider.value;
        
        // 3. Calculate current setup parameters total cells (level size - width * height).
        int totalCells = widthSliderValue * heightSliderValue;

        // 4. Set mine slider range based on enforcement toggle:
        // Soft enforcement means the player chooses a difficulty level,
        // the number of mines will be randomized based on the difficulty from an appropriate range.
        if (_mineSoftEnforcementToggle.isOn)
        {
            _mineCountSlider.maxValue = 3;
            _mineCountSlider.minValue = 1;

            if (mineSliderValue > _mineCountSlider.maxValue || mineSliderValue < _mineCountSlider.minValue)
            {
                _mineCountSlider.value = _mineCountSlider.minValue;
                mineSliderValue = (int)_mineCountSlider.value;
            }
        }
        // Hard enforcement means the player can choose the specific number of mines they will have, between an upper/lower limit.
        else
        {
            _mineCountSlider.maxValue = Mathf.Ceil(totalCells * _maxMineCountMultiplier);
            _mineCountSlider.minValue = Mathf.Ceil(totalCells * _minMineCountMultiplier);

            if (mineSliderValue > _mineCountSlider.maxValue || mineSliderValue < _mineCountSlider.minValue)
            {
                _mineCountSlider.value = _mineCountSlider.minValue;
                mineSliderValue = (int)_mineCountSlider.value;
            }
        }

        // 5. Set slider value displays.
        _widthSliderTextDisplay.text = widthSliderValue.ToString();
        _heightSliderTextDisplay.text = heightSliderValue.ToString();

        // Sets mine slider display based on current enforcement type.
        if (_mineSoftEnforcementToggle.isOn)
        {
            switch (mineSliderValue)
            {
                // Easy difficulty.
                default:
                    _mineCountSliderTextDisplay.text = $"Easy ({Mathf.Ceil(totalCells * _easyRangeMinMultiplier)} - {Mathf.Ceil(totalCells * _easyRangeMaxMultiplier)})";
                    break;
                
                // Medium difficulty.
                case 2:
                    _mineCountSliderTextDisplay.text = $"Medium ({Mathf.Ceil(totalCells * _mediumRangeMinMultiplier)} - {Mathf.Ceil(totalCells * _mediumRangeMaxMultiplier)})";
                    break;

                // Hard difficulty.
                case 3:
                    _mineCountSliderTextDisplay.text = $"Hard ({Mathf.Ceil(totalCells * _hardRangeMinMultiplier)} - {Mathf.Ceil(totalCells * _hardRangeMaxMultiplier)})";
                    break;
            }
        }
        else _mineCountSliderTextDisplay.text = mineSliderValue.ToString();
    }

    /// <summary>
    /// Hnadles the process to start the game.<br/><br/>
    /// 1. Calculate level parameters.<br/>
    /// 2. Set parameters to manager for easy access.<br/>
    /// 3 & 4. hide menu and start the game.
    /// </summary>
    public void StartGame()
    {
        // 1. Calculate Level Parameters:
        // Width.
        int width = (int)_widthSlider.value;
        // Height.
        int height = _heightEqualsWidthToggle.isOn ? (int)_widthSlider.value : (int)_heightSlider.value;
        // Mine Count.
        int totalCells = width * height;
        int mineCount;
        if (_mineSoftEnforcementToggle.isOn)
        {
            switch ((int)_mineCountSlider.value)
            {
                // Easy difficulty.
                default:
                    mineCount = (int)Random.Range(Mathf.Ceil(totalCells * _easyRangeMinMultiplier), Mathf.Ceil(totalCells * _easyRangeMaxMultiplier));
                    break;
                
                // Medium difficulty.
                case 2:
                    mineCount = (int)Random.Range(Mathf.Ceil(totalCells * _mediumRangeMinMultiplier), Mathf.Ceil(totalCells * _mediumRangeMaxMultiplier));
                    break;

                // Hard difficulty.
                case 3:
                    mineCount = (int)Random.Range(Mathf.Ceil(totalCells * _hardRangeMinMultiplier), Mathf.Ceil(totalCells * _hardRangeMaxMultiplier));
                    break;
            }
        }
        else mineCount = (int)_mineCountSlider.value;

        // 2. Set level parameters in the Manager (ease of access).
        MineSweeperManager.Instance.SetLevelParameters(width, height, mineCount);
        
        // 3. Hide menu screen.
        _menuPanel.SetActive(false);

        // 4. Start the level - calls "start game" event.
        _mineSweeperEvents.TriggerStartGame();
    }

    /// <summary>
    /// Reveals the menu screen.
    /// </summary>
    private void ShowMenuPanel()
    {
        // Enable menu screen object.
        _menuPanel.SetActive(true);
        // Update menu screen visual (just to be safe).
        UpdateLevelSetupVisuals();
    }


}
