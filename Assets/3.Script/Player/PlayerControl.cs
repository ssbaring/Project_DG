using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    protected float jumpPower = 6.5f;
    protected float wallJumpPower = 10.0f;
    protected float wallFallSpeed = 5.0f;


    [SerializeField] protected float posX;
    [SerializeField] protected float wallPosX;
    [SerializeField] protected float posY;


    [Header("Check")]
    public float isRightInt = 1;
    [SerializeField] protected bool isRunning;
    [SerializeField] protected bool isAttack;
    [SerializeField] protected bool isWallJump;
    [SerializeField] protected bool isJumping;
    [SerializeField] protected bool isDead;

    [Header("PlayerStat")]
    public float defalutSpeed = 5.0f;
    public float defalutAttackSpeed = 1.0f;

    [HideInInspector] public float jumpStartTime;
    [HideInInspector] public float jumpTime;

    [Header("Damage")]
    public float defalutDamage = 5.0f;
    public float defaultStunDamage = 10.0f;
    public float criticalProbability = 10.0f;

    public float gravity;
    public float waterGravity;

    protected Rigidbody2D rigid;
    protected Animator anim;
    protected PlayerStatus stat;

    public GameObject playerRayPivot;
    public GameObject meleePivot;
    [SerializeField] protected SpriteRenderer wpRender;
    [SerializeField] protected SpriteRenderer spRender;
    [SerializeField] protected PlayerRayCheck playerRay;




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

    public virtual float AttackSpeed()
    {
        return defalutAttackSpeed;
    }

    public virtual float CriticalProbability()
    {
        return criticalProbability;
    }
    #endregion

    protected virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spRender = GetComponent<SpriteRenderer>();
        stat = GetComponent<PlayerStatus>();
        anim = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        if (!isDead)
        {
            //이동
            MoveCharacter();
            //벽점프
            Wall();

            //공격
            if (Input.GetKeyDown(GameManager.instance.AttackKey))
            {
                Attack();
            }

            //점프공격
            if (!playerRay.isCanJump && Input.GetKeyDown(GameManager.instance.AttackKey))
            {
                Attack();
            }

            //점프
            JumpCharacter();
        }


        //땅에 닿음
        if (playerRay.isCanJump)
        {
            isWallJump = false;
            anim.SetBool("IsJumping", false);
            anim.SetBool("IsFalling", false);
            anim.ResetTrigger("JumpTop");
            rigid.gravityScale = gravity;
        }

        /*if (playerRay.isSloth)
        {
            rigid.gravityScale = 0;
        }*/

        //천장
        if ((rigid.velocity.y < 0.198f && rigid.velocity.y > 0) || playerRay.isCeiling)
        {
            anim.SetTrigger("JumpTop");
            jumpTime = -0.1f;
        }
        else if (rigid.velocity.y < 0)
        {
            anim.SetBool("IsJumping", true);
            anim.SetBool("IsFalling", true);
            anim.SetBool("IsIdle", false);
        }


        //애니메이션
        if (posX == 0 && rigid.velocity.y == 0)
        {
            anim.SetBool("IsIdle", true);
        }

        

        //낙하속도 조절
        if (rigid.velocity.y < -15.0f)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, -15.0f);
        }

        //물
        if (playerRay.isWater)
        {
            rigid.gravityScale = waterGravity;
            if (rigid.velocity.y < -5.0f)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, -5.0f);
            }
            jumpPower = 1.5f;
        }
        else
        {
            jumpPower = 6.5f;
        }

    }



    private void MoveCharacter()
    {
        posX = Input.GetAxis("Horizontal");
        anim.SetFloat("RunSpeed", stat.TrueSpeedAnimation());
        Vector2 position = new Vector2(posX * stat.Speed(), rigid.velocity.y);
        if (!isAttack && !isWallJump)
        {
            wallPosX = 0;
            rigid.velocity = position;


            if (posX < 0)
            {
                isRunning = true;
                spRender.flipX = true;
                anim.SetTrigger("RunStart");
                anim.SetBool("IsIdle", false);
                anim.SetBool("IsRunning", true);
                isRightInt = -1;
                playerRayPivot.transform.rotation = Quaternion.Euler(0, 180, 0);   //실제로는 -180으로 바뀜
                meleePivot.transform.rotation = Quaternion.Euler(0, 180, 0);   //실제로는 -180으로 바뀜
            }
            else if (posX > 0)
            {
                isRunning = true;
                spRender.flipX = false;
                anim.SetTrigger("RunStart");
                anim.SetBool("IsIdle", false);
                anim.SetBool("IsRunning", true);
                isRightInt = 1;
                playerRayPivot.transform.rotation = Quaternion.identity;
                meleePivot.transform.rotation = Quaternion.identity;
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
        if ((playerRay.isCanJump || playerRay.isWater) && Input.GetKeyDown(GameManager.instance.JumpKey) && !isAttack)
        {
            JumpAnimation();
            isJumping = true;
            jumpTime = jumpStartTime;
            rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
        }

        if (Input.GetKey(GameManager.instance.JumpKey) && isJumping == true)
        {
            if (jumpTime > 0)
            {
                JumpAnimation();
                rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
                jumpTime -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
                anim.ResetTrigger("JumpStart");
            }
        }

        if (Input.GetKeyUp(GameManager.instance.JumpKey))
        {
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
        if (playerRay.isWall && isWallJump) return;
        anim.SetFloat("AttackSpeed", stat.AttackSpeed());
        anim.SetTrigger("Attack");
        if (isRunning)
        {
            anim.SetBool("IsRunning", false);
            anim.SetBool("IsIdle", true);
        }

    }

    private void Wall()
    {
        if (playerRay.isWall && anim.GetBool("IsJumping") && (posX != 0 || wallPosX != 0))
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 0);
            rigid.gravityScale = 0;
            isWallJump = true;
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

            if (Input.GetKeyDown(GameManager.instance.JumpKey))
            {
                isWallJump = false;
                //Invoke("NotTurn", 0.3f);
                if (wallPosX < 0)
                {
                    Debug.Log("벽점프");
                    anim.SetTrigger("JumpStart");
                    isRightInt = -1;
                    playerRayPivot.transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (wallPosX > 0)
                {
                    Debug.Log("벽점프");
                    anim.SetTrigger("JumpStart");
                    isRightInt = 1;
                    playerRayPivot.transform.rotation = Quaternion.identity;
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
            isWallJump = false;
        }
    }

    protected void CursedDie()
    {
        anim.SetBool("IsCurseDie", true);
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
