using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Value { get; private set; }
    public bool IsEmpty => Value == 0;
    public bool HasMerged;
    public int Number => IsEmpty ? 0 : (int)Mathf.Pow(2, Value);

    public const int MAX_VALUE = 15;

    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _number;

    private CellAnimation _currentAnimation;

    public void SetCellSize(float width, float height)
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }

    public void SetValues(int x, int y, int value, bool updateUI = true)
    {
        X = x;
        Y = y;
        Value = value;

        if (updateUI)
            UpdateCell();
    }

    public void IncreaseValue()
    {
        Value++;
        HasMerged = true;

        GameController.Instance.AddPoints(Number);
        Field.Instance.PointsPerMove += Number;
    }

    public void ResetFlags()
    {
        HasMerged = false;
    }

    public void MergeWithCell(Cell otherCell)
    {
        CellAnimationController.Instance.SmoothTransition(this, otherCell, true);
        otherCell.IncreaseValue();
        SetValues(X, Y, 0);
    }

    public void MoveToCell(Cell target)
    {
        CellAnimationController.Instance.SmoothTransition(this, target, false);

        target.SetValues(target.X, target.Y, Value, false);
        SetValues(X, Y, 0);

        UpdateCell();
    }

    public void UpdateCell()
    {
        _number.text = IsEmpty ? string.Empty : Number.ToString();
        _number.color = Value <= 2 ? ColorManager.Instance.CurrentDarkTextColor : ColorManager.Instance.CurrentLightTextColor;
        _image.color = ColorManager.Instance.CurrentThemeColorsArray[Value];
    }

    public void SetAnimation(CellAnimation animation)
    {
        _currentAnimation = animation;
    }

    public void CancelAnimation()
    {
        if (_currentAnimation != null)
            _currentAnimation.Destroy();
    }
}
