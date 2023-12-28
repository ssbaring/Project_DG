using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private EnemyList enemyList;
    private SpriteRenderer sprender;

    private void Awake()
    {
        //playerAttack = FindObjectOfType<PlayerAttack>();
        sprender = GetComponent<SpriteRenderer>();
        enemyList.enemyCurrentHP = enemyList.enemyMaxHP;
    }

    private void Update()
    {
        if(playerAttack.isEnemyDamaged)
        {
            enemyList.enemyCurrentHP -= playerAttack.damage;
            Debug.Log(enemyList.enemyCurrentHP);
            sprender.color = Color.red;
            playerAttack.isEnemyDamaged = false;
        }
        else
        {
            sprender.color = Color.white;
        }

        if(enemyList.enemyCurrentHP <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
