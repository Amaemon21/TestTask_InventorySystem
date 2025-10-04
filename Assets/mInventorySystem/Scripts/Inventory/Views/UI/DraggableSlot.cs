using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DraggableSlot : MonoBehaviour
{
    [field: SerializeField] public Image Icon { get; private set; }
    
    public Vector2Int SlotCoordA { get; set; }
    public Vector2Int SlotCoordB { get; set; }

    private void OnEnable()
    {
        Icon.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f).SetLink(Icon.gameObject);
    }

    private void OnDisable()
    {
        Icon.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f).SetLink(Icon.gameObject);
    }
}
