using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRayCheck : MonoBehaviour
{
    [SerializeField] private PlayerControl playerCon;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Debug.Log("Ground");
            playerCon.isGround = true;
            playerCon.isWallGrap = false;
            playerCon.isCanJump = true;
            playerCon.rigid.gravityScale = playerCon.gravity;
            //playerCon.groundCollider.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            playerCon.isGround = false;
        }
    }

}
