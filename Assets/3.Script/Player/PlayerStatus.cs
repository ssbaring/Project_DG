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

    public float trueDamage;
    public float trueStunDamage;

    public bool isBackAttack = false;
    public bool isCriticalAttack = false;

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
        float finalSpeed = (1 + speedLevel) * 0.5f + defaultSpeed;
        return finalSpeed;
    }

    public override float CriticalProbability()
    {
        float defaultCriticalProbability = base.CriticalProbability();
        float finalCritical = (criticalLevel * 0.5f) + defaultCriticalProbability;
        return finalCritical;
    }

    public void CriticalHit()
    {
        isCriticalAttack = Random.Range(0, 100.0f) < CriticalProbability();
        if(isCriticalAttack)
        {
            Debug.LogWarning("Critical");
        }
    }


}
