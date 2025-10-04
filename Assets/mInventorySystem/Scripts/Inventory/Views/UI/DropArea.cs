using DG.Tweening;
using UnityEngine;

public class DropArea : MonoBehaviour
{
    [SerializeField] private Vector3 _showPosition;
    [SerializeField] private Vector3 _hidePosition;
    
    [SerializeField] private float _animationDuration;

    private RectTransform _rectTransform;
    
    private void Awake()
    {
        _rectTransform = (RectTransform)transform;
    }

    public void Show()
    {
        _rectTransform.DOAnchorPos(_showPosition, _animationDuration).SetLink(_rectTransform.gameObject);
    }

    public void Hide()
    {
        _rectTransform.DOAnchorPos(_hidePosition, _animationDuration).SetLink(_rectTransform.gameObject);
    }
}