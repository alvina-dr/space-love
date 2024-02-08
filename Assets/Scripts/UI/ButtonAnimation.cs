using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
using FMODUnity;

public class ButtonAnimation : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    #region Properties
    public Image background;
    public TextMeshProUGUI text;
    [SerializeField] private string onClickSoundName;
    #endregion

    [SerializeField] private Color active;
    [SerializeField] private Color inactive;

    public void OnDeselect(BaseEventData eventData)
    {
        text.color = active;
        background.color = inactive;
        transform.DOScale(1f, .3f);
    }

    public void OnSelect(BaseEventData eventData)
    {
        text.color = Color.black;
        background.color = active;
        transform.DOScale(1.05f, .3f);
        var audioEvent = RuntimeManager.CreateInstance("event:/UI/Hover");
        audioEvent.start();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        OnDeselect(null);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnSelect(null);
    }

    public void Activate()
    {
        var audioEvent = RuntimeManager.CreateInstance("event:/UI/Click");
        if (onClickSoundName != "") audioEvent = RuntimeManager.CreateInstance(onClickSoundName);
        audioEvent.start();
    }
}
