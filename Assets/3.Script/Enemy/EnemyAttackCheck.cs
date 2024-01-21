using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackCheck : MonoBehaviour
{
    private PlayerStatus attackCheck;
    private EnemyControl eCon;
    private void Start()
    {
        eCon = GetComponentInParent<EnemyControl>();
        attackCheck = FindObjectOfType<PlayerStatus>();
    }

    private void Update()
    {
        if(eCon.IsRightIntEnemy * attackCheck.isRightInt == 1)
        {
            eCon.isBackAttack = true;
        }
        else
        {
            eCon.isBackAttack = false;
        }
    }
}
