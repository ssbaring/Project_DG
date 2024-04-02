using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum EnemyState
{
    Idle = 1,
    Move = 2,
    Warning = 4,
    Attack = 8,
    Stun = 16,
    Die = 32
}


public class EnemyAI : MonoBehaviour
{
    private EnemyState currentEnemyState = EnemyState.Idle;


    private void Update()
    {
        switch (currentEnemyState)
        {
            case EnemyState.Idle:
             
                break;
            case EnemyState.Move:
                break;
            case EnemyState.Warning:
                break;
            case EnemyState.Attack:
                break;
            case EnemyState.Stun:
                break;
            case EnemyState.Die:
                break;
        }
    }

    private void ChangeEnemyState(EnemyState state)
    {
        currentEnemyState = state;
    }
}
