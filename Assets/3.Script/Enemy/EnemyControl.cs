using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{

    [SerializeField] protected float alertTimer = 0;
    [SerializeField] protected EnemyList enemyList;
    [SerializeField] protected float enemyStun;
    [SerializeField] protected float enemyHealth;

    public float EnemyStun
    {
        get
        {
            return enemyStun;
        }
        set
        {
            enemyStun = value;
        }
    }

    public float EnemyHealth
    {
        get
        {
            return enemyHealth;
        }
        set
        {
            enemyHealth = value;
        }
    }


    protected float restorationStartTime = 0;
    protected Rigidbody2D enemyRigid;
    protected SpriteRenderer sprender;
    protected PlayerStatus playerStat;
    protected Animator enemyAnim;

    public bool isDamaged = false;
    public bool isStun = false;
    public bool isAlert = false;

    public int IsRightIntEnemy
    {
        get
        {
            return transform.rotation == Quaternion.Euler(0, -180, 0) ? -1 : 1;
        }
    }
    public bool isBackAttack = false;

    protected virtual void Start()
    {
        playerStat = FindObjectOfType<PlayerStatus>();
        sprender = GetComponent<SpriteRenderer>();
        enemyRigid = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
        enemyHealth = enemyList.enemyHP;
        enemyStun = enemyList.enemySP;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon") && !isDamaged)
        {
            if (isBackAttack)
            {
                playerStat.isBackAttack = true;
                Debug.Log("백어택");
            }
            else
            {
                playerStat.isBackAttack = false;
            }
            playerStat.CriticalHit();
            enemyHealth -= playerStat.Damage();
            enemyStun -= playerStat.StunDamage();

            enemyRigid.AddForce(new Vector2(playerStat.knockback * playerStat.isRightInt, 0), ForceMode2D.Impulse);
            sprender.color = sprender.color = new Color(1, 0, 0, sprender.color.a);
            HitAnimation();
            isAlert = true;
            alertTimer = 0;
            isDamaged = true;
            Debug.Log("HP : " + enemyHealth);
            Debug.Log("Stun : " + enemyStun);
            Debug.Log("isRightInt : " + IsRightIntEnemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            sprender.color = new Color(1, 1, 1, sprender.color.a);
            playerStat.isCriticalAttack = false;
            ResetAnimationTrigger();
            isDamaged = false;
        }
    }





    protected virtual void Update()
    {
        if (isStun)
        {
            Restore();
        }
        else
        {
            RestoreEnd();
        }

    }

    protected void Restore()
    {
        if (restorationStartTime < enemyList.enemyRestoration)
        {
            restorationStartTime += Time.deltaTime;
            //기절하는 동안 적이 HP와 SP 회복
            enemyStun = Mathf.Lerp(0, enemyList.enemySP, restorationStartTime / enemyList.enemyRestoration);
            enemyHealth += Mathf.CeilToInt(enemyList.enemyHPRestoration * Time.deltaTime);

            sprender.color = new Color(sprender.color.r, sprender.color.g, sprender.color.b, 0.2f);
            enemyRigid.bodyType = RigidbodyType2D.Static;
            GetComponent<Collider2D>().enabled = false;
            StunAnimation();
        }
    }

    protected void RestoreEnd()
    {
        restorationStartTime = 0;
        sprender.color = new Color(sprender.color.r, sprender.color.g, sprender.color.b, 1);
        enemyRigid.bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Collider2D>().enabled = true;
        IdleAnimation();
    }



    #region Animation
    protected void HitAnimation()
    {
        enemyAnim.SetTrigger("Hit");
    }

    protected void StunAnimation()
    {
        enemyAnim.SetBool("IsStun", true);
    }

    protected void IdleAnimation()
    {
        enemyAnim.SetBool("IsStun", false);
    }

    protected void ResetAnimationTrigger()
    {
        enemyAnim.ResetTrigger("Hit");
    }

    protected void StunDisableAlpha()
    {
        sprender.color = new Color(sprender.color.r, sprender.color.g, sprender.color.b, 0.2f);
    }

    protected void StunEnableAlpha()
    {
        sprender.color = new Color(sprender.color.r, sprender.color.g, sprender.color.b, 1);
    }

    #endregion
}
