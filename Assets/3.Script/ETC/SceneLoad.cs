using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    private string nextSceneName;
    private bool isGoal;

    public GameObject loadingScreen;
    public TextMeshProUGUI loadingText;
    public TextMeshProUGUI loadingPercentageText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isGoal = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isGoal = false;
        }
    }

    private void Update()
    {
        if(isGoal)
        {
            isGoal = false;
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        //Debug.Log("¾À ÀüÈ¯");
        nextSceneName = "Stage2";
        StartCoroutine(LoadAsyncScene());
    }

    private IEnumerator LoadAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
        asyncLoad.allowSceneActivation = false;
        loadingScreen.SetActive(true);
        float loadingProgress = asyncLoad.progress;
        int loadingPercentage = 0;
        int count = 0;

        while(!asyncLoad.isDone)
        {
            Debug.Log(loadingProgress);
            loadingPercentage = Mathf.RoundToInt(loadingProgress * 100);
            loadingPercentageText.text = string.Format("{0} %", loadingPercentage);
            switch(count)
            {
                case 0:
                    loadingText.text = "Loading";
                    break;
                case 1:
                    loadingText.text = "Loading.";
                    break;
                case 2:
                    loadingText.text = "Loading..";
                    break;
                case 3:
                    loadingText.text = "Loading...";
                    count = 0;
                    break;
                default:
                    break;
            }
            count++;
            yield return null;
        }
        loadingScreen.SetActive(false);
    }
}
