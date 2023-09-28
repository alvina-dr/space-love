using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class DataHolder : MonoBehaviour
{
    public static DataHolder Instance = null;
    public GeneralData GeneralData;

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
        var audioEvent = RuntimeManager.CreateInstance("event:/MX/MX");
        audioEvent.start();
    }
}
