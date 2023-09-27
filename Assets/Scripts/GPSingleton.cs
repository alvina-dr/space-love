using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSingleton : MonoBehaviour
{
    #region Properties
    public static GPSingleton Instance { get; private set; }
    public PlanetBehavior Planet;
    public UICtrl UICtrl;
    public float spawnRadius;
    public Color visibleRed;
    public Color visibleBlue;
    public Color visibleAll;
    [SerializeField] private List<EnemyData> enemyDataList = new List<EnemyData>();
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

    void spawnEnemy(Enemy enemy)
    {
        float angle = Random.Range(0f, 2.0f * Mathf.PI);
        Instantiate(enemy, new Vector3(-spawnRadius * Mathf.Cos(angle), spawnRadius * Mathf.Sin(angle), 0), Quaternion.identity);
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
        EnemyData[] enemyDataArray = Resources.LoadAll<EnemyData>("EnemyData");
        for (int i = 0; i < enemyDataArray.Length; i++)
        {
            enemyDataList.Add(enemyDataArray[i]);
        }

    }
    #endregion
}
