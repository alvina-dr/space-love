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

    public void FlashPurple()
    {
        Enemy[] enemyArray = FindObjectsOfType<Enemy>();
        for (int i = 0; i < enemyArray.Length; i++)
        {
            enemyArray[i].ChangeColor(EnemyData.Color.White);
        }
    }

    public void FlashTrueColor()
    {
        Enemy[] enemyArray = FindObjectsOfType<Enemy>();
        for (int i = 0; i < enemyArray.Length; i++)
        {
            enemyArray[i].ChangeColor(enemyArray[i].currentColor);
        }
    }

    private void Start()
    {
        DOVirtual.DelayedCall(DataHolder.Instance.GeneralData.loveFrenzyDuration / 10 * 8, () => {
            WarningEndFrenzy();
            FlashTrueColor();
            DOVirtual.DelayedCall(DataHolder.Instance.GeneralData.loveFrenzyDuration / 30, () =>
            {
                FlashPurple();
                DOVirtual.DelayedCall(DataHolder.Instance.GeneralData.loveFrenzyDuration / 30, () =>
                {
                    FlashTrueColor();
                    DOVirtual.DelayedCall(DataHolder.Instance.GeneralData.loveFrenzyDuration / 30, () =>
                    {
                        FlashPurple();
                        DOVirtual.DelayedCall(DataHolder.Instance.GeneralData.loveFrenzyDuration / 30, () =>
                        {
                            FlashTrueColor();
                            DOVirtual.DelayedCall(DataHolder.Instance.GeneralData.loveFrenzyDuration / 30, () =>
                            {
                                FlashPurple();
                            });
                        });
                    });
                });

            });

        });
        DOVirtual.DelayedCall(DataHolder.Instance.GeneralData.loveFrenzyDuration, () => EndFrenzy());
        DOVirtual.DelayedCall(DataHolder.Instance.GeneralData.loveFrenzyDuration + 0.3f, () => Destroy(gameObject));
    }
}
