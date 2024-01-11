using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    
    [SerializeField] protected EnemyList enemyList;
    [SerializeField] protected float enemyHealth;
    [SerializeField] protected float enemyStun;
    [SerializeField] protected float alertTimer = 0;


    protected float restorationStartTime = 0;
    protected Rigidbody2D enemyRigid;
    protected SpriteRenderer sprender;
    protected PlayerStatus playerStat;
    protected Animator enemyAnim;

    public bool isDamaged = false;
    public bool isStun = false;
    public bool isAlert = false;

    public int isRightIntEnemy = 1;
    public float backCoefficent = 1.0f;


    protected virtual void Start()
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
            
            enemyRigid.AddForce(new Vector2(playerStat.knockback * playerStat.isRightInt, 0), ForceMode2D.Impulse);
            sprender.color = sprender.color = new Color(1, 0, 0, sprender.color.a);
            HitAnimation();
            Invoke("Hit", 0.3f);
            isDamaged = true;
            isAlert = true;
            alertTimer = 0;
            Debug.Log("HP : " + enemyHealth);
            Debug.Log("Stun : " + enemyStun);
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

    private void Hit()
    {
        sprender.color = Color.white;
    }

    protected virtual void Update()
    {
        

        if (isStun)
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
        else
        {
            restorationStartTime = 0;
            sprender.color = new Color(sprender.color.r, sprender.color.g, sprender.color.b, 1);
            enemyRigid.bodyType = RigidbodyType2D.Dynamic;
            GetComponent<Collider2D>().enabled = true;
            IdleAnimation();
        }

    }

    #region Animation
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

    public void StunDisableAlpha()
    {
        sprender.color = new Color(sprender.color.r, sprender.color.g, sprender.color.b, 0.2f);
    }

    public void StunEnableAlpha()
    {
        sprender.color = new Color(sprender.color.r, sprender.color.g, sprender.color.b, 1);
    }

    #endregion
}
