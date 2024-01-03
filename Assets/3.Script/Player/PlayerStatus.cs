using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStatus : PlayerControl
{
    [SerializeField] private int strengthLevel = 0;
    [SerializeField] private int agilityLevel = 0;
    [SerializeField] private int speedLevel = 0;
    [SerializeField] private int criticalLevel = 0;

    public bool isBackAttack = false;
    public bool isCriticalAttack = false;
    public override float Damage()
    {
        float defalutDamage = base.Damage();
        float finalDamage = (((1 + strengthLevel) * 1.8f) * defalutDamage) * (isBackAttack ? 1.5f : 1.0f) * (isCriticalAttack ? 2.0f : 1.0f);
        return finalDamage;
    }

    public override float StunDamage()
    {
        float defaultStunDamage = base.StunDamage();
        float finalStunDamage = (((1 + strengthLevel) * 1.8f) * defaultStunDamage) * (isBackAttack ? 2.5f : 1.0f) * (isCriticalAttack ? 1.5f : 1.0f);
        return finalStunDamage;
    }

    public override float Speed()
    {
        float defaultSpeed = base.Speed();
        float finalSpeed = (1 + speedLevel) * 0.5f + defaultSpeed;
        return finalSpeed;
    }


}
