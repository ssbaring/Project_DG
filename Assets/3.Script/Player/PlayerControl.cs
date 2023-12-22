using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpPower = 5f;
    [SerializeField] private float posX;

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpCharacter();
        }

    }

    private void MoveCharacter()
    {
        posX = Input.GetAxis("Horizontal");

        Vector2 position = new Vector2(posX, 0) * speed;

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
        rigid.AddForce(Vector2.up * jumpPower);
    }
}
