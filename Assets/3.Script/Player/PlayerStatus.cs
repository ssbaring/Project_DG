using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public float playerDamage;
    public float playerStunDamage;
    public float playerSpeed;

    [SerializeField] private int strengthLevel = 0;
    [SerializeField] private int agilityLevel = 0;
    [SerializeField] private int speedLevel = 0;
    [SerializeField] private int criticalLevel = 0;

    

    protected void Update()
    {
        //damage = playerDamage + ((1 + strengthLevel) * Random.Range(0.45f, 0.55f));
        //stunDamage = playerStunDamage + ((1 + strengthLevel) * Random.Range(0.4f, 0.6f));
        //speed = playerSpeed + ((1 + speedLevel) * 0.5f);

    }
}
