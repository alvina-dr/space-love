using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlanetBehavior : MonoBehaviour
{
    #region Properties
    private int currentHealth;
    public SerialController serialController;
    [SerializeField] private Transform mesh;
    #endregion

    #region Methods
    public void InflictDamage(int _damage)
    {
        currentHealth -= _damage;
        mesh.DOScale(1.1f, .2f).OnComplete(() =>
        {
            mesh.DOScale(1f, .2f);
            mesh.DOShakePosition(.8f, .1f, 10);
        });
        GPSingleton.Instance.UICtrl.planetHealthBar.SetSliderValue(currentHealth, DataHolder.Instance.GeneralData.planetMaxHealth);
        serialController.SendSerialMessage((currentHealth+10).ToString());
        Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
            GPSingleton.Instance.GameOver();
        }
    }
    #endregion

    #region Unity API
    void Start()
    {
        currentHealth = DataHolder.Instance.GeneralData.planetMaxHealth;
        serialController.SendSerialMessage(currentHealth.ToString());
        GPSingleton.Instance.UICtrl.planetHealthBar.SetSliderValue(currentHealth, DataHolder.Instance.GeneralData.planetMaxHealth);
        mesh.DOLocalRotate(new Vector3(0, 180, 0), 3f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
    }
    #endregion
}
