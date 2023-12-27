using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float wallJumpPower;
    [SerializeField] private float posX;
    [SerializeField] private bool isCanJump;
    [SerializeField] private bool isCeiling;
    [SerializeField] private bool isRunning;
    [SerializeField] private bool isAttack;
    [SerializeField] private bool isWall;
    [SerializeField] private bool isWallJump;


    [SerializeField] private float rayLength;
    [SerializeField] private float wallRayLength;
    [SerializeField] private float isRight = 1;

    public Transform meleeAttack;
    public Transform wallCheck;


    public LayerMask groundLayer;
    public LayerMask wallLayer;

    private Rigidbody2D rigid;
    private SpriteRenderer spRender;
    [SerializeField] private SpriteRenderer wpRender;
    private Animator anim;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spRender = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        isCanJump = Physics2D.Raycast(transform.position, Vector2.down, rayLength, groundLayer);
        isCeiling = Physics2D.Raycast(transform.position, Vector2.up, rayLength, groundLayer);
        isWall = Physics2D.Raycast(wallCheck.GetChild(0).position, Vector2.right * isRight, wallRayLength, wallLayer);

        //===============================================================================================================
        MoveCharacter();
        Wall();
        Debug.DrawRay(wallCheck.GetChild(0).position, (Vector2.right * isRight) * wallRayLength, Color.red);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Attack();
        }

        if (isCanJump && Input.GetKeyDown(KeyCode.Space) && !isAttack)
        {
            anim.SetTrigger("JumpStart");
            JumpCharacter();
        }
        else if (!isCanJump && Input.GetKeyDown(KeyCode.Z))
        {
            Attack();
        }
        else if (isCanJump)
        {
            anim.ResetTrigger("JumpStart");
            anim.ResetTrigger("JumpTop");

        }



        if ((rigid.velocity.y < 0.198f && rigid.velocity.y > 0) || isCeiling)
        {
            anim.SetTrigger("JumpTop");
        }
        else if (rigid.velocity.y < 0)
        {
            anim.SetBool("IsJumping", true);
            anim.SetBool("IsFalling", true);
            anim.SetBool("IsIdle", false);
        }
        else if (rigid.velocity.y == 0 && !isWall)
        {
            anim.SetBool("IsJumping", false);
            anim.SetBool("IsFalling", false);
        }



        if (posX == 0 && rigid.velocity.y == 0)
        {
            anim.SetBool("IsIdle", true);
        }

        //Debug.Log(rigid.velocity.y);
    }

    private void MoveCharacter()
    {
        if (!isAttack && !isWallJump && !isWall)
        {
            posX = Input.GetAxis("Horizontal");
            Vector2 position = new Vector2(posX * speed, rigid.velocity.y);

            rigid.velocity = position;

            if (posX < 0)
            {
                isRunning = true;
                anim.SetTrigger("RunStart");
                anim.SetBool("IsIdle", false);
                anim.SetBool("IsRunning", true);
                isRight = -1;
                transform.rotation = Quaternion.Euler(0, -180, 0);
                //spRender.flipX = true;
                //wpRender.flipX = true;
                //meleeAttack.rotation = Quaternion.Euler(0, -180, 0);
                //wallCheck.rotation = Quaternion.Euler(0, -180, 0);
            }
            else if (posX > 0)
            {
                isRunning = true;
                anim.SetTrigger("RunStart");
                anim.SetBool("IsIdle", false);
                anim.SetBool("IsRunning", true);
                isRight = 1;
                transform.rotation = Quaternion.identity;
                //spRender.flipX = false;
                //wpRender.flipX = false;
                //meleeAttack.rotation = Quaternion.identity;
                //wallCheck.rotation = Quaternion.identity;
            }
            else if (posX == 0)
            {
                isRunning = false;
                anim.SetBool("IsRunning", false);
                anim.SetBool("IsIdle", true);
                anim.ResetTrigger("RunStart");
            }
        }
        else
        {
            return;
        }



        /* if(posX > -0.2f && Input.GetKeyDown(KeyCode.LeftArrow))
         {
             spRender.flipX = true;
         }
         else if (posX < 0.2f && Input.GetKeyDown(KeyCode.RightArrow))
         {
             spRender.flipX = false;
         }*/

    }

    private void JumpCharacter()
    {
        anim.SetBool("IsJumping", true);
        anim.SetBool("IsIdle", false);
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    private void Attack()
    {
        //Debug.Log("АјАн");
        anim.SetTrigger("Attack");
        if (isRunning)
        {
            anim.SetBool("IsRunning", false);
            anim.SetBool("IsIdle", true);
        }
    }

    private void Wall()
    {
        if (isWall && anim.GetBool("IsJumping"))
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 0);
            isWallJump = false;
            anim.SetBool("IsWall", true);
            anim.SetBool("IsIdle", false);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                //isWallJump = true;
                //Invoke("NotTurn", 0.3f);
                anim.SetTrigger("JumpStart");
                if (Input.GetAxis("Horizontal") < 0)
                {
                    isRight = -1;
                    transform.rotation = Quaternion.Euler(0, -180, 0);
                    //meleeAttack.rotation = Quaternion.Euler(0, -180, 0);
                    //wallCheck.rotation = Quaternion.Euler(0, -180, 0);
                }
                else if (Input.GetAxis("Horizontal") > 0)
                {
                    isRight = 1;
                    transform.rotation = Quaternion.identity;
                    //meleeAttack.rotation = Quaternion.identity;
                    //wallCheck.rotation = Quaternion.identity;
                }
                rigid.velocity = new Vector2(-isRight * wallJumpPower, 0.9f * wallJumpPower);
                
            }
        }
        else if (!isWall)
        {

            anim.SetBool("IsWall", false);
        }
    }

    private void NotTurn()
    {
        isWallJump = false;
    }

    public void IsAttack()
    {
        isAttack = true;
    }

    public void IsNotAttack()
    {
        isAttack = false;
    }
    public void IsRun()
    {
        anim.SetBool("IsRunning", false);
        anim.SetBool("IsIdle", true);
    }
}
