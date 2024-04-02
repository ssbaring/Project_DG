using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileWater : MonoBehaviour
{
    [SerializeField] private PlayerControl check;
    private void Start()
    {
        check = FindObjectOfType<PlayerControl>();
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
        if (collision.CompareTag("Player"))
        {
            check.isWater = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            check.isWater = false;
        }
    }
}
