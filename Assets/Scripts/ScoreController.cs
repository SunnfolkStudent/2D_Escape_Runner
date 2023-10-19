using System.Globalization;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private Text scoreLevel0;
    [SerializeField] private Text scoreLevel1;
    [SerializeField] private Text scoreLevel2;

    private void Awake()
    {
        var filePath = Path.Combine(Application.persistentDataPath, "gamedata.json");

        if (File.Exists(filePath))
        {
            var jsonText = File.ReadAllText(filePath);
            var data = JsonUtility.FromJson<ScoresData>(jsonText);

            Debug.Log("Level0: " + data.Scores.Level0);
            Debug.Log("Level1: " + data.Scores.Level1);
            Debug.Log("Level2: " + data.Scores.Level2);

            scoreLevel0.text = data.Scores.Level0.ToString(CultureInfo.InvariantCulture);
            scoreLevel1.text = data.Scores.Level1.ToString(CultureInfo.InvariantCulture);
            scoreLevel2.text = data.Scores.Level2.ToString(CultureInfo.InvariantCulture);
        }
        else
        {
            Debug.Log("JSON file not found.");
        }
    }
}