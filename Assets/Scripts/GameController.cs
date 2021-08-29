using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public float roundTimer = 0;
    public float fadeSpeed = 3f;
    public float _fadeSpeed = 3f;
    public float _roundMaxTime = 50;

    public Text timerText;
    public Transform levelCompleteText;
    public GameObject pauseMenu;

    public delegate void StartTimer();
    public StartTimer startTimer;

    static bool levelFinished;

    private void Awake()
    {
        Setup();

        startTimer += () => StartTimerFunc();
    }

    private void Start()
    {
        fadeSpeed = _fadeSpeed;
        FadeIn();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) LevelFailed();
        if (Input.GetKeyDown(KeyCode.Escape)) pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
    }

    void Setup()
    {
        levelFinished = false;
        roundTimer = 0;

        if (!instance) instance = this;
        else if (instance != this)
        {
            instance._roundMaxTime = _roundMaxTime;
            Destroy(this);
        }
    }

    public void StartTimerFunc()
    {
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        while (roundTimer < _roundMaxTime)
        {
            yield return new WaitForFixedUpdate();
            roundTimer += Time.fixedDeltaTime;
            timerText.text = Mathf.Clamp((int)(_roundMaxTime - roundTimer), 0, 100).ToString();
        }
        if (!levelFinished)
            LevelFailed();
    }

    public static void FadeIn() //1->0
    {
        float f = 1;
        DOTween.To(() => f, x => f = x, 0, instance.fadeSpeed).OnUpdate(() => Fades.fadeOutCounter = f);
    }
    public static void FadeOut() //0->1
    {
        float f = 0;
        DOTween.To(() => f, x => f = x, 1, instance.fadeSpeed).OnUpdate(() => Fades.fadeOutCounter = f);
    }

    public static void LevelComplete()
    {
        levelFinished = true;
        //fade out
        FadeOut();
        //tween in level complete
        instance.levelCompleteText.gameObject.SetActive(true);
        instance.levelCompleteText.DOScale(1, 1).From(0).OnComplete(() => instance.levelCompleteText.DOScale(0, .5f).From(1)).OnComplete(() => SceneManager.LoadScene(Mathf.Clamp(SceneManager.GetActiveScene().buildIndex + 1, 0, SceneManager.sceneCountInBuildSettings - 1)));
        //int i = 0;
        //DOTween.To(() => i, x => i = x, 1, 1).OnUpdate(() => instance.levelCompleteText.fontSize = i);
        //load next level
    }

    public static void LevelFailed()
    {
        levelFinished = true;
        //fade Out
        FadeOut();
        //restart level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
