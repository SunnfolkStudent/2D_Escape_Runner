using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class SceneController : MonoBehaviour
{
    public string nextScene;
    public GameObject text;
    public GameObject img;

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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        _stageTime = Time.time - _startTime;
        DontDestroyOnLoad(gameObject);
        
        SceneManager.LoadScene("timeScreen");
        text.SetActive(true);
        img.SetActive(true);
        Debug.Log(_stageTime);
        _text.text = _stageTime.ToString();
        
        Invoke(nameof(GoToNextScene), timeScreenDuration);
    }
    
    private void GoToNextScene()
    {
        SceneManager.LoadScene(nextScene);
        Destroy(gameObject);
    }
}
