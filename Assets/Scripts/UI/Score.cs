using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI text;

    #region Methods
    public void SetValue(int _value)
    {
        text.transform.DOScale(1.1f, .1f).OnComplete(() =>
        {
            text.text = _value.ToString();
            text.transform.DOScale(1f, .1f);
        });
    }
    #endregion
}
