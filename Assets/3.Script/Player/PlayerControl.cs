using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float posX;
    [SerializeField] private bool isJump;
    [SerializeField] private float rayLength;
    public LayerMask groundlayer;

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

        isJump = Physics2D.Raycast(transform.position, Vector2.down, rayLength, groundlayer);

        if (isJump && Input.GetButtonDown("Jump"))
        {
            JumpCharacter();
        }

        if (rigid.velocity.y > 0)
        {
            anim.SetTrigger("JumpStart");
            anim.SetBool("IsJumping", true);
            anim.SetBool("IsIdle", false);
        }
        else if (rigid.velocity.y < 0)
        {
            anim.SetBool("IsIdle", false);
        }
        else
        {
            anim.ResetTrigger("JumpStart");
            anim.SetBool("IsJumping", false);
        }



        if(posX == 0 && rigid.velocity.y == 0)
        {
            anim.SetBool("IsIdle", true);
        }
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
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }
}
