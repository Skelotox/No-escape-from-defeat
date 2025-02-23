using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private const string HighScoreKey = "HighScores";
    //public List<GameObject> fireBalls= new List<GameObject>();
    [Header("Objects")]
    public GameObject FireBallPrefab;
    public HighscoreData highscoreData;
    public GameObject highscoreTextPrefab;

    [Header("UI")]
    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseBG;
    [SerializeField] private GameObject buttonsPanel;
    [SerializeField] private GameObject highcoresPanel;
    [SerializeField] private GameObject defeatHighcoresPanel;
    [SerializeField] private GameObject newHighcoresPanel;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI stageNameText;
    [SerializeField] private TextMeshProUGUI defeatScore;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TMP_InputField pseudoInput;

    [Header("Variables")]
    private int minutes = 0;
    private float secondes = 0;
    public float spawnRatio = 0.4f;
    private bool halfMinute = true;
    private string[] stageNames;
    private int stageNamesIndex = 0;
    private int newHighscoreIndex = 0;
    private string pseudo;
    public int minFb = 8;
    public int maxFb = 15;
    public float fireBallVelocity = 4.0f;
    public int fireBallCounter = 0;
    private const string FileName = "highscores.json";
    private string SaveFolderPath => Path.Combine(Application.persistentDataPath, "Saves");
    private string FilePath => Path.Combine(SaveFolderPath, FileName);


    private void Awake()
    {
        Time.timeScale = 1;
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
        stageNames = new string[]
        {
            "Fire balls?",
            "Daily life...",
            "Isn't it hot?",
            "Nightmare",
            "Hell incoming",
            "Apocalypse !",
            "How the **** did you get there??"

        };
        stageNameText.text = stageNames[0];
        stageNamesIndex++;
        LoadHighscores();
        SetHighscores(defeatHighcoresPanel);
        SetHighscores(highcoresPanel);
    }

    private void Update()
    {
        secondes += Time.deltaTime;
        if(secondes >= 30f && halfMinute)
        {
            IncreaseDiff();
            halfMinute = false;
        }
        if (secondes >= 60f) 
        {
            minutes += 1;
            IncreaseDiff();
            secondes = 0;
            halfMinute = true;
        }
        score.text = fireBallCounter.ToString();
        timer.text = string.Format("{0:00}:{1:00}", minutes, secondes);

        if (Input.GetKeyUp(KeyCode.Escape) && !pausePanel.activeInHierarchy)
        {
            PauseGame();
        }
        else if(Input.GetKeyUp(KeyCode.Escape) && pausePanel.activeInHierarchy)
        {
            ResumeGame();
        }
    }

    private void IncreaseDiff()
    {
        fireBallVelocity *= 1.1f;
        spawnRatio *= 1.5f;
        maxFb = (int)Mathf.Ceil(maxFb * 1.5f);
        if (stageNamesIndex < stageNames.Count() - 1)
        {
            stageNameText.text = stageNames[stageNamesIndex];
            stageNamesIndex++;
        }
    }

    public void IncreaseScore()
    {
        fireBallCounter++;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        CloseHighscore();
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void ShowHighscores()
    {
        buttonsPanel.SetActive(false);
        highcoresPanel.SetActive(true);
    }


    public void SetHighscores(GameObject parentObject)
    {

        for (int i = 0; i < highscoreData.arrayLimit; i++)
        {
            
            GameObject highscoreTextGO = new GameObject();
            highscoreTextGO.AddComponent<TextMeshProUGUI>();
            highscoreTextGO.GetComponent<TextMeshProUGUI>().text = (i + 1).ToString() + " - " + highscoreData.pseudo[i] + " - " + highscoreData.scores[i] + " - " + highscoreData.time[i];
            highscoreTextGO.GetComponent<TextMeshProUGUI>().color = new Color(0.2196079f, 0.5215687f, 0.5843138f);
            highscoreTextGO.GetComponent<TextMeshProUGUI>().enableWordWrapping= false;
            highscoreTextGO.GetComponent<TextMeshProUGUI>().font = Resources.Load<TMP_FontAsset>("Beyond Wonderland.ttf");
            highscoreTextGO.GetComponent<TextMeshProUGUI>().fontSize = 40;
            highscoreTextGO.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
            highscoreTextGO.gameObject.transform.SetParent(parentObject.transform);
            highscoreTextGO.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            highscoreTextGO.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            highscoreTextGO.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            Instantiate(highscoreTextGO);

        }
    }

    public bool CheckNewHighscore()
    {

        for (int i = 0; i < highscoreData.arrayLimit; i++)
        {
            if (fireBallCounter > int.Parse(highscoreData.scores[i]))
            {
                newHighscoreIndex = i;
                return true;
            }

            i++;
        }
        return false;

    }

    public void CloseHighscore()
    {
        highcoresPanel.SetActive(false);
        buttonsPanel.SetActive(true);
    }

    public void Defeat()
    {
        Time.timeScale = 0;
        defeatScore.text = string.Format("{0:00}:{1:00}", minutes, secondes) + " - " + fireBallCounter.ToString();
        defeatPanel.SetActive(true);
        
        if (CheckNewHighscore())
        {
            newHighcoresPanel.SetActive(true);
        }
    }

    public void SetPseudo()
    {
        this.pseudo= pseudoInput.text;
    }

    public void SaveNewHighscore()
    {
        
        HighscoreData tempHighscore = highscoreData.DeepCopy(); //Can't copy without function cause it copies the reference of the object
        highscoreData.pseudo[newHighscoreIndex] = pseudo;
        highscoreData.scores[newHighscoreIndex] = fireBallCounter.ToString();
        highscoreData.time[newHighscoreIndex] = string.Format("{0:00}:{1:00}", minutes, secondes);

         for (int i = newHighscoreIndex+1; i < tempHighscore.arrayLimit; i++)
         {
             highscoreData.pseudo[i] = tempHighscore.pseudo[i-1];
             highscoreData.scores[i] = tempHighscore.scores[i - 1];
             highscoreData.time[i] = tempHighscore.time[i - 1];
         }
        SaveHighscore(highscoreData);
        newHighcoresPanel.SetActive(false);
        


    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void Replay()
    {
        SceneManager.LoadScene(1);
    }


    /*
     * Load() and Save() functions for app on itch.io which save data through PlayersPrefs
     * Can't do it shared by all users cause of itch.io writing permissions
     */

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


    public void SaveHighscore(HighscoreData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(HighScoreKey, json);
        PlayerPrefs.Save();

    }


    /*
     * Load() and Save() functions if the app is running directly on player's PC
     */

    /*
    public void LoadHighscoresLocalApp()
    {
        if (File.Exists(FilePath))
        {
            string json = File.ReadAllText(FilePath);
            highscoreData = JsonUtility.FromJson<HighscoreData>(json);
        }
        else
        {
            highscoreData= new HighscoreData();
            Debug.Log("No Highscore to load");
        }
    }

    public void SaveHighscoreLocalApp(HighscoreData data)
    {
        if (!Directory.Exists(SaveFolderPath))
        {
            Directory.CreateDirectory(SaveFolderPath);
        }
        string json = JsonUtility.ToJson(highscoreData, true);
        File.WriteAllText(FilePath, json);
        Debug.Log("Highscores saved in : " + FilePath);

    }
    */
}
