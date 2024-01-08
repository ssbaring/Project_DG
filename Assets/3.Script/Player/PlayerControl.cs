using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    private float jumpPower = 8.0f;
    private float wallJumpPower = 10.0f;
    private float wallFallSpeed = 5.0f;


    [SerializeField] private float posX;
    [SerializeField] private float wallPosX;
    [SerializeField] private float posY;


    [Header("Check")]
    public float isRightInt = 1;
    [SerializeField] private bool isRunning;
    [SerializeField] private bool isAttack;
    [SerializeField] private bool isWallJump;
    [SerializeField] private bool isJumping;

    [Header("PlayerStat")]
    public float defalutSpeed = 5.0f;
    public float jumpStartTime;
    public float jumpTime;

    [Header("Damage")]
    public float defalutDamage = 5.0f;
    public float defaultStunDamage = 10.0f;
    public float criticalProbability = 10.0f;

    public float gravity;

    private Rigidbody2D rigid;
    private Animator anim;
    private PlayerStatus stat;

    [SerializeField] private SpriteRenderer wpRender;
    [SerializeField] private PlayerRayCheck playerRay;
    [SerializeField] private Transform respawn;


    [Header("Key")]
    public KeyCode respawnKey;
    public KeyCode AttackKey;
    public KeyCode JumpKey;

    #region PlayerStatus
    public virtual float Damage()
    {
        return defalutDamage;
    }

    public virtual float StunDamage()
    {
        return defaultStunDamage;
    }

    public virtual float Speed()
    {
        return defalutSpeed;
    }

    public virtual float CriticalProbability()
    {
        return criticalProbability;
    }
    #endregion

    protected virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        stat = GetComponent<PlayerStatus>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //이동
        MoveCharacter();
        //벽점프
        Wall();

        //공격
        if (Input.GetKeyDown(AttackKey))
        {
            Attack();
        }

        //점프공격
        if (!playerRay.isCanJump && Input.GetKeyDown(AttackKey))
        {
            Attack();
        }

        //점프
        JumpCharacter();


        //땅에 닿음
        if (playerRay.isCanJump)
        {
            isWallJump = true;
            anim.ResetTrigger("JumpTop");
        }





        //천장
        if ((rigid.velocity.y < 0.198f && rigid.velocity.y > 0) || playerRay.isCeiling)
        {
            anim.SetTrigger("JumpTop");
        }
        else if (rigid.velocity.y < 0)
        {
            anim.SetBool("IsJumping", true);
            anim.SetBool("IsFalling", true);
            anim.SetBool("IsIdle", false);
        }
        else if (playerRay.isCanJump)
        {
            anim.SetBool("IsJumping", false);
            anim.SetBool("IsFalling", false);
        }


        //애니메이션
        if (posX == 0 && rigid.velocity.y == 0)
        {
            anim.SetBool("IsIdle", true);
        }

        //리스폰
        if (Input.GetKeyDown(respawnKey))
        {
            transform.position = respawn.position;
        }

        //낙하속도 조절
        if (rigid.velocity.y < -15.0f)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, -15.0f);
        }

    }



    private void MoveCharacter()
    {
        posX = Input.GetAxis("Horizontal");
        Vector2 position = new Vector2(posX * stat.Speed(), rigid.velocity.y);
        if (!isAttack && isWallJump)
        {
            wallPosX = 0;
            rigid.velocity = position;


            if (posX < 0)
            {
                isRunning = true;
                anim.SetTrigger("RunStart");
                anim.SetBool("IsIdle", false);
                anim.SetBool("IsRunning", true);
                isRightInt = -1;
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (posX > 0)
            {
                isRunning = true;
                anim.SetTrigger("RunStart");
                anim.SetBool("IsIdle", false);
                anim.SetBool("IsRunning", true);
                isRightInt = 1;
                transform.rotation = Quaternion.identity;
            }
            else if (posX == 0)
            {
                isRunning = false;
                anim.SetBool("IsRunning", false);
                anim.SetBool("IsIdle", true);
                anim.ResetTrigger("RunStart");
            }
        }
        else if (isAttack)
        {
            posX = 0;
        }
        else
        {
            return;
        }


    }

    private void JumpCharacter()
    {
        if (playerRay.isCanJump == true && Input.GetKeyDown(JumpKey) && !isAttack)
        {
            JumpAnimation();
            isJumping = true;
            jumpTime = jumpStartTime;
            rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
        }

        if (Input.GetKey(JumpKey) && isJumping == true)
        {
            if (jumpTime > 0)
            {
                JumpAnimation();
                rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
                jumpTime -= Time.deltaTime;
            }
            else
            {
                Debug.Log("JumpTimeCounter = 0");
                isJumping = false;
                anim.ResetTrigger("JumpStart");
            }
        }

        if (Input.GetKeyUp(JumpKey))
        {
            Debug.Log("점프키 뗌");
            isJumping = false;
            anim.ResetTrigger("JumpStart");
        }
    }

    private void JumpAnimation()
    {
        anim.SetTrigger("JumpStart");
        anim.SetBool("IsJumping", true);
        anim.SetBool("IsIdle", false);
    }


    private void Attack()
    {
        Debug.Log("공격");
        if (playerRay.isWall) return;

        anim.SetTrigger("Attack");
        if (isRunning)
        {
            anim.SetBool("IsRunning", false);
            anim.SetBool("IsIdle", true);
        }

    }

    private void Wall()
    {
        if (playerRay.isWall && anim.GetBool("IsJumping"))
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 0);
            rigid.gravityScale = 0;
            isWallJump = false;
            anim.SetBool("IsFalling", false);
            anim.SetBool("IsRunning", false);
            anim.SetBool("IsWall", true);
            anim.SetBool("IsIdle", false);

            wallPosX = Input.GetAxis("Horizontal");
            posX = 0;
            if (isRightInt == -1)
            {
                if (wallPosX < 0)
                {
                    wallPosX = 0;
                }
                else
                {
                    wallPosX = Mathf.Abs(wallPosX);
                }
            }
            else if (isRightInt == 1)
            {
                if (wallPosX > 0)
                {
                    wallPosX = 0;
                }
                else
                {
                    wallPosX = Mathf.Abs(wallPosX) * -1;
                }
            }

            if (Input.GetKeyDown(JumpKey))
            {
                isWallJump = true;
                //Invoke("NotTurn", 0.3f);
                if (wallPosX < 0)
                {
                    Debug.Log("벽점프");
                    anim.SetTrigger("JumpStart");
                    isRightInt = -1;
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (wallPosX > 0)
                {
                    Debug.Log("벽점프");
                    anim.SetTrigger("JumpStart");
                    isRightInt = 1;
                    transform.rotation = Quaternion.identity;
                }
                else return;

                rigid.velocity = new Vector2(-isRightInt * wallJumpPower, 0.9f * wallJumpPower);
                rigid.gravityScale = gravity;
                //isWallJump = true;
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                posY = Input.GetAxisRaw("Vertical");
                rigid.gravityScale = gravity;
                rigid.velocity = new Vector2(0, posY * wallFallSpeed);
            }
        }
        else if (!playerRay.isWall)
        {
            rigid.gravityScale = gravity;
            anim.SetBool("IsWall", false);
        }
    }


    private void NotTurn()
    {
        isWallJump = false;
    }

    #region Animation Event

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

    #endregion
}
