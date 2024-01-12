using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPoint : MonoBehaviour
{
    private SpriteRenderer spriteRender;

    [SerializeField] private bool isCheckIn;

    private void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isCheckIn = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isCheckIn = false;
        }
    }


    private void Update()
    {
        if(isCheckIn)
        {
            if (Input.GetKeyDown(GameManager.instance.CheckPointKey))
            {
                GameManager.instance.respawnPoint.position = transform.position;
                StartCoroutine(Blink());
                Debug.Log("CheckPoint");
                isCheckIn = false;
            }
        }
    }

    private IEnumerator Blink()
    {
        int count = 0;
        while (count < 5)
        {
            spriteRender.color = Color.white;
            count++;
            yield return new WaitForSeconds(0.1f);
            //if (count >= 5) break;
            spriteRender.color = new Color32(108, 108, 108, 255);
            count++;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
