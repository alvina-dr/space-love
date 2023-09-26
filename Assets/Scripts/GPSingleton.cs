using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSingleton : MonoBehaviour
{
    #region Properties
    public static GPSingleton Instance { get; private set; }
    #endregion

    #region Unity API
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion
}
