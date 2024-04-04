using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCeilCheck : MonoBehaviour
{
    [SerializeField] private PlayerControl playerCon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            playerCon.isCeiling = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            playerCon.isCeiling = false;
        }
        else return;
    }
}
