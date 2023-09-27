using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetBehavior : MonoBehaviour
{
    #region Properties
    [SerializeField] private int currentHealth;
    #endregion

    #region Methods
    public void InflictDamage(int _damage)
    {
        currentHealth -= _damage;
        GPSingleton.Instance.UICtrl.planetHealthBar.SetSliderValue(currentHealth, 50);
    }

    #endregion

    #region Unity API
    void Start()
    {
        GPSingleton.Instance.UICtrl.planetHealthBar.SetSliderValue(currentHealth, 50);
    }

    void Update()
    {
        
    }
    #endregion
}
