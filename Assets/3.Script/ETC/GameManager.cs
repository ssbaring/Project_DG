using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public KeyCode InteractionKey;


    public Transform respawnPoint;


    [Header("DevMode")]
    [SerializeField] private PlayerSkillStatus playerSkill;
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!SceneManager.GetActiveScene().name.Equals("TitleScene") && !SceneManager.GetActiveScene().name.Equals("EndScene"))
        {
            respawnPoint = FindObjectOfType<Respawn>().GetComponent<Transform>();
        }
        else return;
    }


    private void Update()
    {
        DevMode();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void DevMode()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
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
