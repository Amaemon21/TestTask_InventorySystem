using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoView : MonoBehaviour
{
    [SerializeField] private Image _imageIcon;
    [SerializeField] private TMP_Text _textItem;
    
    public void Show(Sprite icon, string description)
    {
        _imageIcon.sprite = icon;
        _textItem.text = description;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    private void OnDisable()
    {
        Hide();
    }
}