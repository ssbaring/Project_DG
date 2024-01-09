using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileWater : MonoBehaviour
{
    private PlayerRayCheck check;
    private void Awake()
    {
        check = FindObjectOfType<PlayerRayCheck>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            check.isWater = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            check.isWater = false;
        }
    }
}
