using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance { get; private set; }
    public Color purple;
    public Color transparent;
    public GameObject startingButton;
    [Header("CREDITS")]
    public GameObject creditsButton;
    public GameObject creditsBackButton;
    public CanvasGroup credits;
    [Header("SCOREBOARD")]
    public GameObject scoreboardButton;
    public GameObject scoreboardBackButton;
    public Scoreboard scoreboard;
    public SerialController serialControler;
    public List<char> currentInput;
    [Header("FX")]
    public Transform warpdriveFX;

    public void StartGame()
    {
        DataHolder.Instance.LoadGame();
        warpdriveFX.DOScale(1.17f, 1f);
    }

    public void Credits()
    {
        if (credits.gameObject.activeSelf)
        {
            credits.DOFade(0, .3f).OnComplete(() =>
            {
                EventSystem.current.SetSelectedGameObject(creditsButton);
                credits.gameObject.SetActive(false);
            });
        }
        else
        {
            credits.gameObject.SetActive(true);
            credits.DOFade(1, .3f).OnComplete(() => {
                EventSystem.current.SetSelectedGameObject(creditsBackButton);
            });
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Scoreboard()
    {
        if (scoreboard.scoreboardMenu.gameObject.activeSelf)
        {
            scoreboard.HideScoreboard();
            EventSystem.current.SetSelectedGameObject(scoreboardButton);
        }
        else
        {
            scoreboard.ShowScoreboard();
            //EventSystem.current.SetSelectedGameObject(scoreboardBackButton);
        }
    }

    #region Unity API
    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(startingButton);
        DOVirtual.DelayedCall(.1f, () =>
        {
            EventSystem.current.SetSelectedGameObject(startingButton);
        });
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
        if (!DataHolder.Instance.GeneralData.externalDevice)
        {
            serialControler.enabled = false;
        }

    }

    private void Update()
    {
        if (DataHolder.Instance.GeneralData.externalDevice)
        {
            currentInput.Clear();
        }
    }
    #endregion
}
