using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        _mineSweeperEvents.OnReturnToSetup += ShowMenuPanel;
    }

    void Start()
    {
        UpdateLevelSetupVisuals();
    }

    public void UpdateLevelSetupVisuals()
    {
        // Show/Hide height slider.
        if (_heightEqualsWidthToggle.isOn) _heightSliderParent.SetActive(false);
        else _heightSliderParent.SetActive(true);

        // Get slider values.
        int widthSliderValue = (int)_widthSlider.value;
        int heightSliderValue = _heightEqualsWidthToggle.isOn ? (int)_widthSlider.value : (int)_heightSlider.value;
        int mineSliderValue = (int)_mineCountSlider.value;
        
        // Calculate current setup parameters level size.
        int totalCells = widthSliderValue * heightSliderValue;

        // Set mine slider range based on enforcement toggle.
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

        // Set slider value displays.
        _widthSliderTextDisplay.text = widthSliderValue.ToString();
        _heightSliderTextDisplay.text = heightSliderValue.ToString();

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

    public void StartGame()
    {
        // Calculate Level Parameters:
        // 1. Width.
        int width = (int)_widthSlider.value;
        // 2. Height.
        int height = _heightEqualsWidthToggle.isOn ? (int)_widthSlider.value : (int)_heightSlider.value;
        // 3. Mine Count.
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

        // Set level parameters.
        MineSweeperManager.Instance.SetLevelParameters(width, height, mineCount);
        
        // Hide menu screen.
        _menuPanel.SetActive(false);

        // Start the level.
        MineSweeperManager.Instance.StartGame();
    }

    private void ShowMenuPanel()
    {
        _menuPanel.SetActive(true);
        UpdateLevelSetupVisuals();
    }
}
