using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDCanvas : MonoBehaviour
{
    public static HUDCanvas instance = null;
    private void Awake()
    {
        #region SingleTon
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
}
