using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObject/EnemyLists")]
public class EnemyList : ScriptableObject
{
    public int enemyID;
    public string enemyName;
    public float enemyMaxHP = 100;
    public float enemyCurrentHP = 100;
    public float enemyMaxSP;
    public float enemyCurrentSP;
    public float enemySpeed = 3.0f;
    public Animator enemyAnim;
    public SpriteRenderer enemySprite;

}
