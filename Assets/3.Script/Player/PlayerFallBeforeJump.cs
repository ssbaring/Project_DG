using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallBeforeJump : MonoBehaviour
{
    [SerializeField] private PlayerControl playerCon;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            playerCon.isCanJump = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            playerCon.isCanJump = false;
        }
    }
}
