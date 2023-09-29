using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LogoAnimation : MonoBehaviour
{
    public float bigSize;
    void Start()
    {
        transform.DOScale(bigSize, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);

    }
}
