using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
        if(PlayerPrefs.GetInt("level") != 10)
        {
            LevelData.instance.thisLevel = Resources.Load<BaseLevel>(("LevelData/" + PlayerPrefs.GetInt("level").ToString()));
        }
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
        Console.instance.ConsoleLog("地牢，地下第" + PlayerPrefs.GetInt("level") + "层");
    }

    public delegate void tick();
    public tick projectileTick;
    public tick actionTick;

    public void PlayerAction()
    {
        actionTick.Invoke();
        if (projectileTick != null) projectileTick.Invoke();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(BGMusic.instance.gameObject);
            PlayerController.instance.ClearPlayerPrefs();
            StartCoroutine(FadeOutTitle());
        }
    }

    public CanvasGroup deathScreen;

    private IEnumerator FadeOutTitle()
    {
        float duration = 1f;
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            yield return null;
            fadeUI.alpha = 1f - duration;
        }

        SceneManager.LoadScene(0);
    }

    public void LoadNextLevel()
    {
        PlayerController.instance.SavePlayerPrefs();
        PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
        StartCoroutine(FadeOut());
    }

    public void PlayerDeath()
    {
        PlayerController.instance.ClearPlayerPrefs();
        StartCoroutine(FadeInDeathScreen());
    }

    private IEnumerator FadeInDeathScreen()
    {
        deathScreen.gameObject.SetActive(true);
        float duration = 1f;
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            yield return null;
            deathScreen.alpha = 1f - duration;
        }
    }

    public CanvasGroup fadeUI;

    private IEnumerator FadeIn()
    {
        float duration = 1f;
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            yield return null;
            fadeUI.alpha = duration;
        }
    }

    private IEnumerator FadeOut()
    {
        float duration = 1f;
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            yield return null;
            fadeUI.alpha = 1f - duration;
        }
        
        if(PlayerPrefs.GetInt("level") == 10)
        {
            SceneManager.LoadScene(2);
        }
        else if (PlayerPrefs.GetInt("level") == 11)
        {
            Destroy(BGMusic.instance.gameObject);
            SceneManager.LoadScene(3);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }
}
