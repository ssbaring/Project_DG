using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRayCheck : MonoBehaviour
{
    public bool isCanJump;
    public bool isCeiling;
    public bool isWall;
    public bool isWater;
    public bool isSloth;


    [Header("Collider")]
    public Transform meleeAttack;
    public Transform wallCheck;
    public Transform frontSlothCheck;
    public Transform backSlothCheck;

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
        RaycastHit2D frontSloth = Physics2D.Raycast(frontSlothCheck.position, Vector2.right, wallRayLength, groundLayer);
        RaycastHit2D backSloth = Physics2D.Raycast(backSlothCheck.position, Vector2.left, wallRayLength, groundLayer);

        Debug.DrawRay(wallCheck.position, (Vector2.right * pCon.isRightInt) * wallRayLength, Color.red);
        isCanJump = Physics2D.Raycast(transform.position, Vector2.down, rayGroundLength, Ground_and_Wall);
        isCeiling = Physics2D.Raycast(transform.position, Vector2.up, rayCeilLength, groundLayer);
        isSloth = frontSloth || backSloth;
        isWall = Physics2D.Raycast(wallCheck.position, Vector2.right * pCon.isRightInt, wallRayLength, wallLayer);
    }

   

}
