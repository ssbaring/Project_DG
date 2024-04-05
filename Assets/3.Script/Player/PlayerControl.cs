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

    [Header("State Check")]
    public bool isGround;
    public bool isJumping;
    public bool isRunning;
    public bool isCeiling;
    public bool isWall;
    public bool isWater;
    public bool isAttack;
    public bool isWallGrap;
    public bool isDead;
    public float isRightInt = 1;

    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public LayerMask waterLayer;


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
    [SerializeField] protected BoxCollider2D groundCollider;




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
            //�̵�
            MoveCharacter();
            //������
            Wall();

            //����
            if (Input.GetKeyDown(GameManager.instance.AttackKey))
            {
                Attack();
            }

            //��������
            if (!isGround && Input.GetKeyDown(GameManager.instance.AttackKey))
            {
                Attack();
            }

            //����
            JumpCharacter();
        }


        //���� ����
        if (isGround)
        {
            isWallGrap = false;
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



    private void MoveCharacter()
    {
        posX = Input.GetAxis("Horizontal");
        anim.SetFloat("RunSpeed", stat.TrueSpeedAnimation());
        Vector2 position = new Vector2(posX * stat.Speed(), rigid.velocity.y);
        if (!isAttack && !isWallGrap)
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
        if ((isGround || isWater) && Input.GetKeyDown(GameManager.instance.JumpKey) && !isAttack)
        {
            JumpAnimation();
            isJumping = true;
            isGround = false;
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
        if (isWall && isWallGrap) return;
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
        if (isWall)
        {
            if (Input.GetKeyDown(GameManager.instance.JumpKey))
            {
                groundCollider.enabled = false;
                rigid.velocity = new Vector2(rigid.velocity.x, 0);
                rigid.gravityScale = 0;
                isWallGrap = true;
                isGround = false;
                anim.SetBool("IsFalling", false);
                anim.SetBool("IsRunning", false);
                anim.SetBool("IsWall", true);
                anim.SetBool("IsIdle", false);
            }

            if (isWallGrap)
            {
                Debug.Log("���׽�Ʈ");
                wallPosX = Input.GetAxis("Horizontal");
                posX = 0;
                rigid.velocity = new Vector2(posX, 0);
            }

            if (Input.GetKeyDown(GameManager.instance.JumpKey) && isWallGrap)
            {
                Debug.Log("���� ����");
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

                
                if (wallPosX < 0)
                {
                    Debug.Log("������");
                    Invoke("DelayGroundCollider", 2f);
                    //rigid.AddForce()
                    anim.SetTrigger("JumpStart");
                    isRightInt = -1;
                    playerRayPivot.transform.rotation = Quaternion.Euler(0, 180, 0);
                    isWallGrap = false;
                }
                else if (wallPosX > 0)
                {
                    Debug.Log("������");
                    Invoke("DelayGroundCollider", 2f);
                    anim.SetTrigger("JumpStart");
                    isRightInt = 1;
                    playerRayPivot.transform.rotation = Quaternion.identity;
                    isWallGrap = false;
                }
                else return;

                rigid.velocity = new Vector2(-isRightInt * wallJumpPower, 0.9f * wallJumpPower);
                rigid.gravityScale = gravity;
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
            groundCollider.enabled = true;
            rigid.gravityScale = gravity;
            anim.SetBool("IsWall", false);
            isWallGrap = false;
        }
    }

    protected void CursedDie()
    {
        anim.SetBool("IsCurseDie", true);
    }

    private void DelayGroundCollider()
    {
        groundCollider.enabled = true;
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
