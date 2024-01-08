using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private PlayerStatus playerStat;

    [Header("DevMode")]
    [SerializeField] private int exp = 10000;

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

        playerStat = FindObjectOfType<PlayerStatus>();
    }

    private void Update()
    {
        DevMode();
    }

    private void DevMode()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            playerStat.GetPlayerEXP(exp);
        }
    }
}
