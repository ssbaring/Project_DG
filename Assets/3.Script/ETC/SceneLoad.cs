using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    //[SerializeField] private string nextSceneName;
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
        StartCoroutine(LoadAsyncScene());
    }

    private IEnumerator LoadAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Stage2");
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
                case 1:
                case 2:
                case 3:
                case 4:
                    loadingText.text = "Loading";
                    break;
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    loadingText.text = "Loading.";
                    break;
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                    loadingText.text = "Loading..";
                    break;
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                    loadingText.text = "Loading...";
                    count = 0;
                    break;
                default:
                    break;
            }
            count++;
            if(asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
        loadingScreen.SetActive(false);
    }
}
