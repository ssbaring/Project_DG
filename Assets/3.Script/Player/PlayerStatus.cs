using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStatus : PlayerControl
{

    [Header("Status Level")]
    public int strengthLevel = 0;
    public int agilityLevel = 0;
    public int speedLevel = 0;
    public int criticalLevel = 0;
    public int playerLevel = 1;
    public int skillPoint = 1;
    [Space(10f)]
    public float playerMaxEXP = 100;
    public float playerCurrentEXP = 0;
    [Space(10f)]
    [SerializeField] private int curseStack = 0;
    [SerializeField] private int MaxCurseStack = 4;

    [Header("Item")]
    public Transform Helmet;
    public Transform Armor;
    public Transform Shoes;
    public Transform Weapon;
    /*    public float HelmetDMG
    {
        get
        {
            return Weapon.GetComponentInChildren<ItemSlot>() != null ? Weapon.GetComponentInChildren<ItemSlot>().item.attack_dmg : 0;
        }
    }*/
    #region Get Equipments Value


    public float EquipmentDamage
    {
        get
        {
            return  (Helmet.GetComponentInChildren<ItemSlot>()  != null ? Helmet.GetComponentInChildren<ItemSlot>().item.attack_dmg : 0)
                +   (Armor.GetComponentInChildren<ItemSlot>()   != null ? Armor.GetComponentInChildren<ItemSlot>().item.attack_dmg  : 0)
                +   (Shoes.GetComponentInChildren<ItemSlot>()   != null ? Shoes.GetComponentInChildren<ItemSlot>().item.attack_dmg  : 0)
                +   (Weapon.GetComponentInChildren<ItemSlot>()  != null ? Weapon.GetComponentInChildren<ItemSlot>().item.attack_dmg : 0);
        }
    }


    public float EquipmentStun
    {
        get
        {
            return  (Helmet.GetComponentInChildren<ItemSlot>()  != null ? Helmet.GetComponentInChildren<ItemSlot>().item.stun_dmg : 0)
                +   (Armor.GetComponentInChildren<ItemSlot>()   != null ? Armor.GetComponentInChildren<ItemSlot>().item.stun_dmg  : 0)
                +   (Shoes.GetComponentInChildren<ItemSlot>()   != null ? Shoes.GetComponentInChildren<ItemSlot>().item.stun_dmg  : 0)
                +   (Weapon.GetComponentInChildren<ItemSlot>()  != null ? Weapon.GetComponentInChildren<ItemSlot>().item.stun_dmg : 0);
        }
    }

    public float EquipmentArmor
    {
        get
        {
            return  (Helmet.GetComponentInChildren<ItemSlot>()  != null ? Helmet.GetComponentInChildren<ItemSlot>().item.armor : 0)
                +   (Armor.GetComponentInChildren<ItemSlot>()   != null ? Armor.GetComponentInChildren<ItemSlot>().item.armor  : 0)
                +   (Shoes.GetComponentInChildren<ItemSlot>()   != null ? Shoes.GetComponentInChildren<ItemSlot>().item.armor  : 0);
        }
    }

    public float EquipmentAgility
    {
        get
        {
            return  (Helmet.GetComponentInChildren<ItemSlot>()  != null ? Helmet.GetComponentInChildren<ItemSlot>().item.agility : 0)
                +   (Armor.GetComponentInChildren<ItemSlot>()   != null ? Armor.GetComponentInChildren<ItemSlot>().item.agility  : 0)
                +   (Shoes.GetComponentInChildren<ItemSlot>()   != null ? Shoes.GetComponentInChildren<ItemSlot>().item.agility  : 0);
        }
    }

    public float EquipmentAttackSpeed
    {
        get
        {
            return  (Helmet.GetComponentInChildren<ItemSlot>()  != null ? Helmet.GetComponentInChildren<ItemSlot>().item.attack_speed : 0)
                +   (Armor.GetComponentInChildren<ItemSlot>()   != null ? Armor.GetComponentInChildren<ItemSlot>().item.attack_speed  : 0)
                +   (Shoes.GetComponentInChildren<ItemSlot>()   != null ? Shoes.GetComponentInChildren<ItemSlot>().item.attack_speed  : 0)
                +   (Weapon.GetComponentInChildren<ItemSlot>()  != null ? Weapon.GetComponentInChildren<ItemSlot>().item.attack_speed : 0);
        }
    }

    public float EquipmentMoveSpeed
    {
        get
        {
            return Shoes.GetComponentInChildren<ItemSlot>() != null ? Shoes.GetComponentInChildren<ItemSlot>().item.attack_speed : 0;
        }
    }

    public float EquipmentCriticalRate
    {
        get
        {
            return  (Helmet.GetComponentInChildren<ItemSlot>()  != null ? Helmet.GetComponentInChildren<ItemSlot>().item.critical_rate : 0)
                +   (Armor.GetComponentInChildren<ItemSlot>()   != null ? Armor.GetComponentInChildren<ItemSlot>().item.critical_rate  : 0)
                +   (Shoes.GetComponentInChildren<ItemSlot>()   != null ? Shoes.GetComponentInChildren<ItemSlot>().item.critical_rate  : 0)
                +   (Weapon.GetComponentInChildren<ItemSlot>()  != null ? Weapon.GetComponentInChildren<ItemSlot>().item.critical_rate : 0);
        }
    }
    #endregion

    public bool isRevive = false;
    public float trueDamage;
    public float trueStunDamage;
    public float knockback = 0.2f;

    public List<GameObject> DeadEnemyList;

    public int CurseStack
    {
        get
        {
            return curseStack;
        }
        set
        {
            curseStack = value;
            if (curseStack >= MaxCurseStack)
            {
                isDead = true;
                anim.SetBool("IsCurseDie", true);
                rigid.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            else
            {
                anim.SetBool("IsCurseDie", false);
                rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                Debug.Log(curseStack);
            }

        }
    }

    public bool isBackAttack = false;
    public bool isCriticalAttack = false;

    private PlayerRayCheck check;
    private void Awake()
    {
        check = GetComponent<PlayerRayCheck>();
    }

    public float TrueDamage(float defaultDmg)
    {
        trueDamage = (((1 + strengthLevel) * 1.8f) * defaultDmg + EquipmentDamage);
        return trueDamage;
    }

    public float TrueStunDamage(float defaultSDmg)
    {
        trueStunDamage = (((1 + strengthLevel) * 1.8f) * defaultSDmg + EquipmentStun);
        return trueStunDamage;
    }

    public float TrueSpeedAnimation()
    {
        float trueSpeed = check.isWater ? 0.5f : (speedLevel * 0.1f) + 1;
        return trueSpeed;
    }

    public void GetPlayerEXP(int enemyEXP)
    {
        playerCurrentEXP += enemyEXP;
    }

    private void PlayerLevelUP()
    {
        if (playerCurrentEXP >= playerMaxEXP)
        {
            playerLevel++;
            skillPoint++;
            playerCurrentEXP -= playerMaxEXP;
            playerMaxEXP = Mathf.CeilToInt((playerMaxEXP + ((playerLevel - 1) * 20)) * 1.3f);
        }
        else
        {
            return;
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        PlayerLevelUP();

        //리스폰
        if (Input.GetKeyDown(GameManager.instance.respawnKey))
        {
            transform.position = GameManager.instance.respawnPoint.position;
            rigid.velocity = Vector2.zero;
            for (int i = 0; i < DeadEnemyList.Count; i++)
            {
                //todo... 100숫자 수정하기
                DeadEnemyList[i].GetComponent<EnemyStatus>().EnemyStun = 100;
                DeadEnemyList[i].GetComponent<EnemyStatus>().EnemyHealth = 100;
                DeadEnemyList[i].SetActive(true);
            }
            DeadEnemyList.Clear();
            CurseStack = 0;
            isDead = false;
        }
    }

    #region Overriding

    public override float Damage()
    {
        float defalutDamage = base.Damage();
        float finalDamage = TrueDamage(defalutDamage) * (isBackAttack ? 1.5f : 1.0f) * (isCriticalAttack ? 2.0f : 1.0f);
        return finalDamage;
    }

    public override float StunDamage()
    {
        float defaultStunDamage = base.StunDamage();
        float finalStunDamage = TrueStunDamage(defaultStunDamage) * (isBackAttack ? 2.5f : 1.0f) * (isCriticalAttack ? 1.5f : 1.0f);
        return finalStunDamage;
    }

    public override float Speed()
    {
        float defaultSpeed = base.Speed();
        float finalSpeed = check.isWater ? 2.5f : speedLevel * 0.15f + defaultSpeed;
        return finalSpeed;
    }

    public override float AttackSpeed()
    {
        float defaultAttackSpeed = base.AttackSpeed();
        float finalAttackSpeed = speedLevel * 0.1f + defaultAttackSpeed;
        return finalAttackSpeed;
    }

    public override float CriticalProbability()
    {
        float defaultCriticalProbability = base.CriticalProbability();
        float finalCritical = (criticalLevel * 0.5f) + defaultCriticalProbability;
        return finalCritical;
    }

    #endregion

    public void CriticalHit()
    {
        isCriticalAttack = Random.Range(0, 100.0f) < CriticalProbability();
        if (isCriticalAttack)
        {
            Debug.LogWarning("Critical");
        }
    }




}
