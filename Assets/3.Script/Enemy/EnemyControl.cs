using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [SerializeField] private PlayerStatus playerStat;
    [SerializeField] private EnemyList enemyList;
    [SerializeField] protected float enemyHealth;
    [SerializeField] protected float enemyStun;



    private float restorationStartTime = 0;
    private Rigidbody2D enemyRigid;
    private SpriteRenderer sprender;
    protected Animator enemyAnim;

    public bool isDamaged = false;
    public bool isStun = false;

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
            playerStat.CriticalHit();
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
            playerStat.isCriticalAttack = false;
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
        if (enemyHealth <= 0)
        {
            gameObject.SetActive(false);
        }
        else if (enemyStun <= 0)
        {
            isStun = true;
            playerStat.GetPlayerEXP(enemyList.enemyEXP);
            enemyStun = 0;
        }
        else if (enemyStun >= enemyList.enemySP - 0.1f)
        {
            enemyStun = enemyList.enemySP;
            isStun = false;
        }

        if (isStun)
        {
            if (restorationStartTime < enemyList.enemyRestoration)
            {
                restorationStartTime += Time.deltaTime;
                enemyStun = Mathf.Lerp(0, enemyList.enemySP, restorationStartTime / enemyList.enemyRestoration);
                enemyHealth += Mathf.CeilToInt(enemyList.enemyHPRestoration * Time.deltaTime);
                sprender.color = new Color(sprender.color.r, sprender.color.g, sprender.color.b, 0.2f);
                enemyRigid.bodyType = RigidbodyType2D.Static;
                GetComponent<Collider2D>().enabled = false;
                StunAnimation();
            }
        }
        else
        {
            sprender.color = new Color(sprender.color.r, sprender.color.g, sprender.color.b, 1);
            enemyRigid.bodyType = RigidbodyType2D.Dynamic;
            GetComponent<Collider2D>().enabled = true;
            restorationStartTime = 0;
            IdleAnimation();
        }

    }


    private void HitAnimation()
    {
        enemyAnim.SetTrigger("Hit");
    }

    private void StunAnimation()
    {
        enemyAnim.SetBool("IsStun", true);
    }

    private void IdleAnimation()
    {
        enemyAnim.SetBool("IsStun", false);
    }

    private void ResetAnimationTrigger()
    {
        enemyAnim.ResetTrigger("Hit");
    }

}
