using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRayCheck : MonoBehaviour
{
    public bool isCanJump;
    public bool isCeiling;
    public bool isWall;



    [Header("Collider")]
    public Transform meleeAttack;
    public Transform wallCheck;

    [SerializeField] private float rayLength;
    [SerializeField] private float wallRayLength;
    [SerializeField] private PlayerControl pCon;

    public LayerMask groundLayer;
    public LayerMask wallLayer;

    

    private void Update()
    {
        int Ground_and_Wall = groundLayer | wallLayer;

        Debug.DrawRay(wallCheck.position, (Vector2.right * pCon.isRightInt) * wallRayLength, Color.red);
        isCanJump = Physics2D.Raycast(transform.position, Vector2.down, rayLength, Ground_and_Wall);
        isCeiling = Physics2D.Raycast(transform.position, Vector2.up, rayLength, groundLayer);
        isWall = Physics2D.Raycast(wallCheck.position, Vector2.right * pCon.isRightInt, wallRayLength, wallLayer);
    }
}
