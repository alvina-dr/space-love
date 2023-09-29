using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance { get; private set; }
    public Color purple;
    public Color transparent;
    public GameObject startingButton;
    public CanvasGroup credits;
    public Scoreboard scoreboard;

    public void StartGame()
    {
        DataHolder.Instance.StartGame();
    }

    public void Credits()
    {
        if (credits.gameObject.activeSelf)
        {
            credits.DOFade(0, .3f).OnComplete(() =>
            {
                credits.gameObject.SetActive(false);
            });
        }
        else
        {
            credits.gameObject.SetActive(true);
            credits.DOFade(1, .3f);
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
        }
        else
        {
            scoreboard.ShowScoreboard();
            EventSystem.current.SetSelectedGameObject(scoreboard.mainMenuButton);
        }
    }

    #region Unity API
    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(startingButton);
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
    }
    #endregion
}
