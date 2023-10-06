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
    [SerializeField] private MeshRenderer meshRenderer;
    private Tweener shake;
    #endregion

    #region Methods
    public void InflictDamage(int _damage)
    {
        if (GPCtrl.Instance.spawnerMode == GPCtrl.SpawnerMode.Menu) return; 
        currentHealth -= _damage;
        mesh.DOScale(1.1f, .2f).OnComplete(() =>
        {
            shake.Kill();
            mesh.transform.position = Vector3.zero;
            mesh.DOScale(1f, .2f);
            shake = mesh.DOShakePosition(.1f, new Vector3(.1f, .1f, 0), 1);
            var damageEvent = RuntimeManager.CreateInstance("event:/Earth/PlanetHit");
            damageEvent.start();
            float _destruction = currentHealth * 0.38f / DataHolder.Instance.GeneralData.planetMaxHealth ;
            if(meshRenderer != null) meshRenderer.material.SetFloat("_destruction", _destruction);
        });
        GPCtrl.Instance.UICtrl.planetHealthBar.SetSliderValue(currentHealth, DataHolder.Instance.GeneralData.planetMaxHealth);
        serialController.SendSerialMessage((currentHealth+10).ToString());
        if (currentHealth <= 0)
        {
            var deathEvent = RuntimeManager.CreateInstance("event:/Earth/PlanetExplosion");
            deathEvent.start();
            GPCtrl.Instance.GameOver();
        }
    }
    #endregion

    #region Unity API
    void Start()
    {
        if(GPCtrl.Instance.spawnerMode == GPCtrl.SpawnerMode.Game)
        {
            currentHealth = DataHolder.Instance.GeneralData.planetMaxHealth;
            serialController.SendSerialMessage((currentHealth + 10).ToString());
            GPCtrl.Instance.UICtrl.planetHealthBar.SetSliderValue(currentHealth, DataHolder.Instance.GeneralData.planetMaxHealth);
            mesh.DOLocalRotate(new Vector3(0, 180, 0), 3f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
        }
    }
    #endregion
}
