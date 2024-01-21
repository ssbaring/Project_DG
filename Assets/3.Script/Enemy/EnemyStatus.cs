using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : EnemyControl
{
    [Header("EnemyStatus")]
    [SerializeField] private GameObject UIBar;
    [SerializeField] private GameObject damageTextObject;
    [SerializeField] private GameObject damageText;
    [SerializeField] private GameObject stunDamageTextObject;
    [SerializeField] private GameObject stunDamageText;

    [SerializeField] private Slider SPBar;
    [SerializeField] private Slider HPBar;
    public bool isEnemyDead;

    [SerializeField] private GameObject _HUDCanvas;
    protected override void Start()
    {
        base.Start();
        _HUDCanvas = FindObjectOfType<HUDCanvas>().gameObject;
        UIBar = Instantiate(UIBar, _HUDCanvas.transform);
        damageTextObject = Instantiate(damageTextObject, _HUDCanvas.transform);
        stunDamageTextObject = Instantiate(stunDamageTextObject, _HUDCanvas.transform);
        SPBar = UIBar.transform.GetChild(0).GetComponent<Slider>();
        HPBar = UIBar.transform.GetChild(1).GetComponent<Slider>();
        SPBar.maxValue = enemyList.enemySP;
        HPBar.maxValue = enemyList.enemyHP;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (damageTextObject.transform.childCount > 1 && stunDamageTextObject.transform.childCount > 1)
        {
            Destroy(damageTextObject.transform.GetChild(0).gameObject);
            Destroy(stunDamageTextObject.transform.GetChild(0).gameObject);
        }
            Instantiate(damageText, damageTextObject.transform);
            Instantiate(stunDamageText, stunDamageTextObject.transform);
    }

    protected override void Update()
    {
        base.Update();
        CurrentHPBar(enemyHealth);
        CurrentSPBar(enemyStun);

        UIBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 0.8f, 0));

        if (damageTextObject != null)
        {
            stunDamageTextObject.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.2f, 0));
            damageTextObject.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0.5f, 0.3f, 0));
        }

        //적 체력이나 기절수치에 따른 상태변화
        if (enemyHealth <= 0)
        {
            isAlert = false;
            isEnemyDead = true;


            UIBar.SetActive(false);
            playerStat.DeadEnemyList.Add(gameObject);
            playerStat.CurseStack++;
            gameObject.SetActive(false);
        }
        else if (enemyStun <= 0)
        {
            isStun = true;
            isAlert = false;
            playerStat.GetPlayerEXP(enemyList.enemyEXP);
            enemyStun = 0;
        }
        else if (enemyStun >= enemyList.enemySP - 0.1f)
        {
            enemyStun = enemyList.enemySP;
            isStun = false;
        }




        //10초동안 적 체력바 표시
        if (isAlert)
        {
            alertTimer += Time.deltaTime;
            UIBar.SetActive(true);
            if (alertTimer >= 10.0f)
            {
                isAlert = false;
                alertTimer = 0;
            }
        }
        else if (!isAlert && !isStun)
        {
            UIBar.SetActive(false);
        }
    }



    private void CurrentHPBar(float HP)
    {
        HPBar.value = HP;
    }

    private void CurrentSPBar(float SP)
    {
        SPBar.value = SP;
    }



}
