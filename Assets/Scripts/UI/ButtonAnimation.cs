using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonAnimation : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    #region Properties
    public Image background;
    public TextMeshProUGUI text;
    #endregion

    public void OnDeselect(BaseEventData eventData)
    {
        text.color = MainMenu.Instance.purple;
        background.color = MainMenu.Instance.transparent;

    }

    public void OnSelect(BaseEventData eventData)
    {
        text.color = Color.black;
        background.color = MainMenu.Instance.purple;
    }
}
