using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSingleton : MonoBehaviour
{
    #region Properties
    public static GPSingleton Instance { get; private set; }
    public PlanetBehavior Planet;
    public UICtrl UICtrl;
    public Color visibleRed;
    public Color visibleBlue;
    public Color visibleAll;
    #endregion

    #region Methods
    public void SetColor(Renderer _renderer, EnemyData.Color _color)
    {
        switch(_color)
        {
            case EnemyData.Color.White:
                _renderer.material.color = visibleAll;
                break;
            case EnemyData.Color.Red:
                _renderer.material.color = visibleRed;
                break;
            case EnemyData.Color.Blue:
                _renderer.material.color = visibleBlue;
                break;
        }
    }
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
