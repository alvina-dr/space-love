using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.VFX;

public class GPSingleton : MonoBehaviour
{
    #region Properties
    public static GPSingleton Instance { get; private set; }
    [Header("REFERENCES")]
    public PlanetBehavior Planet;
    public PlayerCursor PlayerRed;
    public PlayerCursor PlayerBlue;
    public UICtrl UICtrl;

    [SerializeField] private List<EnemyData> enemyDataList = new List<EnemyData>();
    public SoundData SoundData;
    public bool pause = false;

    [Header("SPAWN")]
    public float spawnRadius;
    private float startTime;
    private List<float> timerList = new List<float>();

    [Header("COLORS")]
    public Color visibleRed;
    public Color visibleBlue;
    public Color visibleAll;
    public Gradient visibleGradientRed;
    public Gradient visibleGradientBlue;
    public Gradient visibleGradientAll;

    [Header("FX")]
    public GameObject explosionDeathEffect;

    [Header("SCORE")]
    public int currentScore;
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

    public void SetVFX(VisualEffect _renderer, EnemyData.Color _color)
    {
        switch (_color)
        {
            case EnemyData.Color.White:
                _renderer.SetGradient("smokeColor", visibleGradientAll);
                break;
            case EnemyData.Color.Red:
                _renderer.SetGradient("smokeColor", visibleGradientRed);
                break;
            case EnemyData.Color.Blue:
                _renderer.SetGradient("smokeColor", visibleGradientBlue);
                break;
        }
    }

    void SpawnEnemy(Enemy enemyPrefab)
    {
        float angle = Random.Range(0f, 2.0f * Mathf.PI);
        Vector3 pos = new Vector3(spawnRadius * Mathf.Cos(angle), spawnRadius * Mathf.Sin(angle), 0);
        Instantiate(enemyPrefab).transform.position = pos;
    }

    public void GameOver()
    {
        currentScore = PlayerBlue.playerCurrentPoint + PlayerRed.playerCurrentPoint;
        UICtrl.scoreboard.ShowTypeNameMenu();
        pause = true;
        Enemy[] enemyArray = FindObjectsOfType<Enemy>();
        Debug.Log("array size : " + enemyArray.Length);
        for (int i = 0; i < enemyArray.Length; i++)
        {
            enemyArray[i].Kill();
        }
    }
    #endregion

    #region Unity API
    void Start()
    {
        //var audioEvent = RuntimeManager.CreateInstance("event:/Character/TirFeu");
        //audioEvent.start();
        foreach (EnemyData enemy in enemyDataList)
        {
            timerList.Add(enemy.spawnRate);
        }
    }

    private void FixedUpdate()
    {
        if (pause) return;
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
