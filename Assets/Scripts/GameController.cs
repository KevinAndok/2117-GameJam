using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    public static float roundTimer = 0;
    public static float fadeSpeed = 3f;
    public float _fadeSpeed = 3f;
    public float _roundMaxTime = 50;

    public Text timerText;

    public delegate void StartTimer();
    public static StartTimer startTimer;

    static bool levelFinished;

    private void Awake()
    {
        levelFinished = false;
    }

    private void Start()
    {
        startTimer += () => StartTimerFunc();
        fadeSpeed = _fadeSpeed;
        FadeIn();
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
            timerText.text = Mathf.Clamp((_roundMaxTime - roundTimer), 0, 100).ToString();
        }
        if (!levelFinished) 
            LevelFailed();
    }

    public static void FadeIn()//1->0
    {
        float f = 1;
        DOTween.To(() => f, x => f = x, 0, fadeSpeed).OnUpdate(() => Fades.fadeOutCounter = f);
    }
    public static void FadeOut()//0->1
    {
        float f = 0;
        DOTween.To(() => f, x => f = x, 1, fadeSpeed).OnUpdate(() => Fades.fadeOutCounter = f);
    }

    public static void LevelComplete()
    {
        levelFinished = true;
        //fade out
        FadeOut();
        //tween in level complete
        //load next level
        SceneManager.LoadScene(Mathf.Clamp(SceneManager.GetActiveScene().buildIndex + 1, 0, SceneManager.sceneCountInBuildSettings - 1));
    }

    public static void LevelFailed()
    {
        levelFinished = true;
        //fade Out
        FadeOut();
        //restart level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
