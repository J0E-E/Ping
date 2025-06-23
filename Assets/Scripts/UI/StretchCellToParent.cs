using System;
using UnityEngine;
using UnityEngine.UI;

public class StretchCellToParent : MonoBehaviour
{
    private GridLayoutGroup _grid;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _grid = GetComponent<GridLayoutGroup>();
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        float totalWidth = _rectTransform.rect.width;
        float padding = _grid.padding.left + _grid.padding.right;
        float spacing = _grid.spacing.x * (_grid.constraintCount - 1);
        float cellWidth = (totalWidth - padding - spacing) / _grid.constraintCount;

        _grid.cellSize = new Vector2(cellWidth, _grid.cellSize.y);
    }
}
