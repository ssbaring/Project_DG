using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float posX;
    [SerializeField] private bool isCanJump;
    [SerializeField] private bool isCeiling;
    [SerializeField] private bool isAttack;
    [SerializeField] private float rayLength;
    public LayerMask groundlayer;

    private float attackComboTime = 0;
    private int attackCount = 0;
    private Rigidbody2D rigid;
    private SpriteRenderer spRender;
    private Animator anim;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spRender = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        MoveCharacter();
        isCanJump = Physics2D.Raycast(transform.position, Vector2.down, rayLength, groundlayer);
        isCeiling = Physics2D.Raycast(transform.position, Vector2.up, rayLength, groundlayer);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            StopCoroutine(Attack());
            StartCoroutine(Attack());
        }

        if (isCanJump && Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("JumpStart");
            JumpCharacter();
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
        else if (rigid.velocity.y == 0)
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
        posX = Input.GetAxis("Horizontal");

        Vector2 position = new Vector2(posX * speed, rigid.velocity.y);

        rigid.velocity = position;


        if (posX < 0)
        {
            spRender.flipX = true;
            anim.SetTrigger("RunStart");
            anim.SetBool("IsIdle", false);
            anim.SetBool("IsRunning", true);
        }

        if (posX > 0)
        {
            spRender.flipX = false;
            anim.SetTrigger("RunStart");
            anim.SetBool("IsIdle", false);
            anim.SetBool("IsRunning", true);
        }


        if (posX == 0)
        {
            anim.SetBool("IsRunning", false);
            anim.SetBool("IsIdle", true);
            anim.ResetTrigger("RunStart");
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

    private IEnumerator Attack()
    {
        Debug.Log("АјАн");
        anim.SetTrigger("Attack");
        anim.SetBool("IsAttack", true);
        anim.SetBool("IsIdle", false);
        anim.SetInteger("AttackCount", attackCount);
        while (attackComboTime < 1.0f)
        {
            yield return null;
            attackComboTime += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("break");
                break;
            }
        }


        if (attackComboTime >= 1.0f)
        {
            anim.ResetTrigger("Attack");
            anim.SetBool("IsAttack", false);
            anim.SetBool("IsIdle", true);
            attackCount = 0;
            attackComboTime = 0;
        }
        else if (attackComboTime < 1.0f)
        {
            if (attackCount != 2)
            {
                attackCount++;
            }
            else if (attackCount == 2)
            {
                attackCount = 0;
                anim.ResetTrigger("Attack");
                anim.SetBool("IsAttack", false);
                anim.SetBool("IsIdle", true);
            }
            attackComboTime = 0;
        }

    }
}
