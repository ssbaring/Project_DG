using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject skill;

    public KeyCode inventoryKey = KeyCode.I;
    public KeyCode menuKey = KeyCode.Escape;
    public KeyCode skillKey = KeyCode.K;

    private void Update()
    {
        Inventory_Active();
        Menu_Active();
        Skill_Active();
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
