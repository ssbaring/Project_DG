using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChest : MonoBehaviour
{
    [SerializeField] private bool isCanOpen;
    [SerializeField] private bool isOpened = false;
    [SerializeField] private BoxCollider2D itemGet;
    public ItemList item;
    public SpriteRenderer ItemSprite;
    private Animator chestAnim;
    private void Start()
    {
        chestAnim = GetComponent<Animator>();

        ItemSprite.sprite = item.itemSprite;
        ItemSprite.color = new Color(1, 1, 1, 0);
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
                isOpened = true;
            }
        }

        if (chestAnim.GetBool("IsOpen") && isOpened)
        {
            StartCoroutine(ItemSpawn());
            isOpened = false;
            StopCoroutine(ItemSpawn());
        }
    
    
    }

    private IEnumerator ItemSpawn()
    {
        yield return new WaitForSeconds(0.5f);
        float alpha = 0;
        while (ItemSprite.color.a < 1)
        {
            ItemSprite.color = new Color(1, 1, 1, alpha);
            alpha += 0.01f;
            yield return null;
        }
        itemGet.enabled = true;
    }
}
