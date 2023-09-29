using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance { get; private set; }
    public Color purple;
    public Color transparent;
    public GameObject startingButton;
    public GameObject credits;
    public Scoreboard scoreboard;

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void Credits()
    {
        credits.SetActive(!credits.activeSelf);
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
