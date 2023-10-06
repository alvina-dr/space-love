using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.VFX;
using System;
using System.Linq;
using DG.Tweening;
public class GPCtrl : MonoBehaviour
{
    #region Properties
    public static GPCtrl Instance { get; private set; }
    public enum SpawnerMode
    {
        Game = 0,
        Menu = 1
    }

    public SpawnerMode spawnerMode;

    [Header("REFERENCES")]
    public PlanetBehavior Planet;
    public PlayerCursor PlayerRed;
    public PlayerCursor PlayerBlue;
    public UICtrl UICtrl;
    public SerialController serialControler;
    public List<char> currentInput;

    [SerializeField] private List<EnemyData> enemyDataList = new List<EnemyData>();
    [HideInInspector] public bool pause = false;

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

    [Header("SCORE")]
    public int currentScore;
    public int currentGameStage = 0;

    [Header("IN GAME")]
    [HideInInspector] public bool loveFrenzy = false;
    private float loveFrenzyTimer;
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

    public void SetShaderColor(Renderer _renderer, EnemyData.Color _color)
    {
        switch (_color)
        {
            case EnemyData.Color.White:
                _renderer.material.SetColor("_color",  visibleAll);
                break;
            case EnemyData.Color.Red:
                _renderer.material.SetColor("_color", visibleRed);
                break;
            case EnemyData.Color.Blue:
                _renderer.material.SetColor("_color", visibleBlue);
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

    public void SetVFX(TrailRenderer _renderer, EnemyData.Color _color)
    {
        switch (_color)
        {
            case EnemyData.Color.White:
                _renderer.colorGradient = visibleGradientAll;
                break;
            case EnemyData.Color.Red:
                _renderer.colorGradient = visibleGradientRed;
                break;
            case EnemyData.Color.Blue:
                _renderer.colorGradient = visibleGradientBlue;
                break;
        }
    }

    void SpawnEnemy(Enemy enemyPrefab, float _angle = 0, float _radiusBonus = 0, bool center = false)
    {
        if (_angle == 0) _angle = UnityEngine.Random.Range(0f, 2.0f * Mathf.PI);
        Vector3 pos = new Vector3((spawnRadius + _radiusBonus)* Mathf.Cos(_angle), (spawnRadius + _radiusBonus ) * Mathf.Sin(_angle), 0);
        if (center) pos = Vector3.zero;
        Instantiate(enemyPrefab).transform.position = pos;
    }

    public void StartLoveFrenzy()
    {
        loveFrenzyTimer = DataHolder.Instance.GeneralData.loveFrenzyDuration;
        loveFrenzy = true;
        Enemy[] enemyArray = FindObjectsOfType<Enemy>();
        for (int i = 0; i < enemyArray.Length; i++)
        {
            enemyArray[i].ChangeColor(EnemyData.Color.White);
        }
    }

    public void EndLoveFrenzy()
    {
        loveFrenzy = false;
        Enemy[] enemyArray = FindObjectsOfType<Enemy>();
        for (int i = 0; i < enemyArray.Length; i++)
        {
            enemyArray[i].ChangeColor(enemyArray[i].currentColor);
        }
    }

    public void GameOver()
    {
        currentScore = PlayerBlue.playerCurrentPoint + PlayerRed.playerCurrentPoint;
        pause = true;
        Enemy[] enemyArray = FindObjectsOfType<Enemy>();
        for (int i = 0; i < enemyArray.Length; i++)
        {
            enemyArray[i].Kill();
        }
        DataHolder.Instance.musicEvent.setParameterByName("Layer", -1);
        DOVirtual.DelayedCall(1f, () => UICtrl.scoreboard.ShowTypeNameMenu());
    }
    #endregion

    #region Unity API


    private void FixedUpdate()
    {
        if (pause) return;

        float timeSinceStart = Time.time - startTime;

        if (loveFrenzy) loveFrenzyTimer -= Time.deltaTime;
        if (loveFrenzyTimer <= 0) EndLoveFrenzy();

        if (spawnerMode == SpawnerMode.Game)
        {
            currentInput.Clear();
            string input;
            while ((input = serialControler.ReadSerialMessage()) != null)
                if (ReferenceEquals(input, SerialController.SERIAL_DEVICE_CONNECTED))
                    Debug.Log("Connection established");
                else if (ReferenceEquals(input, SerialController.SERIAL_DEVICE_DISCONNECTED))
                    Debug.Log("Connection attempt failed or disconnection detected");
                else
                {
                    currentInput.AddRange(input);
                }

            if (currentInput.Contains('R'))
                PlayerRed.Shoot();

            if (currentInput.Contains('B'))
                PlayerBlue.Shoot();

            if (timeSinceStart >= 180) DataHolder.Instance.musicEvent.setParameterByName("Layer", 3);
            else if (timeSinceStart >= 120) DataHolder.Instance.musicEvent.setParameterByName("Layer", 2);
            else if (timeSinceStart >= 60)
            {
                DataHolder.Instance.musicEvent.setParameterByName("Layer", 1);
            }

        }

        if (timeSinceStart > DataHolder.Instance.GeneralData.gameStage[currentGameStage])
        {
            if (DataHolder.Instance.GeneralData.gameStage.Count > currentGameStage + 1)
            {
                for (int i = 0; i < enemyDataList.Count; i++)
                {
                    enemyDataList[i].spawnTime *= DataHolder.Instance.GeneralData.timeRateReduction;
                }
                currentGameStage++;
            }
        }
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
                if (enemyDataList[i].name == "Standard")
                {
                    float _angle = UnityEngine.Random.Range(0f, 2.0f * Mathf.PI);
                    SpawnEnemy(enemyDataList[i].enemyPrefab, _angle, 1);
                    SpawnEnemy(enemyDataList[i].enemyPrefab, _angle + 0.1f);
                    SpawnEnemy(enemyDataList[i].enemyPrefab, _angle + 0.2f, 2);
                } else if (enemyDataList[i].name == "LoveFrenzy")
                {
                    SpawnEnemy(enemyDataList[i].enemyPrefab, center:true);
                } else
                {
                    SpawnEnemy(enemyDataList[i].enemyPrefab);
                }
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
        startTime = Time.time;
        //if (DataHolder.Instance.GeneralData.computerMode)
        //{
        //    PlayerRed.playerInput.SwitchCurrentControlScheme(UnityEngine.InputSystem.Keyboard.current);
        //    PlayerBlue.playerInput.SwitchCurrentControlScheme(UnityEngine.InputSystem.Keyboard.current);
        //}
    }

    void Start()
    {
        serialControler = GetComponent<SerialController>();
        foreach (EnemyData enemy in enemyDataList)
        {
            timerList.Add(enemy.spawnRate);
        }
    }
    #endregion
}
