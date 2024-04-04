using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallCheck : MonoBehaviour
{
    [SerializeField] private PlayerControl playerCon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            playerCon.isWall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            playerCon.isWall = false;
        }
        else return;
    }
}
