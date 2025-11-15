using System;
using UnityEngine;
using RightNowGames.Enums;

namespace RightNowGames.Grids
{
	/// <summary>
	/// Used to place and track a 2D grid in the game's world.<br/>
	/// Can be placed vertically, horizontally or along the Z axis.<br/><br/>
	/// Can act as the basis for a wide variety of systems.<br/>
	/// Game Examples: Match3, MineSweeper, Sudoku, etc.<br/>
	/// System Examples: Level Design system, Navigation system, Building system, Inventory system, Heat Map, etc.
	/// </summary>
	/// <typeparam name="TGridObject">The object/type stored in each grid cell.</typeparam>
    public class Grid2D<TGridObject>
    {
		#region General Notes

		// Constructor Func syntax examples:
		// Examples using a class named GridCell, requiring 2 ints to create, with optional reference to the grid.
		//// When creating instances of GridCell class (without reference to the grid): (int x, int y) => new GridCell(x, y).
		//// When creating instances of GridCell class (with reference to the grid): (RNGGrid2D<GridCell> g, int x, int y) => new GridCell(g, x, y).

		#endregion

		#region Variables

		public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
		public class OnGridObjectChangedEventArgs : EventArgs
		{
			public int GridWidth;
			public int GridHeight;
			public Vector2Int Position => new Vector2Int(GridWidth, GridHeight);
		}

		private int _gridWidth;
		private int _gridHeight;
		private float _cellSize;
		private float _cellSpacing;
		private Vector3 _originPosition;
		private GridLayoutType2D _gridLayoutType;
		private TGridObject[,] _gridArray;

		#endregion

		#region Getters

		public int GridWidth => _gridWidth;
		public int GridHeight => _gridHeight;
		public float CellSize => _cellSize;
		public float CellSpacing => _cellSpacing;
		public Vector3 OriginPosition => _originPosition;
		public GridLayoutType2D GridLayoutType => _gridLayoutType;

		#endregion

		#region Constructors

		/// <summary>
		/// RNGGrid2D constructor.<br/>
		/// Basic constructor, meant for grids of bools/ints/floats/other basic types.
		/// </summary>
		/// <param name="gridWidth">The grid's width value -> how many columns will it have?</param>
		/// <param name="gridHeight">The grid's height value -> how many rows will it have?</param>
		/// <param name="originPosition">The grid's origin position, will be layed out from this point -> corresponds to the bottom-left corner of the grid.</param>
		/// <param name="cellSize">The grid's individual cell size.</param>
		/// <param name="cellSpacing">The grid's cell spacing, determines the distance between cells -> 0 means no spacing.</param>
		/// <param name="gridLayoutType">How will the grid be layed out?</param>
		/// <param name="showDebug">Should there be a visual debug representing the grid?</param>
		public Grid2D(int gridWidth, int gridHeight, Vector3 originPosition, float cellSize = 1f, float cellSpacing = 0, GridLayoutType2D gridLayoutType = default, bool showDebug = false)
		{
			// Set grid variables.
			_gridWidth = gridWidth;
			_gridHeight = gridHeight;
			_cellSize = cellSize;
			_originPosition = originPosition;
			_gridLayoutType = gridLayoutType;
			_cellSpacing = cellSpacing;

			// Initialize array.
			_gridArray = new TGridObject[gridWidth, gridHeight];

			// Cycle through and initialize each grid cell (TGridObject).
			for (int width = 0; width < gridWidth; width++)
			{
				for (int height = 0; height < gridHeight; height++)
				{
					_gridArray[width, height] = default;
				}
			}

			// Check if required to visualize the grid through debug drawn lines.
			// If so, draw.
			if (showDebug) DrawLineDebug();
		}

		/// <summary>
		/// RNGGrid2D constructor.<br/>
		/// Complex constructor, meant for grids of complex types (mainly custom classes).<br/>
		/// Internally generates TGridObject instance for each cell, without reference to the grid itself.
		/// </summary>
		/// <param name="gridWidth">The grid's width value -> how many columns will it have?</param>
		/// <param name="gridHeight">The grid's height value -> how many rows will it have?</param>
		/// <param name="originPosition">The grid's origin position, will be layed out from this point -> corresponds to the bottom-left corner of the grid.</param>
		/// <param name="CreateGridObject">The default setup of each grid cell's contents.</param>
		/// <param name="cellSize">The grid's individual cell size.</param>
		/// <param name="cellSpacing">The grid's cell spacing, determines the distance between cells -> 0 means no spacing.</param>
		/// <param name="gridLayoutType">How will the grid be layed out?</param>
		/// <param name="showDebug">Should there be a visual debug representing the grid?</param>
		public Grid2D(int gridWidth, int gridHeight, Vector3 originPosition, Func<int, int, TGridObject> CreateGridObject, float cellSize = 1f, float cellSpacing = 0, GridLayoutType2D gridLayoutType = default, bool showDebug = false)
		{
			// Set grid variables.
			_gridWidth = gridWidth;
			_gridHeight = gridHeight;
			_cellSize = cellSize;
			_originPosition = originPosition;
			_gridLayoutType = gridLayoutType;
			_cellSpacing = cellSpacing;

			// Initialize array.
			_gridArray = new TGridObject[gridWidth, gridHeight];

			// Cycle through and initialize each grid cell (TGridObject).
			for (int width = 0; width < gridWidth; width++)
			{
				for (int height = 0; height < gridHeight; height++)
				{
					_gridArray[width, height] = CreateGridObject(width, height);
				}
			}

			// Check if required to visualize the grid through debug drawn lines.
			// If so, draw.
			if (showDebug) DrawLineDebug();
		}

		/// <summary>
		/// RNGGrid2D constructor.<br/>
		/// Complex constructor, meant for grids of complex types (mainly custom classes).<br/>
		/// Internally generates TGridObject instance for each cell, with reference to the grid itself.
		/// </summary>
		/// <param name="gridWidth">The grid's width value -> how many columns will it have?</param>
		/// <param name="gridHeight">The grid's height value -> how many rows will it have?</param>
		/// <param name="originPosition">The grid's origin position, will be layed out from this point -> corresponds to the bottom-left corner of the grid.</param>
		/// <param name="CreateGridObject">The default setup of each grid cell's contents.</param>
		/// <param name="cellSize">The grid's individual cell size.</param>
		/// <param name="cellSpacing">The grid's cell spacing, determines the distance between cells -> 0 means no spacing.</param>
		/// <param name="gridLayoutType">How will the grid be layed out?</param>
		/// <param name="showDebug">Should there be a visual debug representing the grid?</param>
		public Grid2D(int gridWidth, int gridHeight, Vector3 originPosition, Func<Grid2D<TGridObject>, int, int, TGridObject> CreateGridObject, float cellSize = 1f, float cellSpacing = 0, GridLayoutType2D gridLayoutType = default, bool showDebug = false)
		{
			// Set grid variables.
			_gridWidth = gridWidth;
			_gridHeight = gridHeight;
			_cellSize = cellSize;
			_originPosition = originPosition;
			_gridLayoutType = gridLayoutType;
			_cellSpacing = cellSpacing;

			// Initialize array.
			_gridArray = new TGridObject[gridWidth, gridHeight];

			// Cycle through and initialize each grid cell (TGridObject).
			for (int width = 0; width < gridWidth; width++)
			{
				for (int height = 0; height < gridHeight; height++)
				{
					_gridArray[width, height] = CreateGridObject(this, width, height);
				}
			}

			// Check if required to visualize the grid through debug drawn lines.
			// If so, draw.
			if (showDebug) DrawLineDebug();
		}

		#endregion

		#region Methods

		#region Internal Methods

		/// <summary>
		/// Returns a Vector3 with the signature depthValue applied to the "depth" axis.<br/>
		/// Different grid types used different axis for their "depth".<br/>
		/// Vertical grid - Z axis, Horizontal grid - Y axis, Vertical Depth grid - X axis.
		/// </summary>
		/// <param name="returnedDepthValue">How much "depth" to return.</param>
		/// <returns></returns>
		private Vector3 AdditionalDepthValue(float returnedDepthValue)
		{
			return _gridLayoutType switch
			{
				// Horizontal grid type, X axis for width & Z axis for height.
				GridLayoutType2D.Horizontal => new Vector3(0, returnedDepthValue, 0),
				// Vertical Depth grid type, Z axis for width & Y axis for height.
				GridLayoutType2D.VerticalDepth => new Vector3(returnedDepthValue, 0, 0),
				// Vertical grid type, X axis for width & Y axis for height.
				_ => new Vector3(0, 0, returnedDepthValue),
			};
		}

		/// <summary>
		/// Draws debug lines to visualize the newly initialized grid.
		/// </summary>
		/// <param name="drawnLineDuration">For how long should the line be visible?</param>
		/// <param name="drawnLineColor">What color should the line be?</param>
		private void DrawLineDebug(float drawnLineDuration = 100f, Color drawnLineColor = default)
		{
			// If there is spacing between grid cells, draw additional lines to visualize these spaces.
			if (_cellSpacing > 0)
			{
				// Width lines.
				for (int width = 0; width < _gridWidth; width++)
				{
					Debug.DrawLine(GetWorldPosition(width, 0), GetTopLeftWorldPosition(width, _gridHeight - 1), drawnLineColor, drawnLineDuration);
					Debug.DrawLine(GetBottomRightWorldPosition(width, 0), GetTopRightWorldPosition(width, _gridHeight - 1), drawnLineColor, drawnLineDuration);
				}

				// Height lines.
				for (int height = 0; height < _gridHeight; height++)
				{
					Debug.DrawLine(GetWorldPosition(0, height), GetBottomRightWorldPosition(_gridWidth - 1, height), drawnLineColor, drawnLineDuration);
					Debug.DrawLine(GetTopLeftWorldPosition(0, height), GetTopRightWorldPosition(_gridWidth - 1, height), drawnLineColor, drawnLineDuration);
				}
			}
			// Else, draw the lines for the grid without spacing.
			else
			{
				// Width lines.
				for (int width = 0; width < _gridWidth; width++)
				{
					Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, _gridHeight), drawnLineColor, drawnLineDuration);
				}

				// Height lines.
				for (int height = 0; height < _gridHeight; height++)
				{
					Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(_gridWidth, height), drawnLineColor, drawnLineDuration);
				}

				// Remaining lines to close the final cells in each direction (right-most height line & up-most width line).
				Debug.DrawLine(GetWorldPosition(0, _gridHeight), GetWorldPosition(_gridWidth, _gridHeight), drawnLineColor, drawnLineDuration);
				Debug.DrawLine(GetWorldPosition(_gridWidth, 0), GetWorldPosition(_gridWidth, _gridHeight), drawnLineColor, drawnLineDuration);
			}
		}

		#endregion

		#region Simple Getter Methods

		/// <summary>
		/// Returns the width of the grid.<br/>
		/// Either X or Z value, depending on the grid's layout type.
		/// </summary>
		/// <returns></returns>
		public int GetGridWidth() { return _gridWidth; }
		/// <summary>
		/// Returns the height of the grid.<br/>
		/// Either Y or Z value, depending on the grid's layout type.
		/// </summary>
		/// <returns></returns>
		public int GetGridHeight() { return _gridHeight; }
		/// <summary>
		/// Returns the cellSize of the grid.
		/// </summary>
		/// <returns></returns>
		public float GetCellSize() { return _cellSize; }
		/// <summary>
		/// Returns the cellSpacing of the grid.
		/// </summary>
		/// <returns></returns>
		public float GetCellSpacing() { return _cellSpacing; }
		/// <summary>
		/// Returns the originPosition of the grid (0,0 point).<br/>
		/// Corresponds to the bottom left corner of the grid.
		/// </summary>
		/// <returns></returns>
		public Vector3 GetOriginPosition() { return _originPosition; }
		/// <summary>
		/// Returns the layout type of the grid.
		/// </summary>
		/// <returns></returns>
		public GridLayoutType2D GetGridLayout() { return _gridLayoutType; }

		#endregion

		#region Position Validation

		/// <summary>
		/// Returns true if the signature values correspond to a valid position on the grid.
		/// </summary>
		/// <param name="gridWidth">The column to be checked (-1).</param>
		/// <param name="gridHeight">The row to be checked (-1).</param>
		/// <returns></returns>
		public bool IsValidGridPosition(int gridWidth, int gridHeight)
		{
			// Checks if the axis values are within the grid's limits.
			// If so return true, else return false.
			return (gridWidth >= 0 && gridWidth < _gridWidth && gridHeight >= 0 && gridHeight < _gridHeight);

			/*if (axis1 >= 0 && axis1 < _gridWidth && axis2 >= 0 && axis2 < _gridHeight) return true;
			return false;*/
		}

		/// <summary>
		/// Returns true if the signature value corresponds to a valid position on the grid.
		/// </summary>
		/// <param name="position">The position to be checked.</param>
		/// <returns></returns>
		public bool IsValidGridPosition(Vector2Int position)
		{
			// Calls another validator and returns it's result.
			return IsValidGridPosition(position.x, position.y);
		}

		/// <summary>
		/// Returns true if the signature worldPosition corresponds to a valid position on the grid.
		/// </summary>
		/// <param name="worldPosition">The worldPosition to be converted, then checked.</param>
		/// <returns></returns>
		public bool IsValidGridPosition(Vector3 worldPosition)
		{
			// Converts world position to ints.
			int axis1, axis2;
			GetInts(worldPosition, out axis1, out axis2);
			// calls another validator and returns it's result.
			return IsValidGridPosition(axis1, axis2);
		}

		#endregion

		#region Get Cell Indexes

		/// <summary>
		/// Outputs the int values of the gridCell corresponding to the signature worldPosition.
		/// </summary>
		/// <param name="worldPosition">The worldPosition to be converted.</param>
		/// <param name="gridWidth">Width value output.</param>
		/// <param name="gridHeight">Height value output.</param>
		public void GetInts(Vector3 worldPosition, out int gridWidth, out int gridHeight)
		{
			// Floors worldPosition axis values to return the int indexes for the corresponding gridCell.
			switch (_gridLayoutType)
			{
				// Vertical (default) grid layout, X axis for width & Y axis for height.
				default:
				case GridLayoutType2D.Vertical:

					if (_cellSpacing > 0)
					{
						// Width:
						gridWidth = Mathf.FloorToInt((worldPosition - _originPosition).x / (_cellSize + _cellSpacing));
						// Height:
						gridHeight = Mathf.FloorToInt((worldPosition - _originPosition).y / (_cellSize + _cellSpacing));
					}
					else
					{
						// Width:
						gridWidth = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
						// Height:
						gridHeight = Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize);
					}

					break;

				// Horizontal grid layout, X axis for width & Z axis for height.
				case GridLayoutType2D.Horizontal:

					if (_cellSpacing > 0)
					{
						// Width:
						gridWidth = Mathf.FloorToInt((worldPosition - _originPosition).x / (_cellSize + _cellSpacing));
						// Height:
						gridHeight = Mathf.FloorToInt((worldPosition - _originPosition).z / (_cellSize + _cellSpacing));
					}
					else
					{
						// Width:
						gridWidth = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
						// Height:
						gridHeight = Mathf.FloorToInt((worldPosition - _originPosition).z / _cellSize);
					}

					break;

				// Depth grid layout, Z axis for width & Y axis for height.
				case GridLayoutType2D.VerticalDepth:

					if (_cellSpacing > 0)
					{
						// Width:
						gridWidth = Mathf.FloorToInt((worldPosition - _originPosition).z / (_cellSize + _cellSpacing));
						// Height:
						gridHeight = Mathf.FloorToInt((worldPosition - _originPosition).y / (_cellSize + _cellSpacing));
					}
					else
					{
						// Width:
						gridWidth = Mathf.FloorToInt((worldPosition - _originPosition).z / _cellSize);
						// Height:
						gridHeight = Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize);
					}

					break;
			}
		}

		/// <summary>
		/// Returns the Vector2Int value of the gridCell corresponding to the signature worldPosition.
		/// </summary>
		/// <param name="worldPosition">The worldPosition to be converted.</param>
		/// <returns></returns>
		public Vector2Int GetVectorInts(Vector3 worldPosition)
		{
			// Gathers the int indexes for the gridCell corresponding to the worldPosition, calculated by GetInts().
			int axis1, axis2;
			GetInts(worldPosition, out axis1, out axis2);
			// Returns them as a Vector2Int instance.
			return new Vector2Int(axis1, axis2);
		}

		/// <summary>
        /// Returns the int position values of a random cell on the board.
        /// </summary>
        /// <returns></returns>
		public Vector2Int GetRandomCell()
        {
            return new Vector2Int(UnityEngine.Random.Range(0, _gridWidth), UnityEngine.Random.Range(0, _gridHeight));
        }

		#endregion

		#region Get World Position Methods

		#region Corners

		#region World Position - Bottom Left Corner

		// Returns world positions, corresponding to the bottom-left corner of a gridCell.
		/// <summary>
		/// Returns the worldPosition of the gridCell corresponding to the signature values.<br/>
		/// The returned position represents the bottom left corner of the gridCell.<br/>
		/// Not limited to valid grid positions, will return for any set of values based on the grid's calculation formula.
		/// </summary>
		/// <param name="gridWidth"></param>
		/// <param name="gridHeight"></param>
		/// <returns></returns>
		public Vector3 GetWorldPosition(int gridWidth, int gridHeight, float returnedDepthValue = 0)
		{
			Vector3 position;
			switch (_gridLayoutType)
			{
				// Vertical (default) layout, X axis for width (axis1) & Y axis for height (axis2).
				default:
				case GridLayoutType2D.Vertical:
					position = new Vector3(gridWidth, gridHeight, 0) * _cellSize + _originPosition;

					if (_cellSpacing > 0) position += new Vector3(gridWidth * _cellSpacing, gridHeight * _cellSpacing, 0);

					return (returnedDepthValue != 0) ? position + AdditionalDepthValue(returnedDepthValue) : position;

				// Horizontal grid layout, X axis for width (axis1) & Z axis for height (axis2).
				case GridLayoutType2D.Horizontal:
					position = new Vector3(gridWidth, 0, gridHeight) * _cellSize + _originPosition;

					if (_cellSpacing > 0) position += new Vector3(gridWidth * _cellSpacing, 0, gridHeight * _cellSpacing);

					return (returnedDepthValue != 0) ? position + AdditionalDepthValue(returnedDepthValue) : position;

				// Depth grid layout, Z axis for width (axis1) & Y axis for height (axis2).
				case GridLayoutType2D.VerticalDepth:
					position = new Vector3(0, gridWidth, gridHeight) * _cellSize + _originPosition;

					if (_cellSpacing > 0) position += new Vector3(0, gridWidth * _cellSpacing, gridHeight * _cellSpacing);

					return (returnedDepthValue != 0) ? position + AdditionalDepthValue(returnedDepthValue) : position;
			}
		}

		/// <summary>
		/// Returns the worldPosition of the gridCell corresponding to the signature value.<br/>
		/// The returned position represents the bottom left corner of the gridCell.<br/>
		/// Not limited to valid grid positions, will return for any value based on the grid's calculation formula.
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public Vector3 GetWorldPosition(Vector2Int position, float returnedDepthValue = 0)
		{
			return GetWorldPosition(position.x, position.y, returnedDepthValue);
		}

		/// <summary>
		/// Returns the worldPosition of the gridCell corresponding to the signature value.<br/>
		/// The returned position represents the bottom left corner of the gridCell.<br/>
		/// Not limited to valid grid positions, will return for any value based on the grid's calculation formula.
		/// </summary>
		/// <param name="worldPosition"></param>
		/// <returns></returns>
		public Vector3 GetWorldPosition(Vector3 worldPosition, float returnedDepthValue = 0)
		{
			int gridWidth, gridHeight;
			GetInts(worldPosition, out gridWidth, out gridHeight);
			return GetWorldPosition(gridWidth, gridHeight, returnedDepthValue);
		}

		#endregion

		#region World Position - Top Left Corner

		// Returns world positions, corresponding to the top-left corner of a gridCell.
		/// <summary>
		/// Returns the worldPosition of the gridCell corresponding to the signature values.<br/>
		/// The returned position represents the top left corner of the gridCell.<br/>
		/// Not limited to valid grid positions, will return for any set of values based on the grid's calculation formula.
		/// </summary>
		/// <param name="gridWidth"></param>
		/// <param name="gridHeight"></param>
		/// <returns></returns>
		public Vector3 GetTopLeftWorldPosition(int gridWidth, int gridHeight, float returnedDepthValue = 0)
		{
			switch (_gridLayoutType)
			{
				// Both vertical grid types need to add to the height (Y) value to accurately represent the top-left corner of the cell.
				default:
				case GridLayoutType2D.Vertical:
				case GridLayoutType2D.VerticalDepth:
					return GetWorldPosition(gridWidth, gridHeight, returnedDepthValue) + new Vector3(0, _cellSize, 0);

				// Horizontal grid layout uses the Z axis for height and needs to add to it to accurately represent the top-left corner of the cell.
				case GridLayoutType2D.Horizontal:
					return GetWorldPosition(gridWidth, gridHeight, returnedDepthValue) + new Vector3(0, 0, _cellSize);
			}
		}

		/// <summary>
		/// Returns the worldPosition of the gridCell corresponding to the signature value.<br/>
		/// The returned position represents the top left corner of the gridCell.<br/>
		/// Not limited to valid grid positions, will return for any value based on the grid's calculation formula.
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public Vector3 GetTopLeftWorldPosition(Vector2Int position, float returnedDepthValue = 0)
		{
			return GetTopLeftWorldPosition(position.x, position.y, returnedDepthValue);
		}

		/// <summary>
		/// Returns the worldPosition of the gridCell corresponding to the signature value.<br/>
		/// The returned position represents the top left corner of the gridCell.<br/>
		/// Not limited to valid grid positions, will return for any value based on the grid's calculation formula.
		/// </summary>
		/// <param name="worldPosition"></param>
		/// <returns></returns>
		public Vector3 GetTopLeftWorldPosition(Vector3 worldPosition, float returnedDepthValue = 0)
		{
			int gridWidth, gridHeight;
			GetInts(worldPosition, out gridWidth, out gridHeight);
			return GetTopLeftWorldPosition(gridWidth, gridHeight, returnedDepthValue);
		}

		#endregion

		#region World Position - Bottom Right Corner

		// Returns world positions, corresponding to the bottom-right corner of a gridCell.
		/// <summary>
		/// Returns the world position of the gridCell corresponding ot the signature values.<br/>
		/// The returned position represents the bottom right corner of the gridCell.<br/>
		/// Not limited to valid grid positions, will return for any set of values based on the grid's calculation formula.
		/// </summary>
		/// <param name="gridWidth"></param>
		/// <param name="gridHeight"></param>
		/// <returns></returns>
		public Vector3 GetBottomRightWorldPosition(int gridWidth, int gridHeight, float returnedDepthValue = 0)
		{
			switch (_gridLayoutType)
			{
				// Both the default (vertical) and horizontal grids use the X axis for width,
				// requiring them to add to it's value to accurately represent the bottom right corner of the cell.
				default:
				case GridLayoutType2D.Vertical:
				case GridLayoutType2D.Horizontal:
					return GetWorldPosition(gridWidth, gridHeight, returnedDepthValue) + new Vector3(_cellSize, 0, 0);

				// Vertical depth grid uses the Z axis for width,
				// requiring an addition to it's value to accurately represent the bottom right corner of the cell.
				case GridLayoutType2D.VerticalDepth:
					return GetWorldPosition(gridWidth, gridHeight, returnedDepthValue) + new Vector3(0, 0, _cellSize);
			}
		}

		/// <summary>
		/// Returns the world position of the gridCell corresponding ot the signature value.<br/>
		/// The returned position represents the bottom right corner of the gridCell.<br/>
		/// Not limited to valid grid positions, will return for any value based on the grid's calculation formula.
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public Vector3 GetBottomRightWorldPosition(Vector2Int position, float returnedDepthValue = 0)
		{
			return GetBottomRightWorldPosition(position.x, position.y, returnedDepthValue);
		}

		/// <summary>
		/// Returns the world position of the gridCell corresponding ot the signature value.<br/>
		/// The returned position represents the bottom right corner of the gridCell.<br/>
		/// Not limited to valid grid positions, will return for any value based on the grid's calculation formula.
		/// </summary>
		/// <param name="worldPosition"></param>
		/// <returns></returns>
		public Vector3 GetBottomRightWorldPosition(Vector3 worldPosition, float returnedDepthValue = 0)
		{
			int gridWidth, gridHeight;
			GetInts(worldPosition, out gridWidth, out gridHeight);
			return GetBottomRightWorldPosition(gridWidth, gridHeight, returnedDepthValue);
		}

		#endregion

		#region World Position - Top Right Corner

		// Returns world positions, corresponding to the top-right corner of a gridCell.
		/// <summary>
		/// Returns the worldPosition of the gridCell corresponding to the signature values.<br/>
		/// The returned position represents the top right corner of the gridCell.<br/>
		/// Not limited to valid grid positions, will return for any set of values based on the grid's calculation formula.
		/// </summary>
		/// <param name="gridWidth"></param>
		/// <param name="gridHeight"></param>
		/// <returns></returns>
		public Vector3 GetTopRightWorldPosition(int gridWidth, int gridHeight, float returnedDepthValue = 0)
		{
			switch (_gridLayoutType)
			{
				// Vertical (default) grid, uses the X axis for width & Y axis for height.
				// Applies addition to both axis (X & Y).
				default:
				case GridLayoutType2D.Vertical:
					return GetWorldPosition(gridWidth, gridHeight, returnedDepthValue) + new Vector3(_cellSize, _cellSize, 0);

				// Horizontal grid, uses the X axis for width & Z axis for height.
				// Applies addition to both axis (X & Z).
				case GridLayoutType2D.Horizontal:
					return GetWorldPosition(gridWidth, gridHeight, returnedDepthValue) + new Vector3(_cellSize, 0, _cellSize);

				// Vertical depth grid, uses Z axis for width & Y axis for height.
				// Applies addition to both axis (Z & Y).
				case GridLayoutType2D.VerticalDepth:
					return GetWorldPosition(gridWidth, gridHeight, returnedDepthValue) + new Vector3(0, _cellSize, _cellSize);
			}
		}

		/// <summary>
		/// Returns the worldPosition of the gridCell corresponding to the signature value.<br/>
		/// The returned position represents the top right corner of the gridCell.<br/>
		/// Not limited to valid grid positions, will return for any value based on the grid's calculation formula.
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public Vector3 GetTopRightWorldPosition(Vector2Int position, float returnedDepthValue = 0)
		{
			return GetTopRightWorldPosition(position.x, position.y, returnedDepthValue);
		}

		/// <summary>
		/// Returns the worldPosition of the gridCell corresponding to the signature value.<br/>
		/// The returned position represents the top right corner of the gridCell.<br/>
		/// Not limited to valid grid positions, will return for any value based on the grid's calculation formula.
		/// </summary>
		/// <param name="worldPosition"></param>
		/// <returns></returns>
		public Vector3 GetTopRightWorldPosition(Vector3 worldPosition, float returnedDepthValue = 0)
		{
			int gridWidth, gridHeight;
			GetInts(worldPosition, out gridWidth, out gridHeight);
			return GetTopRightWorldPosition(gridWidth, gridHeight, returnedDepthValue);
		}

		#endregion

		#endregion

		#region Edges

		#region World Position - Left Edge Center

		// Returns world positions, corresponding to the middle of the left cell edge
		/// <summary>
		/// Returns a worldPosition corresponding to the center of the left edge of the gridCell corresponding to the signature values.<br/>
		/// Not limited to valid signature values, will return for any value based on the grid's calculation formula.
		/// </summary>
		/// <param name="gridWidth"></param>
		/// <param name="gridHeight"></param>
		/// <param name="returnedDepthValue"></param>
		/// <returns></returns>
		public Vector3 GetLeftEdgeWorldPosition(int gridWidth, int gridHeight, float returnedDepthValue = 0)
		{
			Vector3 position = GetWorldPosition(gridWidth, gridHeight) + new Vector3(0, _cellSize / 2, 0);
			return (returnedDepthValue != 0) ? position + AdditionalDepthValue(returnedDepthValue) : position;
		}

		/// <summary>
		/// Returns a worldPosition corresponding to the center of the left edge of the gridCell corresponding to the signature values.<br/>
		/// Not limited to valid signature values, will return for any value based on the grid's calculation formula.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="returnedDepthValue"></param>
		/// <returns></returns>
		public Vector3 GetLeftEdgeWorldPosition(Vector2Int position, float returnedDepthValue = 0)
		{
			return GetLeftEdgeWorldPosition(position.x, position.y, returnedDepthValue);
		}

		/// <summary>
		/// Returns a worldPosition corresponding to the center of the left edge of the gridCell corresponding to the signature values.<br/>
		/// Not limited to valid signature values, will return for any value based on the grid's calculation formula.
		/// </summary>
		/// <param name="worldPosition"></param>
		/// <param name="returnedDepthValue"></param>
		/// <returns></returns>
		public Vector3 GetLeftEdgeWorldPosition(Vector3 worldPosition, float returnedDepthValue = 0)
		{
			int gridWidth, gridHeight;
			GetInts(worldPosition, out gridWidth, out gridHeight);
			return GetLeftEdgeWorldPosition(gridWidth, gridHeight, returnedDepthValue);
		}

		#endregion

		#region World Position - Bottom Edge Center

		// Returns world positions, corresponding to the middle of the bottom cell edge
		/// <summary>
		/// Returns a worldPosition corresponding to the center of the bottom edge of the gridCell corresponding to the signature values.<br/>
		/// Not limited to valid signature values, will return for any value based on the grid's calculation formula.
		/// </summary>
		/// <param name="gridWidth"></param>
		/// <param name="gridHeight"></param>
		/// <param name="returnedDepthValue"></param>
		/// <returns></returns>
		public Vector3 GetBottomEdgeWorldPosition(int gridWidth, int gridHeight, float returnedDepthValue = 0)
		{
			Vector3 position = GetWorldPosition(gridWidth, gridHeight) + new Vector3(_cellSize / 2, 0, 0);
			return (returnedDepthValue != 0) ? position + AdditionalDepthValue(returnedDepthValue) : position;
		}

		/// <summary>
		/// Returns a worldPosition corresponding to the center of the bottom edge of the gridCell corresponding to the signature values.<br/>
		/// Not limited to valid signature values, will return for any value based on the grid's calculation formula.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="returnedDepthValue"></param>
		/// <returns></returns>
		public Vector3 GetBottomEdgeWorldPosition(Vector2Int position, float returnedDepthValue = 0)
		{
			return GetBottomEdgeWorldPosition(position.x, position.y, returnedDepthValue);
		}

		/// <summary>
		/// Returns a worldPosition corresponding to the center of the bottom edge of the gridCell corresponding to the signature values.<br/>
		/// Not limited to valid signature values, will return for any value based on the grid's calculation formula.
		/// </summary>
		/// <param name="worldPosition"></param>
		/// <param name="returnedDepthValue"></param>
		/// <returns></returns>
		public Vector3 GetBottomEdgeWorldPosition(Vector3 worldPosition, float returnedDepthValue = 0)
		{
			int gridWidth, gridHeight;
			GetInts(worldPosition, out gridWidth, out gridHeight);
			return GetBottomEdgeWorldPosition(gridWidth, gridHeight, returnedDepthValue);
		}

		#endregion

		#region World Position - Right Edge Center

		// Returns world positions, corresponding to the middle of the right cell edge
		/// <summary>
		/// Returns a worldPosition corresponding to the center of the right edge of the gridCell corresponding to the signature values.<br/>
		/// Not limited to valid signature values, will return for any value based on the grid's calculation formula.
		/// </summary>
		/// <param name="gridWidth"></param>
		/// <param name="gridHeight"></param>
		/// <param name="returnedDepthValue"></param>
		/// <returns></returns>
		public Vector3 GetRightEdgeWorldPosition(int gridWidth, int gridHeight, float returnedDepthValue = 0)
		{
			Vector3 position = GetTopRightWorldPosition(gridWidth, gridHeight) - new Vector3(0, _cellSize / 2, 0);
			return (returnedDepthValue != 0) ? position + AdditionalDepthValue(returnedDepthValue) : position;
		}

		/// <summary>
		/// Returns a worldPosition corresponding to the center of the right edge of the gridCell corresponding to the signature values.<br/>
		/// Not limited to valid signature values, will return for any value based on the grid's calculation formula.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="returnedDepthValue"></param>
		/// <returns></returns>
		public Vector3 GetRightEdgeWorldPosition(Vector2Int position, float returnedDepthValue = 0)
		{
			return GetRightEdgeWorldPosition(position.x, position.y, returnedDepthValue);
		}

		/// <summary>
		/// Returns a worldPosition corresponding to the center of the right edge of the gridCell corresponding to the signature values.<br/>
		/// Not limited to valid signature values, will return for any value based on the grid's calculation formula.
		/// </summary>
		/// <param name="worldPosition"></param>
		/// <param name="returnedDepthValue"></param>
		/// <returns></returns>
		public Vector3 GetRightEdgeWorldPosition(Vector3 worldPosition, float returnedDepthValue = 0)
		{
			int gridWidth, gridHeight;
			GetInts(worldPosition, out gridWidth, out gridHeight);
			return GetRightEdgeWorldPosition(gridWidth, gridHeight, returnedDepthValue);
		}

		#endregion

		#region World Position - Top Edge Center

		// Returns world positions, corresponding to the middle of the top cell edge
		/// <summary>
		/// Returns a worldPosition corresponding to the center of the top edge of the gridCell corresponding to the signature values.<br/>
		/// Not limited to valid signature values, will return for any value based on the grid's calculation formula.
		/// </summary>
		/// <param name="gridWidth"></param>
		/// <param name="gridHeight"></param>
		/// <param name="returnedDepthValue"></param>
		/// <returns></returns>
		public Vector3 GetTopEdgeWorldPosition(int gridWidth, int gridHeight, float returnedDepthValue = 0)
		{
			Vector3 position = GetTopRightWorldPosition(gridWidth, gridHeight) - new Vector3(_cellSize / 2, 0, 0);
			return (returnedDepthValue != 0) ? position + AdditionalDepthValue(returnedDepthValue) : position;
		}

		/// <summary>
		/// Returns a worldPosition corresponding to the center of the top edge of the gridCell corresponding to the signature values.<br/>
		/// Not limited to valid signature values, will return for any value based on the grid's calculation formula.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="returnedDepthValue"></param>
		/// <returns></returns>
		public Vector3 GetTopEdgeWorldPosition(Vector2Int position, float returnedDepthValue = 0)
		{
			return GetTopEdgeWorldPosition(position.x, position.y, returnedDepthValue);
		}

		/// <summary>
		/// Returns a worldPosition corresponding to the center of the top edge of the gridCell corresponding to the signature values.<br/>
		/// Not limited to valid signature values, will return for any value based on the grid's calculation formula.
		/// </summary>
		/// <param name="worldPosition"></param>
		/// <param name="returnedDepthValue"></param>
		/// <returns></returns>
		public Vector3 GetTopEdgeWorldPosition(Vector3 worldPosition, float returnedDepthValue = 0)
		{
			int gridWidth, gridHeight;
			GetInts(worldPosition, out gridWidth, out gridHeight);
			return GetTopEdgeWorldPosition(gridWidth, gridHeight, returnedDepthValue);
		}

		#endregion

		#endregion

		#region Centers

		#region World Position - Cell Center

		// Returns world positions, corresponding to the center of a gridCell.
		/// <summary>
		/// Returns the worldPosition of the gridCell corresponding to the signature values.<br/>
		/// The returned position represents the center of the gridCell.<br/>
		/// Not limited to valid grid positions, will return for any set of values based on the grid's calculation formula.
		/// </summary>
		/// <param name="gridWidth"></param>
		/// <param name="gridHeight"></param>
		/// <returns></returns>
		public Vector3 GetCellCenterWorldPosition(int gridWidth, int gridHeight, float returnedDepthValue = 0)
		{
			// Applies the same type of addition as in the "GetTopRightWorldPosition" method, only with half the value.
			switch (_gridLayoutType)
			{
				// Vertical (default) grid, uses the X axis for width & Y axis for height.
				default:
				case GridLayoutType2D.Vertical:
					return GetWorldPosition(gridWidth, gridHeight, returnedDepthValue) + new Vector3(_cellSize / 2, _cellSize / 2, 0);

				// Horizontal grid, uses the X axis for width & Z axis for height.
				case GridLayoutType2D.Horizontal:
					return GetWorldPosition(gridWidth, gridHeight, returnedDepthValue) + new Vector3(_cellSize / 2, 0, _cellSize / 2);

				// Vertical depth grid, uses the Z axis for width & Y axis for height.
				case GridLayoutType2D.VerticalDepth:
					return GetWorldPosition(gridWidth, gridHeight, returnedDepthValue) + new Vector3(0, _cellSize / 2, _cellSize / 2);
			}
		}

		/// <summary>
		/// Returns the worldPosition of the gridCell corresponding to the signature value.<br/>
		/// The returned position represents the center of the gridCell.<br/>
		/// Not limited to valid grid positions, will return for any value based on the grid's calculation formula.
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public Vector3 GetCellCenterWorldPosition(Vector2Int position, float returnedDepthValue = 0)
		{
			return GetCellCenterWorldPosition(position.x, position.y, returnedDepthValue);
		}

		/// <summary>
		/// Returns the worldPosition of the gridCell corresponding to the signature value.<br/>
		/// The returned position represents the center of the gridCell.<br/>
		/// Not limited to valid grid positions, will return for any value based on the grid's calculation formula.
		/// </summary>
		/// <param name="worldPosition"></param>
		/// <returns></returns>
		public Vector3 GetCellCenterWorldPosition(Vector3 worldPosition, float returnedDepthValue = 0)
		{
			int gridWidth, gridHeight;
			GetInts(worldPosition, out gridWidth, out gridHeight);
			return GetCellCenterWorldPosition(gridWidth, gridHeight, returnedDepthValue);
		}

		#endregion

		// Returns the world position corresponding to the center of the grid.
		/// <summary>
		/// Returns a worldPosition corresponding to the center of the grid.
		/// </summary>
		/// <returns></returns>
		public Vector3 GetGridCenterWorldPosition(float returnedDepthValue = 0)
		{
			Vector3 position;
			// Even number of columns.
			if (_gridWidth % 2 == 0)
			{
				// Even number of rows.
				if (_gridHeight % 2 == 0)
				{
					position = GetWorldPosition(_gridWidth / 2, _gridHeight / 2);
					return (returnedDepthValue != 0) ? position + AdditionalDepthValue(returnedDepthValue) : position;
				}
				// Odd number of rows.
				else
				{
					position = GetWorldPosition(_gridWidth / 2, Mathf.FloorToInt(_gridHeight / 2)) + new Vector3(0, _cellSize / 2, 0);
					return (returnedDepthValue != 0) ? position + AdditionalDepthValue(returnedDepthValue) : position;
				}
			}
			// Odd number of columns.
			else
			{
				// Even number of rows.
				if (_gridHeight % 2 == 0)
				{
					position = GetWorldPosition(Mathf.FloorToInt(_gridWidth / 2), _gridHeight / 2) + new Vector3(_cellSize / 2, 0, 0);
					return (returnedDepthValue != 0) ? position + AdditionalDepthValue(returnedDepthValue) : position;
				}
				// Odd number of rows.
				else
				{
					position = GetCellCenterWorldPosition(Mathf.FloorToInt(_gridWidth / 2), Mathf.FloorToInt(_gridHeight / 2));
					return (returnedDepthValue != 0) ? position + AdditionalDepthValue(returnedDepthValue) : position;
				}
			}
		}

		#endregion

		#endregion

		#region Rotations

		/// <summary>
		/// Returns the appropriate rotation for objects placed on the grid, based on the grid's layout type.
		/// </summary>
		/// <returns></returns>
		public Quaternion GetFacingRotation()
		{
			// Returns the forward facing rotation of the grid, based on it's GridLayoutType.
			return _gridLayoutType switch
			{
				// X axis for width & Z axis for height.
				GridLayoutType2D.Horizontal => Quaternion.Euler(new Vector3(-90, 0, 0)),
				// Z axis for depth & Y axis for height.
				GridLayoutType2D.VerticalDepth => Quaternion.Euler(new Vector3(0, -90, 0)),
				// Represents GridLayoutType2D.Vertical, X axis for width & Y axis for height.
				_ => Quaternion.Euler(Vector3.zero),
			};
		}

		/// <summary>
		/// Returns the inverse of the appropriate rotation for objects placed on the grid, based on the grid's layout type.
		/// </summary>
		/// <returns></returns>
		public Quaternion GetInvertedFacingRotation()
		{
			return Quaternion.Inverse(GetFacingRotation());
		}

		#endregion

		#region Camera Methods

		/// <summary>
		/// Returns a float to accurately set the camera's orthographic size.<br/>
		/// The returned value is meant to allow for the camera to show all of the grid's height + signature additionalSize.
		/// </summary>
		/// <param name="additionalSize">By how much to increase the returned value.</param>
		/// <returns></returns>
		public float GetCameraOrthographicSize(float additionalSize = 1.5f)
		{
			// Returns the required camera orthographic size to display the whole grid.
			// The returned value is always bigger than the minimum requiremnent by additionalSize to create a buffer.
			return ((_gridHeight * _cellSize) / 2) + additionalSize;
		}

		/// <summary>
        /// Returns a Vector3 positional value to properly place the camera.<br/>
		/// The returned value is meant to be used to place the camera alligned to the center of the grid.
        /// </summary>
        /// <param name="zAxisDepth"></param>
        /// <returns></returns>
		public Vector3 GetGridCameraPosition(float zAxisDepth = -10)
        {
            return _originPosition + new Vector3(_gridWidth / 2, _gridHeight / 2, zAxisDepth);
        }

		#endregion

		#region Grid Object Methods

		#region Set Grid Object

		/// <summary>
		/// Sets the gridObject corresponding to the signature values to the signature value.<br/>
		/// Returns true if the signature values mathced a valid grid position.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool SetGridObject(int width, int height, TGridObject value)
		{
			// Checks if the signature values represent a valid position on the grid.
			if (IsValidGridPosition(width, height))
			{
				// If it is a valid position, set it's value to the signature value.
				// If the position is valid, return true.
				_gridArray[width, height] = value;
				TriggerGridObjectChanged(width, height);
				return true;
			}
			// Else return false.
			return false;
		}

		/// <summary>
		/// Sets the gridObject corresponding to the signature position to the signature value.<br/>
		/// Returns true if the signature values mathced a valid grid position.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool SetGridObject(Vector2Int position, TGridObject value)
		{
			return SetGridObject(position.x, position.y, value);
		}

		/// <summary>
		/// Sets the gridObject corresponding to the signature worldPosition to the signature value.<br />
		/// Returns true if the signature worldPosition matched a valid grid position.
		/// </summary>
		/// <param name="worldPosition"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool SetGridObject(Vector3 worldPosition, TGridObject value)
		{
			int axis1, axis2;
			GetInts(worldPosition, out axis1, out axis2);
			return SetGridObject(axis1, axis2, value);
		}

		#endregion

		#region Get Grid Object

		/// <summary>
		/// Returns the gridObject corresponding to the signature values.<br />
		/// If the signature values didn't match a valid grid position, returns a default gridObject.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public TGridObject GetGridObject(int width, int height)
		{
			// Checks if the signature values represent a valid grid position.
			// If they are, return the TGridObject of the grid position.
			if (IsValidGridPosition(width, height)) return _gridArray[width, height];
			// Else return default.
			else return default(TGridObject);
		}

		/// <summary>
		/// Returns the gridObject corresponding to the signature value.<br />
		/// If the signature values didn't match a valid grid position, returns a default gridObject.
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public TGridObject GetGridObject(Vector2Int position)
		{
			return GetGridObject(position.x, position.y);
		}

		/// <summary>
		/// Returns the gridObject corresponding to the signature worldPosition.<br />
		/// If the signature worldPosition didn't match a valid grid position, returns a default gridObject.
		/// </summary>
		/// <param name="worldPosition"></param>
		/// <returns></returns>
		public TGridObject GetGridObject(Vector3 worldPosition)
		{
			int axis1, axis2;
			GetInts(worldPosition, out axis1, out axis2);
			return GetGridObject(axis1, axis2);
		}

		#endregion

		#endregion

		#region Event Trigger Methods

		/// <summary>
		/// Triggers the OnGridObjectChanged event with the signature values.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void TriggerGridObjectChanged(int width, int height)
		{
			OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { GridWidth = width, GridHeight = height });
		}

		/// <summary>
		/// Triggers the OnGridObjectChanged event with the signature value.
		/// </summary>
		/// <param name="position"></param>
		public void TriggerGridObjectChanged(Vector2Int position)
		{
			OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { GridWidth = position.x, GridHeight = position.y });
		}

		/// <summary>
		/// Triggers the OnGridObjectChanged event with the signature value.
		/// </summary>
		/// <param name="worldPosition"></param>
		public void TriggerGridObjectChanged(Vector3 worldPosition)
		{
			int axis1, axis2;
			GetInts(worldPosition, out axis1, out axis2);
			OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { GridWidth = axis1, GridHeight = axis2 });
		}

		#endregion

		#endregion


	}
}