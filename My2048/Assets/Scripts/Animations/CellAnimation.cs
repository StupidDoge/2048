using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CellAnimation : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _points;

    private float _moveTime = .1f;
    private float _appearTime = .1f;

    private Sequence _sequence;

    public void Move(Cell startCell, Cell targetCell, bool isMerging)
    {
        startCell.CancelAnimation();
        targetCell.SetAnimation(this);

        _image.color = ColorManager.Instance.CurrentThemeColorsArray[startCell.Value];
        _points.text = startCell.Number.ToString();
        _points.color = startCell.Value <= 2 ? ColorManager.Instance.CurrentDarkTextColor : ColorManager.Instance.CurrentLightTextColor;

        transform.position = startCell.transform.position;

        _sequence = DOTween.Sequence();
        _sequence.Append(transform.DOMove(targetCell.transform.position, _moveTime).SetEase(Ease.InOutQuad));

        if (isMerging)
        {
            _sequence.AppendCallback(() =>
            {
                _image.color = ColorManager.Instance.CurrentThemeColorsArray[targetCell.Value];
                _points.text = targetCell.Number.ToString();
                _points.color = targetCell.Value <= 2 ? ColorManager.Instance.CurrentDarkTextColor : ColorManager.Instance.CurrentLightTextColor;
            });

            _sequence.Append(transform.DOScale(1.2f, _appearTime));
            _sequence.Append(transform.DOScale(1f, _appearTime));
        }

        _sequence.AppendCallback(() =>
        {
            targetCell.UpdateCell();
            Destroy();
        });
    }

    public void Appear(Cell cell)
    {
        cell.CancelAnimation();
        cell.SetAnimation(this);

        _image.color = ColorManager.Instance.CurrentThemeColorsArray[cell.Value];
        _points.text = cell.Number.ToString();
        _points.color = cell.Value <= 2 ? ColorManager.Instance.CurrentDarkTextColor : ColorManager.Instance.CurrentLightTextColor;

        transform.position = cell.transform.position;
        transform.localScale = Vector2.zero;

        _sequence = DOTween.Sequence();

        _sequence.Append(transform.DOScale(1.2f, _appearTime));
        _sequence.Append(transform.DOScale(1f, _appearTime));
        _sequence.AppendCallback(() =>
        {
            cell.UpdateCell();
            Destroy();
        });
    }

    public void Destroy()
    {
        _sequence.Kill();
        Destroy(gameObject);
    }
}
