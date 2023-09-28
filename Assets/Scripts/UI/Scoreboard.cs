using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Scoreboard : MonoBehaviour
{
    [System.Serializable]
    public class ScoreboardEntry
    {
        public string name;
        public int score;

        public ScoreboardEntry(string _name, int _score)
        {
            name = _name;
            score = _score;
        }
    }

    public class ScoreboardList
    {
        public List<ScoreboardEntry> entries = new List<ScoreboardEntry>();
    }

    private ScoreboardList scoreList = new ScoreboardList();
    public TMP_InputField inputField;
    public TextMeshProUGUI scoreText;
    public CanvasGroup typeNameMenu;
    public CanvasGroup scoreboardMenu;
    public GameObject scoreEntryPrefab;
    public Transform scoreEntryLayout;
    public GameObject mainMenuButton;

    private void Start()
    {
        typeNameMenu.alpha = 0;
        scoreboardMenu.alpha = 0;
        scoreText.transform.localScale = Vector3.zero;
        if (scoreList.entries.Count == 0)
        {
            if (PlayerPrefs.HasKey("scoreboard"))
            {
                string json = PlayerPrefs.GetString("scoreboard"); // use scoreboard-levelname
                scoreList = JsonUtility.FromJson<ScoreboardList>(json);
            }
        }

    }

    public void AddScoreButton()
    {
        AddScoreToScoreboard(inputField.text, GPSingleton.Instance.currentScore);
        typeNameMenu.gameObject.SetActive(false);
        ShowScoreboard();
    }

    public void AddScoreToScoreboard(string _name, int _score)
    {
        ScoreboardEntry entry = new(_name, _score);
        scoreList.entries.Add(entry);
        SaveScoreboard();
    }

    public void ShowTypeNameMenu()
    {
        typeNameMenu.gameObject.SetActive(true);
        typeNameMenu.DOFade(1, .3f).OnComplete(() =>
        {
            scoreText.text = GPSingleton.Instance.currentScore.ToString();
            scoreText.transform.DOScale(1.1f, .3f).OnComplete(() =>
            {
                scoreText.transform.DOScale(1f, .1f);
            });
        });
    }

    public void ShowScoreboard()
    {
        EventSystem.current.SetSelectedGameObject(mainMenuButton);
        scoreboardMenu.gameObject.SetActive(true);
        scoreboardMenu.DOFade(1, .3f);
        for (int i = 0; i < scoreList.entries.Count; i++)
        {
            InstantiateScoreboardEntry(scoreList.entries[i], i);
        }
    }

    public void SaveScoreboard()
    {
        scoreList.entries.Sort(SortByScore);
        if (scoreList.entries.Count > DataHolder.Instance.GeneralData.scoreboardSize)
            scoreList.entries = scoreList.entries.Take(DataHolder.Instance.GeneralData.scoreboardSize).ToList();
        string json = JsonUtility.ToJson(scoreList);
        PlayerPrefs.SetString("scoreboard", json);
        PlayerPrefs.Save();
    }

    public void InstantiateScoreboardEntry(ScoreboardEntry scoreboardEntry, int rank)
    {
        GameObject scoreEntry = Instantiate(scoreEntryPrefab, scoreEntryLayout);
        scoreEntry.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = rank.ToString();
        scoreEntry.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = scoreboardEntry.name;
        scoreEntry.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = scoreboardEntry.score.ToString();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    static int SortByScore(ScoreboardEntry p1, ScoreboardEntry p2)
    {
        return -p1.score.CompareTo(p2.score);
    }
}
