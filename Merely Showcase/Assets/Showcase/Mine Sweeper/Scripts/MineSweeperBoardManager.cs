using UnityEngine;
using System.Collections.Generic;
using RightNowGames.Grids;
using RightNowGames.Utilities;

/// <summary>
/// Responsible for managing the game board - the entire logic where the puzzle is located.
/// </summary>
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
        // Event Subscriptions:
        // Initiallize game board when the "game start" event is called.
        _mineSweeperEvents.OnStartGame += InitiallizeGameAndBoard;

        // Reset the board when the "play again" event is called.
        _mineSweeperEvents.OnPlayAgain += ResetBoard;
        
        // Clear the board when the "return to setup" event is called.
        _mineSweeperEvents.OnReturnToSetup += ClearBoard;
    }

    void Update()
    {
        // Make sure the update loop does not run when the minesweeper game is not in progress.
        if (!_gameInProgress) return;

        HandlePlayerInput();
    }

    /// <summary>
    /// Handles the game's initiallization:<br/><br/>
    /// 1. Sets the board parameters.<br/>
    /// 2. Initiallizes the underlying grid.<br/>
    /// 3. Spawns the grid cells.<br/>
    /// 4. Sets the camera based on the grid's size.<br/>
    /// 5. Place grid background and resize.<br/>
    /// 6. Initializes gameplay UI.
    /// </summary>
    private void InitiallizeGameAndBoard()
    {
        // 1. Set Board Parameters.
        _gridWidth = MineSweeperManager.Instance.LevelGridWidth;
        _gridHeight = MineSweeperManager.Instance.LevelGridHeight;
        _mineCount = MineSweeperManager.Instance.LevelMineCount;
        _flagsLeft = _mineCount;
        _boardIsPopulated = false;
        _gameInProgress = true;

        // 2. Initialize Game Board.
        _gameBoard = new Grid2D<MineSweeperCellContents>(_gridWidth, _gridHeight, Vector3.zero);

        // 3. Spawn Board Cells.
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

        // 4. Move the camera into place and change it's size.
        Camera.main.transform.position = _gameBoard.GetGridCameraPosition().Add(y: _cameraVerticalOffset);
        Camera.main.orthographicSize = _gameBoard.GetCameraOrthographicSize(_additionalCameraOrthographicSize);

        // 5. Place the grid background and change it's size.
        _gridBackgroundVisual = Instantiate(_gridBackgroundPrefab, _gameBoard.GetGridCenterWorldPosition(1), Quaternion.identity);
        _gridBackgroundVisual.transform.localScale = new Vector3(_gridWidth + _gridBackgroundAdditionalSize, _gridHeight + _gridBackgroundAdditionalSize, 1);

        // 6. Enable and set the gameplay UI.
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
        // Mine Placement.
        PlaceMines(invalidMinePosition);

        // Cell Number Calculation.
        AssignCellNumbers();

        _boardIsPopulated = true;
    }

    /// <summary>
    /// Randomly selects board positions to place mines in and sets the underlying logic.
    /// </summary>
    /// <param name="invalidMinePosition"></param>
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

    /// <summary>
    /// Calculates which cells contain numbers and sets the underlying logic.
    /// </summary>
    private void AssignCellNumbers()
    {
        int mineNeighborCount;

        // Cycle through the board's cells.
        for (int x = 0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                // Mine cells cannot be numbered cells, so first we ensure the current cell we are working on is not a mine.
                if (_gameBoard.GetGridObject(x, y) != MineSweeperCellContents.Mine)
                {
                    mineNeighborCount = 0;

                    // Calculate the number of mine neighbors for the current cell.
                    for (int i = 0; i < _cellNeighborOffsets.Count; i++)
                    {
                        // Current neighbor position.
                        Vector2Int neighborPosition = new(x + _cellNeighborOffsets[i].x, y + _cellNeighborOffsets[i].y);
                        // Check if the position is valid & contains a mine, if so - increase count.
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

    /// <summary>
    /// Player Input Handler.<br/><br/>
    /// 1. Left-Click -> cell reveal related logic.<br/>
    /// 2. Right-Click -> flag marker placement logic.
    /// </summary>
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

    /// <summary>
    /// Handles the logic to reveal all neighbors of an already-revealed number cell that was clicked again.
    /// </summary>
    /// <param name="cellPosition"></param>
    /// <returns></returns>
    private bool RevealNeighborCells(Vector2Int cellPosition)
    {
        bool validNeighborReveal = false;
        int correctlyMarkedMines = 0;
        List<Vector2Int> neighborCellsToReveal = new();
        List<Vector2Int> emptyNeighborCells = new();

        // Pass over all neighbor positions.
        for (int i = 0; i < _cellNeighborOffsets.Count; i++)
        {
            Vector2Int neighborPosition = cellPosition + _cellNeighborOffsets[i];

            // Ensure the calculated neighbor position is a valid grid position.
            if (_gameBoard.IsValidGridPosition(neighborPosition))
            {
                // Check if the current position is empty, to be able to follow it through to an empty cell mass reveal.
                if (_gameBoard.GetGridObject(neighborPosition) == MineSweeperCellContents.Empty)
                {
                    emptyNeighborCells.Add(neighborPosition);
                }

                // Ensure the current position is not flagged - we do not reveal flagged cells.
                if (!_boardVisuals[neighborPosition.x, neighborPosition.y].IsFlagged)
                {
                    neighborCellsToReveal.Add(new Vector2Int(neighborPosition.x, neighborPosition.y));
                }
                // If the current position is flagged, we check if it is a mine.
                // If the position is BOTH flagged AND a mine - it counts toward the click that resulted in calling this method being a valid action.
                else if (_gameBoard.GetGridObject(neighborPosition) == MineSweeperCellContents.Mine)
                {
                    correctlyMarkedMines++;
                }
            }
        }

        // Check if the tracker for correctly flagged mine neighbors equals to the number value of the clicked cell.
        // If it is - this reveal action was valid.
        if (correctlyMarkedMines == _boardVisuals[cellPosition.x, cellPosition.y].Number) validNeighborReveal = true;

        // If the reveal action is valid & some of the neighbors are empty - we use these empty cells to create mass reveal calls.
        if (validNeighborReveal)
        {
            if (emptyNeighborCells.Count > 0)
            {
                // Remove empty cells from the neighbor reveal list (no need to reveal these multiple times).
                neighborCellsToReveal.RemoveWholeList(emptyNeighborCells);

                // Reveal neighbors.
                if (neighborCellsToReveal.Count > 0)
                {
                    for (int i = 0; i < neighborCellsToReveal.Count; i++)
                    {
                        _boardVisuals[neighborCellsToReveal[i].x, neighborCellsToReveal[i].y].RevealCell();
                    }
                }

                // Reveal empty cells using the sweeping reveal logic.
                for (int i = 0; i < emptyNeighborCells.Count; i++)
                {
                    EmptyClickSweepingReveal(emptyNeighborCells[i]);
                }
            }
            else
            {
                // Reveal neighbors.
                for (int i = 0; i < neighborCellsToReveal.Count; i++)
                {
                    _boardVisuals[neighborCellsToReveal[i].x, neighborCellsToReveal[i].y].RevealCell();
                }
            }
        }

        // Return bool result reagrding reveal action validity.
        return validNeighborReveal;
    }

    /// <summary>
    /// Reveals the empty cell that was clicked and all it's neighbor cells.<br/>
    /// if it has empty neighbors - also reveals all their neighbors, and so on...
    /// </summary>
    /// <param name="cellPosition"></param>
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

    /// <summary>
    /// Checkers method to see if the game has been won.
    /// </summary>
    private void CheckIfGameWon()
    {
        // Ensure we do not perform this check if the game is not in progress.
        if (!_gameInProgress) return;

        // Calculate how many cells are left to reveal.
        int cellsRemaining = 0;
        for (int x = 0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                if (!_boardVisuals[x, y].IsRevealed) cellsRemaining++;
            }
        }

        // If the number of cells left to reveal equals the number of mines the game is won.
        if (cellsRemaining == _mineCount) GameComplete(gameWon: true);
    }

    /// <summary>
    /// Handles the game's completion (Win/Lose).
    /// </summary>
    /// <param name="gameWon"></param>
    private void GameComplete(bool gameWon = true)
    {
        // Track game finished, to disable Update loop.
        _gameInProgress = false;

        // Reveal all still-hidden cells.
        RevealAllCells();

        // Hide gameplay UI.
        MineSweeperGameplayUI.Instance.HideGameplayUI();

        // Trigger game finished event.
        _mineSweeperEvents.TriggerGameFinished(gameWon);
    }

    /// <summary>
    /// Reveals all cells on the board.
    /// </summary>
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

    /// <summary>
    /// Resets the board.<br/><br/>
    /// 1. Destroys all board visuals.
    /// 2. Respawns a new board, all values reset.
    /// </summary>
    private void ResetBoard()
    {
        // Destroy current game board.
        ClearBoard();

        // Spawn a new game board.
        InitiallizeGameAndBoard();
    }

    /// <summary>
    /// Destroys the current game board and resets underlying logic references.
    /// </summary>
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
