using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonAnimation : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    #region Properties
    public Image background;
    public TextMeshProUGUI text;
    #endregion

    [SerializeField] private Color active;
    [SerializeField] private Color inactive;

    public void OnDeselect(BaseEventData eventData)
    {
        text.color = active;
        background.color = inactive;

    }

    public void OnSelect(BaseEventData eventData)
    {
        text.color = Color.black;
        background.color = active;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        OnDeselect(null);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnSelect(null);
    }
}
