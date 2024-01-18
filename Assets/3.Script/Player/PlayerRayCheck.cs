using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRayCheck : MonoBehaviour
{
    public bool isCanJump;
    public bool isGround;
    public bool isCeiling;
    public bool isWall;
    public bool isWater;
    public bool isSloth;


    [Header("Collider")]
    public Transform meleeAttack;
    public Transform wallCheck;
    public Transform groundCheck;
    public Transform frontSlothCheck;
    public Transform backSlothCheck;

    [SerializeField] private PlayerControl pCon;
    [SerializeField] private float wallRayLength;
    [SerializeField] private float rayCeilLength;
    [SerializeField] private float rayGroundLength;
    [SerializeField] private Vector2 boxSize = new Vector2(0.1f, 0.1f);

    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public LayerMask waterLayer;

    [SerializeField] private BoxCollider2D box;

    private void Start()
    {
        pCon = GetComponent<PlayerControl>();
    }

    private void Update()
    {
        int Ground_and_Wall = groundLayer | wallLayer | waterLayer;
        RaycastHit2D frontSloth = Physics2D.Raycast(frontSlothCheck.position, Vector2.right, wallRayLength, groundLayer);
        RaycastHit2D backSloth = Physics2D.Raycast(backSlothCheck.position, Vector2.left, wallRayLength, groundLayer);

        RaycastHit2D lineHit = Physics2D.Raycast(transform.position, Vector2.down, rayGroundLength, Ground_and_Wall);
        //RaycastHit2D boxHit = Physics2D.BoxCast(groundCheck.position, boxSize, 0, Vector2.down, 0, Ground_and_Wall);

        Debug.DrawRay(wallCheck.position, (Vector2.right * pCon.isRightInt) * wallRayLength, Color.red);
        
        isCeiling = Physics2D.Raycast(transform.position, Vector2.up, rayCeilLength, groundLayer);
        isSloth = frontSloth || backSloth;
        isWall = Physics2D.Raycast(wallCheck.position, Vector2.right * pCon.isRightInt, wallRayLength, wallLayer);
        isCanJump = lineHit;
        //isGround = lineHit;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, boxSize);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Vector2.up * rayCeilLength);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, Vector2.down * rayGroundLength);

    }

}
