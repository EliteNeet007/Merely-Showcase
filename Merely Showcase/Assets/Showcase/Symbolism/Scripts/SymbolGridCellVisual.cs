using UnityEngine;

public class SymbolGridCellVisual : MonoBehaviour
{
    [SerializeField] private GameObject[] _symbolPrefabs;
    [SerializeField] private Transform _symbolVisualParent;

    [Header("Pedestal Highlight")]
    [SerializeField] private MeshRenderer _pedestalBaseRenderer;
    [SerializeField] private MeshRenderer _pedestalElevatedStepRenderer;
    [SerializeField] private Material _pedestalDefaultMaterial;
    [SerializeField] private Material _pedestalHighlightMaterial;

    private GameObject _currentVisual;
    private bool _isHighlighted;

    public bool IsHighlighted { get { return _isHighlighted; } }

    public void SetHighlightState(bool highlight)
    {
        if (highlight)
        {
            _pedestalBaseRenderer.material = _pedestalHighlightMaterial;
            _pedestalElevatedStepRenderer.material = _pedestalHighlightMaterial;
        }
        else
        {
            _pedestalBaseRenderer.material = _pedestalDefaultMaterial;
            _pedestalElevatedStepRenderer.material = _pedestalDefaultMaterial;
        }
    }

    public void SwapSymbol(SymbolGridManager.Symbol newSymbol)
    {
        // Check if the cell already displays a symbol visual, if so - destroy it before we spawn the new visual.
        if (_currentVisual != null) Destroy(_currentVisual);

        // Spawn the visual that matches the newSymbol to be swapped to.
        _currentVisual = Instantiate(_symbolPrefabs[(int)newSymbol - 1], _symbolVisualParent);
    }
}
