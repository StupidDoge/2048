using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public static Field Instance;

    [Header("Field Properties")]

    public float CellSize;
    public float Spacing;
    public int FieldSize;
    public int PointsPerMove = 0;

    [Space(5)]

    [SerializeField] private Cell _cell;
    [SerializeField] private RectTransform _transform;
    [SerializeField] private int _initCellsCount;
    [SerializeField] private bool _anyCellMoved;

    private string saveKey;

    public Cell[,] CellMatrix { get; private set; }

    private int[,] _savedField;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        SwipeDetection.SwipeEvent += OnInput;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.A))
            OnInput(Vector2.left);
        if (Input.GetKeyDown(KeyCode.D))
            OnInput(Vector2.right);
        if (Input.GetKeyDown(KeyCode.W))
            OnInput(Vector2.up);
        if (Input.GetKeyDown(KeyCode.S))
            OnInput(Vector2.down);
#endif
    }

    private void OnInput(Vector2 direction)
    {
        if (!GameController.GameStarted)
            return;

        SaveField();
        // проблема - после хода без движени€ €чеек anycellmoved == true, поэтому поле всЄ равно сохран€етс€

        _anyCellMoved = false;
        ResetCellsFlags();
        PointsPerMove = 0;
        GameController.CanReturnMove = true;

        Move(direction);

        if (_anyCellMoved)
        {
            GenerateRandomCell();
            CheckGameResult();
        }

        GameController.Instance.Save();
    }

    private void Move(Vector2 direction)
    {
        int startXY = direction.x > 0 || direction.y < 0 ? FieldSize - 1 : 0;
        int dir = direction.x != 0 ? (int)direction.x : -(int)direction.y;

        for (int i = 0; i < FieldSize; i++)
        {
            for (int j = startXY; j < FieldSize && j >= 0; j -= dir)
            {
                var cell = direction.x != 0 ? CellMatrix[j, i] : CellMatrix[i, j];

                if (cell.IsEmpty)
                    continue;

                var cellToMerge = FindCellToMerge(cell, direction);
                if (cellToMerge != null)
                {
                    cell.MergeWithCell(cellToMerge);
                    _anyCellMoved = true;

                    continue;
                }

                var emptyCell = FindEmptyCell(cell, direction);
                if (emptyCell != null)
                {
                    cell.MoveToCell(emptyCell);
                    _anyCellMoved = true;
                }
            }
        }
    }

    private Cell FindCellToMerge(Cell cell, Vector2 direction)
    {
        int startX = cell.X + (int)direction.x;
        int startY = cell.Y - (int)direction.y;

        for (int x = startX, y = startY; 
            x >= 0 && x < FieldSize && y >=0 && y < FieldSize; 
            x += (int)direction.x, y -= (int)direction.y)
        {
            if (CellMatrix[x, y].IsEmpty)
                continue;

            if (CellMatrix[x, y].Value == cell.Value && !CellMatrix[x, y].HasMerged)
                return CellMatrix[x, y];

            break;
        }

        return null;
    }

    private Cell FindEmptyCell(Cell cell, Vector2 direction)
    {
        Cell emptyCell = null;

        int startX = cell.X + (int)direction.x;
        int startY = cell.Y - (int)direction.y;

        for (int x = startX, y = startY;
            x >= 0 && x < FieldSize && y >= 0 && y < FieldSize;
            x += (int)direction.x, y -= (int)direction.y)
        {
            if (CellMatrix[x, y].IsEmpty)
                emptyCell = CellMatrix[x, y];
            else
                break;
        }

        return emptyCell;
    }

    private void CheckGameResult()
    {
        bool lose = true;

        for (int x = 0; x < FieldSize; x++)
        {
            for (int y = 0; y < FieldSize; y++)
            {
                if (CellMatrix[x, y].Value == Cell.MAX_VALUE)
                {
                    GameController.Instance.Win();
                    return;
                }

                if (CellMatrix[x, y].IsEmpty || 
                    FindCellToMerge(CellMatrix[x, y], Vector2.up) ||
                    FindCellToMerge(CellMatrix[x, y], Vector2.left) ||
                    FindCellToMerge(CellMatrix[x, y], Vector2.right) ||
                    FindCellToMerge(CellMatrix[x, y], Vector2.down))
                {
                    lose = false;
                }
            }
        }

        if (lose)
            GameController.Instance.Lose();
    }

    private void CreateField()
    {
        CellMatrix = new Cell[FieldSize, FieldSize];

        _savedField = new int[FieldSize, FieldSize];

        float fieldWidth = FieldSize * (CellSize + Spacing) + Spacing;
        _transform.sizeDelta = new Vector2(fieldWidth, fieldWidth);

        float startX = -(fieldWidth / 2) + (CellSize / 2) + Spacing;
        float startY = (fieldWidth / 2) - (CellSize / 2) - Spacing;

        for (int x = 0; x < FieldSize; x++)
        {
            for (int y = 0; y < FieldSize; y++)
            {
                Cell newCell = Instantiate(_cell, transform, false);
                Vector2 position = new Vector2(startX + (x * (CellSize + Spacing)), startY - (y * (CellSize + Spacing)));
                newCell.transform.localPosition = position;

                CellMatrix[x, y] = newCell;
                newCell.SetValues(x, y, 0);
            }
        }
    }

    public void GenerateField(bool spawnDefaultCells = true)
    {
        if (CellMatrix == null)
            CreateField();

        for (int x = 0; x < FieldSize; x++)
        {
            for (int y = 0; y < FieldSize; y++)
            {
                CellMatrix[x, y].SetValues(x, y, 0);
                CellMatrix[x, y].SetCellSize(CellSize, CellSize);
            }
        }

        if (spawnDefaultCells)
            for (int i = 0; i < _initCellsCount; i++)
                GenerateRandomCell();

        SaveField();
    }

    private void GenerateRandomCell()
    {
        var emptyCells = new List<Cell>();

        for (int x = 0; x < FieldSize; x++)
            for (int y = 0; y < FieldSize; y++)
                if (CellMatrix[x, y].IsEmpty)
                    emptyCells.Add(CellMatrix[x, y]);

        if (emptyCells.Count == 0)
            throw new System.Exception("No empty cells");

        int value = Random.Range(0, 10) == 0 ? 2 : 1;
        Cell cell = emptyCells[Random.Range(0, emptyCells.Count)];
        cell.SetValues(cell.X, cell.Y, value, false);

        CellAnimationController.Instance.SmoothAppear(cell);
    }

    private void ResetCellsFlags()
    {
        for (int x = 0; x < FieldSize; x++)
            for (int y = 0; y < FieldSize; y++)
                CellMatrix[x, y].ResetFlags();
    }

    public void SaveField()
    {
        for (int x = 0; x < FieldSize; x++)
            for (int y = 0; y < FieldSize; y++)
                _savedField[x, y] = CellMatrix[x, y].Value;
    }

    public void ResetField()
    {
        for (int x = 0; x < FieldSize; x++)
            for (int y = 0; y < FieldSize; y++)
                CellMatrix[x, y].SetValues(x, y, _savedField[x, y]);
    }
}
