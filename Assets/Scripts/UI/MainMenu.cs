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
    public CanvasGroup credits;
    public Scoreboard scoreboard;
    public SerialController serialControler;
    public List<char> currentInput;

    public void StartGame()
    {
        DataHolder.Instance.LoadGame();
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

    private void Update()
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
            EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();

        if (currentInput.Contains('B'))
            EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
    }
    #endregion
}
