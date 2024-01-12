using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStatus : PlayerControl
{
    
    [Header("Status Level")]
    public int strengthLevel = 0;
    public int agilityLevel = 0;
    public int speedLevel = 0;
    public int criticalLevel = 0;
    public int playerLevel = 1;
    public int skillPoint = 1;
    [Space(10f)]
    public float playerMaxEXP = 100;
    public float playerCurrentEXP = 0;
    [Space(10f)]
    [SerializeField] private int curseStack = 0;

    public float trueDamage;
    public float trueStunDamage;
    public float knockback = 0.2f;

    public int CurseStack
    {
        get
        {
            return curseStack;
        }
        set
        {
            curseStack = value;
            if(curseStack >= 4)
            {
                isDead = true;
                anim.SetBool("IsCurseDie", true);
                rigid.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            else
            {
                anim.SetBool("IsCurseDie", false);
                rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                Debug.Log(curseStack);
            }

        }
    }
    
    public bool isBackAttack = false;
    public bool isCriticalAttack = false;

    private PlayerRayCheck check;
    private void Awake()
    {
        check = GetComponent<PlayerRayCheck>();
    }

    public float TrueDamage(float defaultDmg)
    {
        trueDamage = (((1 + strengthLevel) * 1.8f) * defaultDmg);
        return trueDamage;
    }

    public float TrueStunDamage(float defaultSDmg)
    {
        trueStunDamage = (((1 + strengthLevel) * 1.8f) * defaultSDmg);
        return trueStunDamage;
    }

    public float TrueSpeedAnimation()
    {
        float trueSpeed = check.isWater ? 0.5f : (speedLevel * 0.1f) + 1;
        return trueSpeed;
    }

    public void GetPlayerEXP(int enemyEXP)
    {
        playerCurrentEXP += enemyEXP;
    }

    private void PlayerLevelUP()
    {
        if(playerCurrentEXP >= playerMaxEXP)
        {
            playerLevel++;
            skillPoint++;
            playerCurrentEXP -= playerMaxEXP;
            playerMaxEXP = Mathf.CeilToInt((playerMaxEXP + ((playerLevel - 1) * 20)) * 1.3f);
        }
        else
        {
            return;
        }
    }

    protected override void Update()
    {
        base.Update();
        PlayerLevelUP();
    }

    #region Overriding

    public override float Damage()
    {
        float defalutDamage = base.Damage();
        float finalDamage = TrueDamage(defalutDamage) * (isBackAttack ? 1.5f : 1.0f) * (isCriticalAttack ? 2.0f : 1.0f);
        return finalDamage;
    }

    public override float StunDamage()
    {
        float defaultStunDamage = base.StunDamage();
        float finalStunDamage = TrueStunDamage(defaultStunDamage) * (isBackAttack ? 2.5f : 1.0f) * (isCriticalAttack ? 1.5f : 1.0f);
        return finalStunDamage;
    }

    public override float Speed()
    {
        float defaultSpeed = base.Speed();
        float finalSpeed = check.isWater ? 2.5f : speedLevel * 0.15f + defaultSpeed;
        return finalSpeed;
    }

    public override float AttackSpeed()
    {
        float defaultAttackSpeed = base.AttackSpeed();
        float finalAttackSpeed = speedLevel * 0.1f + defaultAttackSpeed;
        return finalAttackSpeed;
    }

    public override float CriticalProbability()
    {
        float defaultCriticalProbability = base.CriticalProbability();
        float finalCritical = (criticalLevel * 0.5f) + defaultCriticalProbability;
        return finalCritical;
    }

    #endregion

    public void CriticalHit()
    {
        isCriticalAttack = Random.Range(0, 100.0f) < CriticalProbability();
        if(isCriticalAttack)
        {
            Debug.LogWarning("Critical");
        }
    }




}
