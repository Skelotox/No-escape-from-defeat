using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }
    [SerializeField] private GameObject buttonsPanel;
    [SerializeField] private GameObject highscorePanel;
    [SerializeField] private GameObject highscorePanelBG;
    private const string HighScoreKey = "HighScores";
    public HighscoreData highscoreData;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        LoadHighscores();
        SetHighscores(highscorePanelBG);
        
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Highscores()
    {
        buttonsPanel.SetActive(false);
        highscorePanel.SetActive(true);
    }

    public void Back()
    {
        buttonsPanel.SetActive(true);
        highscorePanel.SetActive(false);
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadHighscores()
    {
        if (PlayerPrefs.HasKey(HighScoreKey))
        {
            string json = PlayerPrefs.GetString(HighScoreKey);
            highscoreData = JsonUtility.FromJson<HighscoreData>(json);
            //highScores = loadedScores.scores;
        }
        else
        {
            highscoreData = new HighscoreData();
        }
    }

    public void SetHighscores(GameObject parentObject)
    {

        for (int i = 0; i < highscoreData.arrayLimit; i++)
        {

            GameObject highscoreTextGO = new GameObject();
            highscoreTextGO.AddComponent<TextMeshProUGUI>();
            highscoreTextGO.GetComponent<TextMeshProUGUI>().text = (i + 1).ToString() + " - " + highscoreData.pseudo[i] + " - " + highscoreData.scores[i] + " - " + highscoreData.time[i];
            highscoreTextGO.GetComponent<TextMeshProUGUI>().color = new Color(0.4245283f, 0.4245283f, 0.4245283f);
            highscoreTextGO.GetComponent<TextMeshProUGUI>().enableWordWrapping = false;
            highscoreTextGO.GetComponent<TextMeshProUGUI>().font = Resources.Load<TMP_FontAsset>("Beyond Wonderland.ttf");
            highscoreTextGO.GetComponent<TextMeshProUGUI>().fontSize = 36;
            highscoreTextGO.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
            highscoreTextGO.gameObject.transform.SetParent(parentObject.transform);
            highscoreTextGO.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            highscoreTextGO.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            highscoreTextGO.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            highscoreTextGO.transform.SetSiblingIndex(i+1);
            Instantiate(highscoreTextGO);

        }
    }
}
