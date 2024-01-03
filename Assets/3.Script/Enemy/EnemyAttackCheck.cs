using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackCheck : MonoBehaviour
{
    public LayerMask PlayerLayer;

    [SerializeField] private GameObject backAttack;
    [SerializeField] private float backAttackrayLength = 0.5f;

    private PlayerStatus attackCheck;
    private EnemyControl eCon;
    private void Start()
    {
        eCon = GetComponentInParent<EnemyControl>();
        attackCheck = FindObjectOfType<PlayerStatus>();
    }

    private void Update()
    {
        attackCheck.isBackAttack = Physics2D.Raycast(backAttack.transform.position, Vector2.left * eCon.isRightIntEnemy, backAttackrayLength, PlayerLayer);
        Debug.DrawRay(backAttack.transform.position, Vector2.left * eCon.isRightIntEnemy * backAttackrayLength, Color.white);
    }
}
