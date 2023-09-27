using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetBehavior : MonoBehaviour
{
    #region Properties
    private int currentHealth;
    #endregion

    #region Methods
    public void InflictDamage(int _damage)
    {
        currentHealth -= _damage;
        GPSingleton.Instance.UICtrl.planetHealthBar.SetSliderValue(currentHealth, DataHolder.Instance.GeneralData.planetMaxHealth);
        if (currentHealth <= 0) GPSingleton.Instance.GameOver();
    }

    #endregion

    #region Unity API
    void Start()
    {
        currentHealth = DataHolder.Instance.GeneralData.planetMaxHealth;
        GPSingleton.Instance.UICtrl.planetHealthBar.SetSliderValue(currentHealth, DataHolder.Instance.GeneralData.planetMaxHealth);
    }
    #endregion
}
