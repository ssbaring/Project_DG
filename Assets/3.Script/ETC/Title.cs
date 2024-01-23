using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void GameStart()
    {
        SceneLoad.instance.LoadNextScene();
    }

    public void GoTitle()
    {
        SceneManager.LoadScene("TitleScene");
        SceneLoad.instance.sceneIndex = 0;
    }
}
