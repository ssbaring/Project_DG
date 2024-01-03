using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [SerializeField] private PlayerStatus playerStat;
    [SerializeField] private EnemyList enemyList;
    [SerializeField] protected float enemyHealth;
    [SerializeField] protected float enemyStun;

    

    private Rigidbody2D enemyRigid;
    private SpriteRenderer sprender;
    protected Animator enemyAnim;

    public bool isDamaged = false;

    public int isRightIntEnemy = 1;
    public float backCoefficent = 1.0f;


    protected virtual void Awake()
    {
        playerStat = FindObjectOfType<PlayerStatus>();
        sprender = GetComponent<SpriteRenderer>();
        enemyRigid = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
        enemyHealth = enemyList.enemyHP;
        enemyStun = enemyList.enemySP;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon") && !isDamaged)
        {
            enemyHealth -= playerStat.Damage();
            enemyStun -= playerStat.StunDamage();
            sprender.color = Color.red;
            HitAnimation();
            Invoke("Hit", 0.3f);
            isDamaged = true;
            Debug.Log("HP : " + enemyHealth);
            Debug.Log("Stun : " + enemyStun);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            sprender.color = Color.white;
            ResetAnimationTrigger();
            isDamaged = false;
        }
    }

    private void Hit()
    {
        sprender.color = Color.white;
    }

    private void Update()
    {
        if(enemyHealth <= 0)
        {
            gameObject.SetActive(false);
        }
        else if(enemyStun <= 0)
        {
            sprender.color = new Color(1, 1, 1, 0.2f);
            enemyRigid.bodyType = RigidbodyType2D.Static;
            GetComponent<Collider2D>().enabled = false;
            StunAnimation();
        }


    }


    private void HitAnimation()
    {
        enemyAnim.SetTrigger("Hit");
        enemyAnim.SetBool("IsIdle", false);
    }

    private void StunAnimation()
    {
        enemyAnim.SetBool("IsStun", true);
        enemyAnim.SetBool("IsIdle", false);
    }

    private void ResetAnimationTrigger()
    {
        enemyAnim.ResetTrigger("Hit");
        enemyAnim.SetBool("IsIdle", true);
    }

}
