using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider progressBar;
    public TextMeshProUGUI loading_text;
    public TextMeshProUGUI percentage_text;
    public string sceneToLoad;
    public CanvasGroup canvasGroup;

    public void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("loadingScreen");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void StartGame()
    {
        StartCoroutine(StartLoad());
    }


    IEnumerator StartLoad()
    {
        loadingScreen.SetActive(true);
        yield return StartCoroutine(FadeLoadingScreen(1, 1));

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);

        StartCoroutine(LoadingTextAnimation(operation));
        StartCoroutine(ProgressBar(operation));
        
        while (!operation.isDone)
        {
            yield return null;
        }

        yield return StartCoroutine(FadeLoadingScreen(0, 1));
        progressBar.value = 0;
        percentage_text.text = "0%";
        loading_text.text = "Loading";
        loadingScreen.SetActive(false);
        StopAllCoroutines();
    }

    IEnumerator FadeLoadingScreen(float targetValue, float duration)
    {
        float startValue = canvasGroup.alpha;
        float time = 0;

        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startValue, targetValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = targetValue;
    }
    IEnumerator ProgressBar(AsyncOperation loadingOperation)
    {
        while (!loadingOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadingOperation.progress / 0.9f);
            percentage_text.text = Mathf.Round(progressValue * 100) + "%";
            progressBar.value = progressValue;
            yield return null;
        }
    }
    
    IEnumerator LoadingTextAnimation(AsyncOperation loadingOperation)
    {
        int counter = 0;
        while (!loadingOperation.isDone)
        {
            if (counter == 3)
            {
                loading_text.text = "Loading.";
                counter = 0;
            }
            else
            {
                counter++;
                loading_text.text += ".";
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}
