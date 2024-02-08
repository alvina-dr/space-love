using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FrenzyFX : MonoBehaviour
{
    public Animator animator;
    public void WarningEndFrenzy()
    {
        animator.SetTrigger("FrenzyShake");
    }

    public void EndFrenzy()
    {
        animator.SetTrigger("FrenzyEnd");
    }

    private void Start()
    {
        DOVirtual.DelayedCall(DataHolder.Instance.GeneralData.loveFrenzyDuration / 10 * 8, () => WarningEndFrenzy());
        DOVirtual.DelayedCall(DataHolder.Instance.GeneralData.loveFrenzyDuration, () => EndFrenzy());
        DOVirtual.DelayedCall(DataHolder.Instance.GeneralData.loveFrenzyDuration + 0.3f, () => Destroy(gameObject));
    }
}
