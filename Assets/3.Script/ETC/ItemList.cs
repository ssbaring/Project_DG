using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObject/ItemLists")]
public class ItemList : ScriptableObject
{
    public enum ItemType
    {
        equipment_Head = 2,
        equipment_Armor = 4,
        equipment_Shoes = 8,
        equipment_Weapon = 16,
        consumable = 32,
        material = 64,
        all = equipment_Head | equipment_Armor | equipment_Shoes | equipment_Weapon | consumable | material
    }
    public ItemType itemType;           //아이템 타입
    public int itemId;                  //아이템 ID

    [Header("Body")]
    public float armor;                 //아이템 방어력
    public float agility;               //아이템 회피율
    public float attack_speed;          //아이템 공격속도
    [Header("Weapon")]
    public float attack_dmg;            //아이템 데미지
    public float stun_dmg;              //아이템 기절데미지
    [Header("Shoes")]
    public float move_speed;            //아이템 이동속도
    [Header("Equipment")]
    public float critical_rate;         //아이템 크리티컬확률
    [Header("Consumable")]
    public float restoreHP;
    public float restoreMP;
    public float attackUp;

    [Header("Infomation")]
    public string itemName;
    [TextArea]
    public string itemExplain;

    [Header("Sprite")]
    public Sprite itemSprite;
}
