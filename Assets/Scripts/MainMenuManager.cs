using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Transform player;
    public CanvasGroup fadeUI;

    private void Start()
    {
        StartCoroutine(FadeIn());
        //skip levels
        PlayerPrefs.SetInt("level", 1);
    }

    private void Update()
    {
        if(player.position == new Vector3(0, -6, 0))
        {
            StartCoroutine(FadeOut());
        }
    }

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
        SceneManager.LoadScene(1);
    }
}
