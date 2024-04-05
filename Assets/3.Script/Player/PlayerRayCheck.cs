using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRayCheck : MonoBehaviour
{
    [SerializeField] private PlayerControl playerCon;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            playerCon.isGround = true;
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            playerCon.isGround = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            playerCon.isGround = false;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            playerCon.isGround = true;
        }
    }

}
