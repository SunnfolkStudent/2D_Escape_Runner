using System;
using System.IO;
using Player;
using UnityEngine;
using UnityEditor;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public string nextScene;
    public GameObject text;
    public GameObject img;

    private Behaviour  _pauseMenu;

    private Text _text;
    private float _startTime;
    private float _stageTime;

    [SerializeField] private float timeScreenDuration = 3f;

    private void Start()
    {
        _startTime = Time.time;
        _text = text.GetComponent<Text>();
        text.SetActive(false);
        img.SetActive(false);
        _pauseMenu = GetComponentInChildren<PauseMenu>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        _stageTime = Time.time - _startTime;
        DontDestroyOnLoad(gameObject);

        UpdateScores();

        _pauseMenu.enabled = false;
        
        SceneManager.LoadScene("timeScreen");
        text.SetActive(true);
        img.SetActive(true);
        Debug.Log(_stageTime);
        _text.text = _stageTime.ToString();
        
        Invoke(nameof(GoToNextScene), timeScreenDuration);
    }

    private void UpdateScores()
    {
        var filePath = Application.persistentDataPath + "/gamedata.json";

        if (File.Exists(filePath))
        {
            var jsonText = File.ReadAllText(filePath);
            Debug.Log(jsonText);

            var data = JsonUtility.FromJson<ScoresData>(jsonText);

            if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                Debug.Log("0");
                data.Scores.Level0 = (int)_stageTime;
            }
            else if (SceneManager.GetActiveScene().name == "FirstLevel")
            {
                Debug.Log("1");
                data.Scores.Level1 = (int)_stageTime;
            }
            else if (SceneManager.GetActiveScene().name == "SecondLevel")
            {
                Debug.Log("2");
                data.Scores.Level2 = (int)_stageTime;
            }

            var updatedJson = JsonUtility.ToJson(data);
            File.WriteAllText(filePath, updatedJson);
        }
        else
        {
            Debug.Log("JSON file not found.");
        }
    }
    
    private void GoToNextScene()
    {
        SceneManager.LoadScene(nextScene);
        Destroy(gameObject);
    }
}

[System.Serializable]
public class ScoresData
{
    public ScoreLevels Scores;
}

[System.Serializable]
public class ScoreLevels
{
    public float Level0;
    public float Level1;
    public float Level2;
}