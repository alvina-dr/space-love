using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using FMODUnity;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Sirenix.OdinInspector;

public class DataHolder : MonoBehaviour
{
    public static DataHolder Instance = null;
    public GeneralData GeneralData;
    public FMOD.Studio.EventInstance musicEvent;
    public FMOD.Studio.EventInstance ambianceEvent;
    [Header("TRANSITION")]
    [SerializeField] private Image darkBackground;
    public Playercursor1 controlPlayer1;
    public Playercursor2 controlPlayer2;
    private bool cursor1Input = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            GeneralData = Resources.Load<GeneralData>("GeneralData");
            musicEvent = RuntimeManager.CreateInstance("event:/MX/MX");
            musicEvent.start();
            ambianceEvent = RuntimeManager.CreateInstance("event:/AMB/Cockpit");
            ambianceEvent.start();
            controlPlayer2 = new Playercursor2();
            controlPlayer2.Enable();
            controlPlayer2.Player.Fire.started += ButtonInputUI1;
            controlPlayer1 = new Playercursor1();
            controlPlayer1.Enable();
            controlPlayer1.Player.Fire.started += ButtonInputUI2;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ButtonInputUI1(InputAction.CallbackContext callbackContext)
    {
        if (EventSystem.current.currentSelectedGameObject != null)
            EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
        
    }

    public void ButtonInputUI2(InputAction.CallbackContext callbackContext)
    {
        if (cursor1Input == true || callbackContext.GetType().ToString() ==  "Playercursor1")
        {
            if (EventSystem.current.currentSelectedGameObject != null)
                EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
        }
        cursor1Input = !cursor1Input;
    }

    public void LoadGame()
    {
        //Destroy(GPCtrl.Instance.gameObject);
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        DOVirtual.DelayedCall(1.7f, () =>
        {
            darkBackground.DOFade(1, 0.3f).OnComplete(() =>
            {
                SceneManager.LoadScene("Game");
                musicEvent.setParameterByName("Layer", 0);
                ambianceEvent.setParameterByName("Layer", 0);
            });
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
            musicEvent.setParameterByName("Heart", 0);
            ambianceEvent.setParameterByName("Layer", -1);
        });
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Camera.main.orthographicSize = 3;
        darkBackground.DOFade(0, 0.6f).OnComplete(() =>
        {
            Camera.main.DOOrthoSize(5.0f, 0.3f).OnComplete(() =>
            {
                Camera.main.DOOrthoSize(4.8f, 0.3f);
            });
        });
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    [Button]
    public void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
