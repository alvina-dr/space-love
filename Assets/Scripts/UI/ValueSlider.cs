using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ValueSlider : MonoBehaviour
{
    #region Properties
    [SerializeField] private Slider slider;
    #endregion

    #region Methods
    public void SetSliderValue(float _value, float _maxValue)
    {
        slider.DOValue(_value / _maxValue, .2f);
    }
    #endregion
}
