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
using Unity.VisualScripting;

public class DataHolder : MonoBehaviour
{
    public static DataHolder Instance = null;
    public GeneralData GeneralData;
    public FMOD.Studio.EventInstance musicEvent;
    public FMOD.Studio.EventInstance ambianceEvent;
    [Header("TRANSITION")]
    [SerializeField] private Image darkBackground;
    public IA_PlayerCursor controlPlayer;

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

            controlPlayer = new IA_PlayerCursor();
            controlPlayer.Enable();
            controlPlayer.Player.Fire.performed += OnClickButton;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnClickButton(InputAction.CallbackContext callbackContext)
    {
        if (EventSystem.current.currentSelectedGameObject != null)
            EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
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
}
