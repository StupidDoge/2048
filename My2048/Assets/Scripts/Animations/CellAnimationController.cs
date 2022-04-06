using DG.Tweening;
using UnityEngine;

public class CellAnimationController : MonoBehaviour
{
    public static CellAnimationController Instance { get; private set; }

    [SerializeField] CellAnimation _cellAnimation;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        DOTween.Init();
    }

    public void SmoothTransition(Cell startCell, Cell targetCell, bool isMerging)
    {
        Instantiate(_cellAnimation, transform, false).Move(startCell, targetCell, isMerging);
    }

    public void SmoothAppear(Cell cell)
    {
        Instantiate(_cellAnimation, transform, false).Appear(cell);
    }
}
