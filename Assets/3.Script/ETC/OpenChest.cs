using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChest : MonoBehaviour
{
    [SerializeField] private bool isCanOpen;
    private Animator chestAnim;

    private void Start()
    {
        chestAnim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isCanOpen = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isCanOpen = false;
        }
    }

    private void Update()
    {
        if(isCanOpen)
        {
            if(Input.GetKeyDown(GameManager.instance.InteractionKey))
            {
                chestAnim.SetBool("IsOpen", true);
            }
        }
    }
}
