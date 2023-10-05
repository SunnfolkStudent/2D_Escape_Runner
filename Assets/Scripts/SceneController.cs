using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class SceneController : MonoBehaviour
{
    public Object timeScreen;
    public Object nextScene;
    public GameObject canvas;

    private Text _text;
    private float _startTime;
    private float _stageTime;

    private void Start()
    {
        _startTime = Time.time;
        _text = canvas.GetComponent<Text>();
        canvas.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        _stageTime = Time.time - _startTime;
        DontDestroyOnLoad(gameObject);
        
        SceneManager.LoadScene(timeScreen.name);
        canvas.SetActive(true);
        Debug.Log(_stageTime);
        _text.text = _stageTime.ToString();
        
        Invoke(nameof(GoToNextScene), 5f);
    }
    
    private void GoToNextScene()
    {
        SceneManager.LoadScene(nextScene.name);
        Destroy(gameObject);
    }
}
