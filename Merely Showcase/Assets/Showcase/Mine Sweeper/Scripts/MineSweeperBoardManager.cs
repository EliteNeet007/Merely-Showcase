using UnityEngine;
using System.Collections.Generic;
using RightNowGames.Grids;
using RightNowGames.Utilities;

public class MineSweeperBoardManager : MonoBehaviour
{
    private readonly List<Vector2Int> _cellNeighborOffsets = new List<Vector2Int>
    {
        new(-1, -1),
        new(-1, 0),
        new(-1, 1),
        new(0, -1),
        new(0, 1),
        new(1, -1),
        new(1, 0),
        new(1, 1),
    };

    private enum MineSweeperCellContents
    {
        Empty,
        Number,
        Mine,
    }

    [Header("Asset References")]
    [SerializeField]
    private MineSweeperEventsSO _mineSweeperEvents;
    [SerializeField]
    private GameObject _gridCellPrefab;
    [SerializeField]
    private GameObject _gridBackgroundPrefab;

    [Header("Placement Parameters")]
    [SerializeField]
    private float _cameraVerticalOffset;
    [SerializeField]
    private float _additionalCameraOrthographicSize;
    [SerializeField]
    private float _gridBackgroundAdditionalSize;

    private Grid2D<MineSweeperCellContents> _gameBoard;
    private MineSweeperCellVisual[,] _boardVisuals;
    private GameObject _gridBackgroundVisual;
    private bool _boardIsPopulated;
    private bool _gameInProgress;
    private int _gridWidth;
    private int _gridHeight;
    private int _mineCount;
    private int _flagsLeft;

    private void OnDisable()
    {
        _mineSweeperEvents.OnStartGame -= InitiallizeGameAndBoard;
        _mineSweeperEvents.OnPlayAgain -= ResetBoard;
        _mineSweeperEvents.OnReturnToSetup -= ClearBoard;
    }

    private void OnEnable()
    {
        _mineSweeperEvents.OnStartGame += InitiallizeGameAndBoard;
        _mineSweeperEvents.OnPlayAgain += ResetBoard;
        _mineSweeperEvents.OnReturnToSetup += ClearBoard;
    }

    void Update()
    {
        if (!_gameInProgress) return;

        HandlePlayerInput();
    }

    /// <summary>
    /// Handles the game's initiallization:<br/><br/>
    /// 1. Sets the board parameters.<br/>
    /// 2. Initiallizes the underlying grid.<br/>
    /// 3. Spawns the grid cells.<br/>
    /// 4. Sets the camera based on the grid's size.<br/>
    /// 5. Place grid background and resize.
    /// </summary>
    private void InitiallizeGameAndBoard()
    {
        // Set Board Parameters.
        _gridWidth = MineSweeperManager.Instance.LevelGridWidth;
        _gridHeight = MineSweeperManager.Instance.LevelGridHeight;
        _mineCount = MineSweeperManager.Instance.LevelMineCount;
        _flagsLeft = _mineCount;
        _boardIsPopulated = false;
        _gameInProgress = true;

        // Initialize Game Board.
        _gameBoard = new Grid2D<MineSweeperCellContents>(_gridWidth, _gridHeight, Vector3.zero);

        // Spawn Board Cells.
        _boardVisuals = new MineSweeperCellVisual[_gridWidth, _gridHeight];
        Vector3 spawnOffset = new Vector3(0.5f, 0.5f, 0);
        for (int x = 0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                // Spawn cell.
                _boardVisuals[x, y] = Instantiate(_gridCellPrefab, spawnOffset.Add(x: x, y: y), Quaternion.identity, gameObject.transform).GetComponent<MineSweeperCellVisual>();
                // Ensure the cell's values are set to their proper default values.
                _boardVisuals[x, y].ResetCell();
            }
        }

        // Move the camera into place and change it's size.
        Camera.main.transform.position = _gameBoard.GetGridCameraPosition().Add(y: _cameraVerticalOffset);
        Camera.main.orthographicSize = _gameBoard.GetCameraOrthographicSize(_additionalCameraOrthographicSize);

        // Place the grid background and change it's size.
        _gridBackgroundVisual = Instantiate(_gridBackgroundPrefab, _gameBoard.GetGridCenterWorldPosition(1), Quaternion.identity);
        _gridBackgroundVisual.transform.localScale = new Vector3(_gridWidth + _gridBackgroundAdditionalSize, _gridHeight + _gridBackgroundAdditionalSize, 1);

        // Enable and set the gameplay UI.
        MineSweeperGameplayUI.Instance.ShowGameplayUI();
        MineSweeperGameplayUI.Instance.SetFlagsLeftText(_flagsLeft);
    }

    /// <summary>
    /// Handles board data population:<br/><br/>
    /// 1. Places mines.<br/>
    /// 2. Calculates number values.
    /// </summary>
    /// <param name="invalidMinePosition"></param>
    private void PopulateBoard(Vector2Int invalidMinePosition)
    {
        _boardIsPopulated = true;

        // Mine Placement.
        PlaceMines(invalidMinePosition);

        // Cell Number Calculation.
        AssignCellNumbers();
    }

    private void PlaceMines(Vector2Int invalidMinePosition)
    {
        List<Vector2Int> invalidMinePositions = new() { invalidMinePosition };
        for (int i = 0; i < _cellNeighborOffsets.Count; i++)
        {
            invalidMinePositions.Add(invalidMinePosition + _cellNeighborOffsets[i]);
        }

        int minesPlaced = 0;
        while (minesPlaced < _mineCount)
        {
            // Generate random board position to place a mine in.
            Vector2Int minePosition = _gameBoard.GetRandomCell();

            // Ensure we do not place a mine where the player has made their first click, or in the 8 neighboring cells.
            // This ensures the player's first click will be an empty cell and lead to a mass-reveal.
            if (invalidMinePositions.Contains(minePosition)) continue;
            // Ensure we do not place more than 1 mine in the same position.
            else if (_gameBoard.GetGridObject(minePosition) == MineSweeperCellContents.Mine) continue;
            // Place the mine.
            else
            {
                _gameBoard.SetGridObject(minePosition, MineSweeperCellContents.Mine);
                _boardVisuals[minePosition.x, minePosition.y].SetMine();
                minesPlaced++;
            }
        }
    }

    private void AssignCellNumbers()
    {
        int mineNeighborCount;

        // Cycle through the board's cells.
        for (int x = 0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                if (_gameBoard.GetGridObject(x, y) != MineSweeperCellContents.Mine)
                {
                    mineNeighborCount = 0;

                    // Calculate the number of mine neighbors for the current cell.
                    for (int i = 0; i < _cellNeighborOffsets.Count; i++)
                    {
                        Vector2Int neighborPosition = new(x + _cellNeighborOffsets[i].x, y + _cellNeighborOffsets[i].y);
                        if (_gameBoard.IsValidGridPosition(neighborPosition) &&
                        _gameBoard.GetGridObject(neighborPosition) == MineSweeperCellContents.Mine) mineNeighborCount++;
                    }

                    // Set number-related values.
                    _boardVisuals[x, y].SetNumber(mineNeighborCount);
                    if (mineNeighborCount > 0) _gameBoard.SetGridObject(x, y, MineSweeperCellContents.Number);
                }
            }
        }
    }

    private void HandlePlayerInput()
    {
        // Track mouse position.
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int boardClickPosition = _gameBoard.GetVectorInts(mousePosition);

        // Left Click Input - the left click is used to select a cell to reveal / reveal all the neighbors of an already clicked cell.
        if (Input.GetMouseButtonDown(0))
        {
            // Ensure the player clicked on the board and not outside of it.
            if (_gameBoard.IsValidGridPosition(boardClickPosition))
            {
                // We clicked an already revealed cell - reveal all it's neighbor cells.
                if (_boardVisuals[boardClickPosition.x, boardClickPosition.y].IsRevealed)
                {
                    // Reveals the clicked position's neighboring cells.
                    // If they are marked incorrectly, the game will be lost.
                    if (!RevealNeighborCells(boardClickPosition)) GameComplete(gameWon: false);

                    CheckIfGameWon();
                }
                // We clicked an unrevealed cell - we reveal it.
                else
                {
                    // Check if the board has already been populated, if not - populates it.
                    // This is to ensure players cannot click a mine when they first click.
                    if (!_boardIsPopulated) PopulateBoard(boardClickPosition);

                    switch (_gameBoard.GetGridObject(boardClickPosition))
                    {
                        // Empty cell clicked.
                        default:
                            EmptyClickSweepingReveal(boardClickPosition);
                            break;
                        
                        // Number cell clicked.
                        case MineSweeperCellContents.Number:
                            _boardVisuals[boardClickPosition.x, boardClickPosition.y].RevealCell();
                            break;
                        
                        // Mine cell clicked.
                        case MineSweeperCellContents.Mine:
                            _boardVisuals[boardClickPosition.x, boardClickPosition.y].RevealCell(markMineBackground: true);
                            GameComplete(gameWon: false);
                            break;
                    }

                    // Checks if the player has revealed all non-mine cells.
                    CheckIfGameWon();
                }
            }
        }

        // Right Click Input - the right click is used to mark a cell with a flag.
        if (Input.GetMouseButtonDown(1))
        {
            // Ensure the player clicked on the board and not outside of it.
            if (_gameBoard.IsValidGridPosition(boardClickPosition))
            {
                // Ensure we do not unnecessarily perform actions in response to an already-revealed cell.
                if (_boardVisuals[boardClickPosition.x, boardClickPosition.y].IsRevealed) return;

                // Tracks how many flags are left to be placed, increses/decreases based on the currnet vacell value - before flipping it.
                if (_boardVisuals[boardClickPosition.x, boardClickPosition.y].IsFlagged) _flagsLeft++;
                else _flagsLeft--;
                // Either places or removes a flag mark on the clicked cell.
                _boardVisuals[boardClickPosition.x, boardClickPosition.y].FlipFlaggedState();

                MineSweeperGameplayUI.Instance.SetFlagsLeftText(_flagsLeft);
            }
        }
    }

    private bool RevealNeighborCells(Vector2Int cellPosition)
    {
        bool validReveal = false;
        int correctlyMarkedMines = 0;

        for (int i = 0; i < _cellNeighborOffsets.Count; i++)
        {
            Vector2Int neighborPosition = cellPosition + _cellNeighborOffsets[i];

            if (_gameBoard.IsValidGridPosition(neighborPosition))
            {
                if (!_boardVisuals[neighborPosition.x, neighborPosition.y].IsFlagged)
                {
                    _boardVisuals[neighborPosition.x, neighborPosition.y].RevealCell();
                }

                if (_gameBoard.GetGridObject(neighborPosition) == MineSweeperCellContents.Mine
                && _boardVisuals[neighborPosition.x, neighborPosition.y].IsFlagged)
                {
                    correctlyMarkedMines++;
                }
            }
        }

        if (correctlyMarkedMines == _boardVisuals[cellPosition.x, cellPosition.y].Number) validReveal = true;

        return validReveal;
    }

    private void EmptyClickSweepingReveal(Vector2Int cellPosition)
    {
        Queue<Vector2Int> emptyCellsToHandle = new(); // The list of empty cells to check.
        List<Vector2Int> cellsToReveal = new(); // The list of cells to reveal.
        List<Vector2Int> alreadyCheckedcells = new(); // Empty cells that were already checked.

        // Add the initial empty cell to the lists.
        emptyCellsToHandle.Enqueue(cellPosition);
        cellsToReveal.Add(cellPosition);

        // Pass over all empty cells.
        while (emptyCellsToHandle.Count > 0)
        {
            // Take out the current empty cell.
            Vector2Int currentCell = emptyCellsToHandle.Dequeue();
            
            // Cycle through all the cell's neighbors.
            for (int i = 0; i < _cellNeighborOffsets.Count; i++)
            {
                Vector2Int neighborPosition = currentCell + _cellNeighborOffsets[i];

                // Ensure the neighbor position is a valid position on the grid.
                if (_gameBoard.IsValidGridPosition(neighborPosition))
                {
                    // Add the current neighbor to the reveal list while ensuring no duplicates.
                    if (!cellsToReveal.Contains(neighborPosition)) cellsToReveal.Add(neighborPosition);
                    
                    // Checks if the current neighbor is empty, if so - checks if it was already handled and adds it to the list to handle if it wasn't.
                    if (_gameBoard.GetGridObject(neighborPosition) == MineSweeperCellContents.Empty &&
                    !alreadyCheckedcells.Contains(neighborPosition) &&
                    !emptyCellsToHandle.Contains(neighborPosition))
                    {
                        emptyCellsToHandle.Enqueue(neighborPosition);
                    }
                }
            }
            
            // Add it to the cells that were already checked.
            alreadyCheckedcells.Add(currentCell);
        }

        // reveal the entire list of cells to reveal
        for (int i = 0; i < cellsToReveal.Count; i++)
        {
            _boardVisuals[cellsToReveal[i].x, cellsToReveal[i].y].RevealCell();
        }
    }

    private void CheckIfGameWon()
    {
        if (!_gameInProgress) return;

        int cellsRemaining = 0;

        for (int x = 0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                if (!_boardVisuals[x, y].IsRevealed) cellsRemaining++;
            }
        }

        if (cellsRemaining == _mineCount) GameComplete(gameWon: true);
    }

    private void GameComplete(bool gameWon = true)
    {
        _gameInProgress = false;

        RevealAllCells();

        // Hide gameplay UI.
        MineSweeperGameplayUI.Instance.HideGameplayUI();

        _mineSweeperEvents.TriggerGameFinished(gameWon);
    }

    private void RevealAllCells()
    {
        for (int x = 0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                if (!_boardVisuals[x, y].IsRevealed) _boardVisuals[x, y].RevealCell();
            }
        }
    }

    private void ResetBoard()
    {
        ClearBoard();

        InitiallizeGameAndBoard();
    }

    private void ClearBoard()
    {
        // Destroy board visuals.
        gameObject.transform.DestroyAllChildren();
        Destroy(_gridBackgroundVisual);

        // Clear grid references.
        _gameBoard = null;
        _boardVisuals = null;
    }


}
