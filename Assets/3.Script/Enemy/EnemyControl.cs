using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [SerializeField] private PlayerControl playerControl;
    [SerializeField] private EnemyList enemyList;
    [SerializeField] private float enemyHealth;
    private SpriteRenderer sprender;

    public bool isDamaged = false;

    private void Awake()
    {
        playerControl = FindObjectOfType<PlayerControl>();
        sprender = GetComponent<SpriteRenderer>();
        enemyHealth = enemyList.enemyHP;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            enemyHealth -= playerControl.damage;
            sprender.color = Color.red;
            Invoke("Hit", 0.3f);
            isDamaged = true;
            Debug.Log(enemyHealth);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            sprender.color = Color.white;
            isDamaged = false;
        }
    }

    private void Hit()
    {
        sprender.color = Color.white;
    }

    private void Update()
    {
        if(enemyHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    /*public class Node
    {
        public bool walkable;
        public Vector2 worldPosition;
        public int gridX;
        public int gridY;

        public int gCost;
        public int hCost;

        public Node parent;

        public Node(bool walk, Vector2 worldPos, int _gridX, int _gridY)
        {
            walkable = walk;
            worldPosition = worldPos;
            gridX = _gridX;
            gridY = _gridY;
        }

        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }
    }*/
}
