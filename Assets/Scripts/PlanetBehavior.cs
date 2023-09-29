using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FMODUnity;
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
        if (GPSingleton.Instance.spawnerMode == GPSingleton.SpawnerMode.Menu) return; 
        currentHealth -= _damage;
        mesh.DOScale(1.1f, .2f).OnComplete(() =>
        {
            mesh.DOKill();
            mesh.transform.position = Vector3.zero;
            mesh.DOScale(1f, .2f);
            mesh.DOShakePosition(.1f, new Vector3(.1f, .1f, 0), 1);
            var damageEvent = RuntimeManager.CreateInstance("event:/Earth/PlanetHit");
            damageEvent.start();
        });
        GPSingleton.Instance.UICtrl.planetHealthBar.SetSliderValue(currentHealth, DataHolder.Instance.GeneralData.planetMaxHealth);
        serialController.SendSerialMessage((currentHealth+10).ToString());
        if (currentHealth <= 0)
        {
            var deathEvent = RuntimeManager.CreateInstance("event:/Earth/PlanetExplosion");
            deathEvent.start();
            GPSingleton.Instance.GameOver();
        }
    }
    #endregion

    #region Unity API
    void Start()
    {
        if(GPSingleton.Instance.spawnerMode == GPSingleton.SpawnerMode.Game)
        {
            currentHealth = DataHolder.Instance.GeneralData.planetMaxHealth;
            serialController.SendSerialMessage((currentHealth + 10).ToString());
            GPSingleton.Instance.UICtrl.planetHealthBar.SetSliderValue(currentHealth, DataHolder.Instance.GeneralData.planetMaxHealth);
            mesh.DOLocalRotate(new Vector3(0, 180, 0), 3f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
        }
    }
    #endregion
}
