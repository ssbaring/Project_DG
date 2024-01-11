using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : EnemyControl
{
    [Header("EnemyStatus")]
    [SerializeField] private GameObject UIBar;
    [SerializeField] private Slider SPBar;
    [SerializeField] private Slider HPBar;

    

    protected override void Start()
    {
        base.Start();
        SPBar.maxValue = enemyList.enemySP;
        HPBar.maxValue = enemyList.enemyHP;
    }

    protected override void Update()
    {
        base.Update();
        CurrentHPBar(enemyHealth);
        CurrentSPBar(enemyStun);

        UIBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 0.8f, 0));

        if (enemyHealth <= 0)
        {
            isAlert = false;
            UIBar.SetActive(false);
            playerStat.curseStack++;
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
        else if(!isAlert && !isStun)
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
