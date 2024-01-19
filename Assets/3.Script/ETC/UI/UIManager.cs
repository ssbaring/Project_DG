using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    private void Awake()
    {
        #region SingleTon
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
    }

    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject skill;
    [SerializeField] private GameObject option;

    public KeyCode inventoryKey = KeyCode.I;
    public KeyCode menuKey = KeyCode.Escape;
    public KeyCode skillKey = KeyCode.K;

    private void Update()
    {
        if (SceneManager.GetActiveScene().name.Equals("TitleScene")) return;
        else
        {
            if (inventory.activeSelf || skill.activeSelf || option.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    inventory.SetActive(false);
                    skill.SetActive(false);
                    option.SetActive(false);
                }
            }
            else if (!inventory.activeSelf || !skill.activeSelf || !menu.activeSelf)
            {
                Menu_Active();
            }

            if (!menu.activeSelf)
            {
                Inventory_Active();
                Skill_Active();
            }
        }
    }

    private void Inventory_Active()
    {
        if (Input.GetKeyDown(inventoryKey) && !inventory.activeSelf)
        {
            inventory.SetActive(true);
        }
        else if (Input.GetKeyDown(inventoryKey) && inventory.activeSelf)
        {
            inventory.SetActive(false);
        }
    }

    private void Menu_Active()
    {
        if (Input.GetKeyDown(menuKey) && !menu.activeSelf)
        {
            menu.SetActive(true);
        }
        else if (Input.GetKeyDown(menuKey) && menu.activeSelf)
        {
            menu.SetActive(false);
        }
    }

    private void Skill_Active()
    {
        if (Input.GetKeyDown(skillKey) && !skill.activeSelf)
        {
            skill.SetActive(true);
        }
        else if (Input.GetKeyDown(skillKey) && skill.activeSelf)
        {
            skill.SetActive(false);
        }
    }
}
