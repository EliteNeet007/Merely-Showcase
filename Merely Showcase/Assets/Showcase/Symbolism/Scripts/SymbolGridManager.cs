using UnityEngine;
using RightNowGames.Grids;
using RightNowGames.Utilities;

public class SymbolGridManager : MonoBehaviour
{
    public enum Symbol
    {
        None,
        Circle,
        Triangle,
        Cube,
        Spike,
    }

    [SerializeField] private GameObject _gridCellVisualPrefab;

    [SerializeField] private int _gridWidth;
    [SerializeField] private int _gridHeight;
    [SerializeField] private int _gridCellSize;
    [SerializeField] private float _gridCellSpacing;

    private Grid2D<Symbol> _symbolGrid;
    private SymbolGridCellVisual[,] _symbolVisuals;
    private int _maxSymbolValue;

    private void Start()
    {
        InitializeSymbolGrid(true);
    }

    public void InitializeSymbolGrid(bool overrideOriginPosition = false)
    {
        // Check if we require an override for the grid's origin.
        // If such action is required calculate the appropriate position and move this object to it BEFORE setting up the grid.
        // Ideally only used in the design scene.
        if (overrideOriginPosition) CalculateAndSetOriginPosition();

        // Initialize underlying grid and visual cache.
        _symbolGrid = new(_gridWidth, _gridHeight, transform.position, _gridCellSize, _gridCellSpacing, RightNowGames.Enums.GridLayoutType2D.Horizontal);
        _symbolVisuals = new SymbolGridCellVisual[_gridWidth, _gridHeight];
        _maxSymbolValue = (int)Symbol.Spike;

        // Spawn and cache grid visual elements.
        for (int x = 0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                // We spawn and simultaneously cache the visual object's script reference.
                //                                 1. what we spawn.      2. where we spawn it.                         3. how it is rotated.    4. parent.
                _symbolVisuals[x, y] = Instantiate(_gridCellVisualPrefab, _symbolGrid.GetCellCenterWorldPosition(x, y), transform.localRotation, transform).GetComponent<SymbolGridCellVisual>();
            }
        }
    }

    /// <summary>
    /// 1. Sum up the grid's total area to arrive at the offset we must apply to the grid's origin position for the grid's center to be placed at (0, 0).<br/>
    /// 2. Place this object at the calculated position
    /// </summary>
    private void CalculateAndSetOriginPosition()
    {
        // Grid total width.
        float gridX = (_gridWidth * _gridCellSize) + ((_gridWidth - 1) * _gridCellSpacing);
        // Grid total height.
        float gridY = (_gridHeight * _gridCellSize) + ((_gridHeight - 1) * _gridCellSpacing);
        // Apply positional offset to this object.
        transform.position = new Vector3(-(gridX / 2), transform.position.y, -(gridY / 2));
    }

    /// <summary>
    /// Checks if the signature position corresponds to a valid grid position.<br/>
    /// If so - increments the cell's symbol and updates the visual.
    /// </summary>
    /// <param name="worldPosition"></param>
    public void InteractWithGridCell(Vector3 worldPosition)
    {
        if (_symbolGrid.IsValidGridPosition(worldPosition))
        {
            // Gather current symbol info.
            Vector2Int gridPosition = _symbolGrid.GetVectorInts(worldPosition);
            Symbol symbol = _symbolGrid.GetGridObject(gridPosition);

            // Increment symbol value.
            symbol++;
            if ((int)symbol > _maxSymbolValue) symbol = (Symbol)1;

            // Set new symbol info.
            _symbolGrid.SetGridObject(gridPosition, symbol);
            _symbolVisuals[gridPosition.x, gridPosition.y].SwapSymbol(symbol);
        }
    }


}
