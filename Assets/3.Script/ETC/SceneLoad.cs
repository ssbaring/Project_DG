using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    public static SceneLoad instance = null;

    private void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
    }

    public string nextSceneName;
    public int sceneIndex = 0;

    public bool isGoal;

    public GameObject loadingScreen;
    public TextMeshProUGUI loadingText;
    public TextMeshProUGUI loadingPercentageText;


    private void Update()
    {
        if (isGoal)
        {
            isGoal = false;
            LoadNextScene();
        }
    }

    public void LoadNextScene()
    {
        //Debug.Log("¾À ÀüÈ¯");
        StartCoroutine(LoadAsyncScene());
    }

    private IEnumerator LoadAsyncScene()
    {
        AsyncOperation asyncLoad;
        if (SceneManager.GetActiveScene().buildIndex == sceneIndex)
        {
            sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        }
        else
        {
            asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        }
        asyncLoad.allowSceneActivation = false;
        loadingScreen.SetActive(true);
        float gameTime = 0;
        float loadingPercentage = 0;
        int count = 0;

        while (!asyncLoad.isDone)
        {
            yield return null;
            gameTime += Time.deltaTime;
            //loadingPercentage = Mathf.RoundToInt(asyncLoad.progress * 100);

            #region Loading Text
            switch (count)
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
            #endregion

            if (loadingPercentage >= 90)
            {
                loadingPercentage = Mathf.Lerp(loadingPercentage, 100, gameTime);
                if (loadingPercentage == 100)
                {
                    asyncLoad.allowSceneActivation = true;
                }
            }
            else
            {
                loadingPercentage = Mathf.Lerp(loadingPercentage, asyncLoad.progress * 100, gameTime);
                if (loadingPercentage >= 90) gameTime = 0;
            }
            loadingPercentageText.text = string.Format("{0} %", Mathf.RoundToInt(loadingPercentage));

        }
        loadingScreen.SetActive(false);
    }
}
