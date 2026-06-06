using UnityEngine;

public class PuzzleDesignInput : MonoBehaviour
{
    [SerializeField] private SymbolGridManager _gridManager;
    [SerializeField] private LayerMask _castTargetLayers;

    private SymbolGridCellVisual _gridCellVisual;
    private Vector3 _mouseWorldPosition;

    private void Update()
    {
        // Calculate cast variables.
        _mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 castDirection = (_mouseWorldPosition - Camera.main.transform.position).normalized;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Check if currently hovering over a grid cell.
        if (Physics.Raycast(ray, out RaycastHit info, Mathf.Infinity, _castTargetLayers))
        {
            // Check that we managed to acquire the grid cell's script reference.
            if (info.collider.transform.parent.TryGetComponent(out _gridCellVisual))
            {
                // Highlight the grid cell pedestal - if it's not yet highlighted.
                if (!_gridCellVisual.IsHighlighted)
                {
                    _gridCellVisual.SetHighlightState(true);
                }

                // Check for input, and change the current symbol in the pedestal.
                if (Input.GetKeyDown(KeyCode.E))
                {
                    _gridManager.InteractWithGridCell(_gridCellVisual.gameObject.transform.position);
                }
            }
        }
        // If the cast did not hit anything, ensure we are not forgetting to de-highlight cell visuals that we may have previously highlighted.
        else
        {
            if (_gridCellVisual != null)
            {
                _gridCellVisual.SetHighlightState(false);
                _gridCellVisual = null;
            }
        }
    }

    
}
