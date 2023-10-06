using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class DataHolder : MonoBehaviour
{
    public static DataHolder Instance = null;
    public GeneralData GeneralData;
    public FMOD.Studio.EventInstance musicEvent;
    public FMOD.Studio.EventInstance ambianceEvent;
    [Header("TRANSITION")]
    [SerializeField] private Image darkBackground;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        GeneralData = Resources.Load<GeneralData>("GeneralData");
        musicEvent = RuntimeManager.CreateInstance("event:/MX/MX");
        musicEvent.start();
        ambianceEvent = RuntimeManager.CreateInstance("event:/AMB/Cockpit");
        ambianceEvent.start();
    }

    public void LoadGame()
    {
        //Destroy(GPCtrl.Instance.gameObject);
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        darkBackground.DOFade(1, .3f).OnComplete(() =>
        {
            SceneManager.LoadScene("Game");
            musicEvent.setParameterByName("Layer", 0);
            ambianceEvent.setParameterByName("Layer", 0);
        });
    }

    public void LoadMenu()
    {
        //Destroy(GPCtrl.Instance.gameObject);
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        darkBackground.DOFade(1, .3f).OnComplete(() =>
        {
            SceneManager.LoadScene("Menu");
            musicEvent.setParameterByName("Layer", -1);
            ambianceEvent.setParameterByName("Layer", -1);
        });
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        darkBackground.DOFade(0, .3f);
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
}
