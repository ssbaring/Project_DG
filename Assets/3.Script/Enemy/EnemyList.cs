using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObject/EnemyLists")]
public class EnemyList : ScriptableObject
{
    public int enemyID;
    public string enemyName;
    public float enemyHP = 100;
    public float enemySP;
    public float enemySpeed = 3.0f;
    public Animator enemyAnim;
    public SpriteRenderer enemySprite;

}
