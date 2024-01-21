using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public TextMeshProUGUI damageText;
    public GameObject textObject;
    public bool isStun;
    private PlayerStatus player;
    private RectTransform canvasRect;
    private float rangeDamageText = 18.0f;
    [Header("Speed")]
    public float textMoveSpeed = 2f;
    public float textAlphaSpeed = 0.2f;


    [Header("Time")]
    public float destroyTime = 0.5f;
    public float gameTime = 0;
    Color textAlpha;

    private void Start()
    {
        player = FindObjectOfType<PlayerStatus>();
        canvasRect = GetComponentInParent<RectTransform>();
        textAlpha = damageText.color;
        if (isStun)
        {
            StunDamage();
        }
        else
        {
            Damage();
        }
        Destroy(textObject, destroyTime);
    }


    private void Update()
    {
        /*gameTime += Time.deltaTime;
        if(gameTime < destroyTime)
        {
            textAlpha.a = Mathf.Lerp(textAlpha.a, 0, textAlphaSpeed * Time.deltaTime);
            damageText.color = textAlpha;
        }
        else
        {
            gameTime = 0;
        }*/
    }

    private void Damage()
    {
        float randomX = Random.Range(-rangeDamageText * 0.5f, rangeDamageText * 0.5f);
        float randomY = Random.Range(-rangeDamageText * 0.5f, rangeDamageText * 0.5f);

        textObject.transform.localPosition = new Vector3(randomX, randomY);

        if (player.isCriticalAttack)
        {
            damageText.color = Color.red;
        }
        else
        {
            damageText.color = Color.white;

        }

        damageText.text = string.Format("{0}", player.Damage());
    }

    private void StunDamage()
    {
        float randomX = Random.Range(-rangeDamageText * 0.5f, rangeDamageText * 0.5f);
        float randomY = Random.Range(-rangeDamageText * 0.5f, rangeDamageText * 0.5f);

        textObject.transform.localPosition = new Vector3(randomX, randomY);

        if (player.isCriticalAttack)
        {
            damageText.color = Color.red;
        }
        else
        {
            damageText.color = Color.white;

        }

        damageText.text = string.Format("{0}", player.StunDamage());
    }
}
