using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public enum PlayerState
    {
        Idle = 1,
        Move = 2,
        Attack = 4,
        DashAttack = 8,
        Jump = 16,
        JumpAttack = 32,
        WallJump = 64,
        Die = 128
    }
    [SerializeField] protected PlayerState currentState = PlayerState.Idle;

    protected float jumpPower = 6.5f;
    protected float wallJumpPower = 10.0f;
    protected float wallFallSpeed = 5.0f;


    [SerializeField] protected float posX;
    [SerializeField] protected float wallPosX;
    [SerializeField] protected float posY;

    [Header("Ray Status")]
    [SerializeField] private float wallRayLength;
    [SerializeField] private float rayCeilLength;
    [SerializeField] private float rayGroundLength;
    [SerializeField] private Vector2 boxSize = new Vector2(0.1f, 0.1f);
    [SerializeField] private int Ground_and_Wall;

    [Header("State Check")]
    public bool isCanJump;
    public bool isGround;
    public bool isCeiling;
    public bool isWall;
    public bool isWater;
    public bool isSloth;

    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public LayerMask waterLayer;

    [Header("Check")]
    public float isRightInt = 1;
    public bool isRunning;
    public bool isAttack;
    public bool isWallJump;
    public bool isJumping;
    public bool isDead;

    [Header("Collider")]
    public Transform meleeAttack;
    public Transform wallCheck;
    public Transform groundCheck;
    public Transform frontSlothCheck;
    public Transform backSlothCheck;

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
        Ground_and_Wall = groundLayer | wallLayer | waterLayer;
    }

    protected virtual void Update()
    {
        RaycastHit2D lineHit = Physics2D.Raycast(transform.position, Vector2.down, rayGroundLength, Ground_and_Wall);
        isCeiling = Physics2D.Raycast(transform.position, Vector2.up, rayCeilLength, groundLayer);
        isWall = Physics2D.Raycast(wallCheck.position, Vector2.right * isRightInt, wallRayLength, wallLayer);
        isCanJump = lineHit;


        if (!isDead)
        {
            switch (currentState)
            {
                case PlayerState.Idle:
                    IdlePlayer();
                    if (Input.GetAxis("Horizontal") != 0)
                    {
                        ChangeState(PlayerState.Move);
                    }
                    else if (Input.GetKeyDown(GameManager.instance.AttackKey))
                    {
                        ChangeState(PlayerState.Attack);
                    }
                    else if (Input.GetKeyDown(GameManager.instance.JumpKey))
                    {
                        ChangeState(PlayerState.Jump);
                    }
                    break;
                case PlayerState.Move:
                    MoveCharacter();
                    if (Input.GetKeyDown(GameManager.instance.AttackKey))
                    {
                        ChangeState(PlayerState.DashAttack);
                    }
                    else if (Input.GetKeyDown(GameManager.instance.JumpKey))
                    {
                        ChangeState(PlayerState.Jump);
                    }
                    else if (Input.GetAxis("Horizontal") == 0)
                    {
                        ChangeState(PlayerState.Idle);
                    }
                    break;
                case PlayerState.Attack:
                    Attack();
                    if (Input.GetKeyDown(GameManager.instance.AttackKey))
                    {
                        ChangeState(PlayerState.Attack);
                    }
                    break;
                case PlayerState.DashAttack:
                    DashAttack();
                    if (Input.GetAxis("Horizontal") == 0)
                    {
                        ChangeState(PlayerState.Idle);
                    }
                    else if (Input.GetKeyDown(GameManager.instance.AttackKey))
                    {
                        ChangeState(PlayerState.DashAttack);
                    }
                    else if (Input.GetKeyDown(GameManager.instance.JumpKey))
                    {
                        ChangeState(PlayerState.Jump);
                    }
                    break;
                case PlayerState.Jump:
                    JumpCharacter();
                    if (Input.GetKeyDown(GameManager.instance.AttackKey))
                    {
                        ChangeState(PlayerState.JumpAttack);
                    }
                    break;
                case PlayerState.JumpAttack:
                    if (Input.GetKeyDown(GameManager.instance.AttackKey))
                    {
                        ChangeState(PlayerState.JumpAttack);
                    }
                    break;
                case PlayerState.WallJump:
                    Wall();
                    if (Input.GetKeyDown(GameManager.instance.JumpKey))
                    {
                        ChangeState(PlayerState.Jump);
                    }
                    break;
                case PlayerState.Die:
                    isDead = true;
                    break;
            }


            //��������
            if (!isCanJump && Input.GetKeyDown(GameManager.instance.AttackKey))
            {
                Attack();
            }

        }


        //���� ����
        if (isCanJump)
        {
            isWallJump = false;
            anim.SetBool("IsJumping", false);
            anim.SetBool("IsFalling", false);
            anim.ResetTrigger("JumpTop");
            rigid.gravityScale = gravity;
        }


        //õ��
        if ((rigid.velocity.y < 0.198f && rigid.velocity.y > 0) || isCeiling)
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


        //�ִϸ��̼�
        if (posX == 0 && rigid.velocity.y == 0)
        {
            anim.SetBool("IsIdle", true);
        }



        //���ϼӵ� ����
        if (rigid.velocity.y < -15.0f)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, -15.0f);
        }

        //��
        if (isWater)
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


    private void ChangeState(PlayerState state)
    {
        currentState = state;
        Debug.Log($"Current State = {state}");
    }

    private void IdlePlayer()
    {
        isRunning = false;
        anim.SetBool("IsRunning", false);
        anim.SetBool("IsIdle", true);
        anim.ResetTrigger("RunStart");
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
                playerRayPivot.transform.rotation = Quaternion.Euler(0, 180, 0);   //�����δ� -180���� �ٲ�
                meleePivot.transform.rotation = Quaternion.Euler(0, 180, 0);   //�����δ� -180���� �ٲ�
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
            else
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
        if ((isCanJump || isWater) && !isAttack)
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
        if (isWall && isWallJump) return;
        anim.SetFloat("AttackSpeed", stat.AttackSpeed());
        anim.SetTrigger("Attack");
    }

    private void DashAttack()
    {
        if (isWall && isWallJump) return;
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
        if (isWall && anim.GetBool("IsJumping") && (posX != 0 || wallPosX != 0))
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
                    Debug.Log("������");
                    anim.SetTrigger("JumpStart");
                    isRightInt = -1;
                    playerRayPivot.transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (wallPosX > 0)
                {
                    Debug.Log("������");
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
        else if (!isWall)
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
