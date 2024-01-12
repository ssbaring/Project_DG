using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private PlayerStatus playerStat;

    public GameObject Room;
    private PolygonCollider2D poly;

    [Header("Key")]
    public KeyCode respawnKey;
    public KeyCode AttackKey;
    public KeyCode JumpKey;
    public KeyCode CheckPointKey;


    public Transform respawnPoint;
    

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
        poly = FindObjectOfType<PolygonCollider2D>();
        
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

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log(poly.points[0]);
            Debug.Log(poly.points[2]);
        }
    }
}
