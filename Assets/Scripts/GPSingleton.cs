using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

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
    private float startTime;
    private List<float> timerList = new List<float>();
    public SoundData SoundData;
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

    void SpawnEnemy(Enemy enemyPrefab)
    {
        float angle = Random.Range(0f, 2.0f * Mathf.PI);
        Vector3 pos = new Vector3(spawnRadius * Mathf.Cos(angle), spawnRadius * Mathf.Sin(angle), 0);
        Instantiate(enemyPrefab, pos, Quaternion.LookRotation(-pos,Vector3.forward));
    }
    #endregion

    #region Unity API
    void Start()
    {
        var audioEvent = RuntimeManager.CreateInstance("event:/Character/TirFeu");
        audioEvent.start();
        foreach (EnemyData enemy in enemyDataList)
        {
            timerList.Add(enemy.spawnRate);
        }
    }

    private void FixedUpdate()
    {
        float timeSinceStart = Time.time - startTime;
        for(int i = 0; i < timerList.Count; i++)
        {
            if(timeSinceStart < enemyDataList[i].spawnTime)
            {
                continue;
            }
            timerList[i] -= Time.fixedDeltaTime;
            if (timerList[i] <= 0f)
            {
                timerList[i] = enemyDataList[i].spawnRate;
                SpawnEnemy(enemyDataList[i].enemyPrefab);
            }
        }
    }
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
