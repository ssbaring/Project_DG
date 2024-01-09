using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRayCheck : MonoBehaviour
{
    public bool isCanJump;
    public bool isCeiling;
    public bool isWall;
    public bool isWater;


    [Header("Collider")]
    public Transform meleeAttack;
    public Transform wallCheck;

    [SerializeField] private PlayerControl pCon;
    [SerializeField] private float wallRayLength;
    [SerializeField] private float rayCeilLength;
    [SerializeField] private float rayGroundLength;

    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public LayerMask waterLayer;

    [SerializeField] private BoxCollider2D box;



    private void Update()
    {
        int Ground_and_Wall = groundLayer | wallLayer | waterLayer;

        Debug.DrawRay(wallCheck.position, (Vector2.right * pCon.isRightInt) * wallRayLength, Color.red);
        isCanJump = Physics2D.Raycast(transform.position, Vector2.down, rayGroundLength, Ground_and_Wall);
        isCeiling = Physics2D.Raycast(transform.position, Vector2.up, rayCeilLength, groundLayer);
        isWall = Physics2D.Raycast(wallCheck.position, Vector2.right * pCon.isRightInt, wallRayLength, wallLayer);
    }

   

}
