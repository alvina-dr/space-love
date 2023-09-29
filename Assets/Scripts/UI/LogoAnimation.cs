using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LogoAnimation : MonoBehaviour
{
    void Start()
    {
        transform.DOScale(1.01f, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);

    }
}
