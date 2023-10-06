using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.SceneManagement;

public class DataHolder : MonoBehaviour
{
    public static DataHolder Instance = null;
    public GeneralData GeneralData;
    public FMOD.Studio.EventInstance musicEvent;
    public FMOD.Studio.EventInstance ambianceEvent;

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

    public void StartGame()
    {
        Destroy(GPCtrl.Instance.gameObject);
        SceneManager.LoadScene("Game");
        musicEvent.setParameterByName("Layer", 0);
        ambianceEvent.setParameterByName("Layer", 0);
    }
}
