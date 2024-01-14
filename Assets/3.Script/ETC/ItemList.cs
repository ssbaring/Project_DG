using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObject/ItemLists")]
public class ItemList : ScriptableObject
{
    public enum ItemType
    {
        equipment_Head = 0,
        equipment_Body,
        equipment_Shoes,
        equipment_Weapon,
        consumable,
        material 
    }
    public ItemType itemType;           //������ Ÿ��
    public int itemId;                  //������ ID

    [Header("Body")]
    public float armor;                 //������ ����
    public float agility;               //������ ȸ����
    public float attack_speed;          //������ ���ݼӵ�
    [Header("Weapon")]
    public float attack_dmg;            //������ ������
    public float stun_dmg;              //������ ����������
    [Header("Shoes")]
    public float move_speed;            //������ �̵��ӵ�
    [Header("Equipment")]
    public float critical_rate;         //������ ũ��Ƽ��Ȯ��
    [Header("Consumable")]
    public float restoreHP;
    public float restoreMP;
    public float attackUp;

    [Header("Sprite")]
    public Sprite itemSprite;
}
