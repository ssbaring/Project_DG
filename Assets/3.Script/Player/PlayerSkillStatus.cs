using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerSkillStatus : MonoBehaviour
{
    public TextMeshProUGUI StrengthLevelText;
    public TextMeshProUGUI AgilityLevelText;
    public TextMeshProUGUI SpeedLevelText;
    public TextMeshProUGUI CriticalLevelText;

    public TextMeshProUGUI StrengthValueText;
    public TextMeshProUGUI StunValueText;
    public TextMeshProUGUI SpeedValueText;
    public TextMeshProUGUI CriticalValueText;

    [SerializeField] private int maxLevel = 20;

    private PlayerStatus level;

    private void Start()
    {
        level = FindObjectOfType<PlayerStatus>();
    }

    protected void Update()
    {
        StrengthLevelText.text = string.Format("+{0}", level.strengthLevel);
        AgilityLevelText.text = string.Format("+{0}", level.agilityLevel);
        SpeedLevelText.text = string.Format("+{0}", level.speedLevel);
        CriticalLevelText.text = string.Format("+{0}", level.criticalLevel);

        StrengthValueText.text = string.Format("{0}", level.TrueDamage(level.defalutDamage));
        StunValueText.text = string.Format("{0}", level.TrueStunDamage(level.defaultStunDamage));
        SpeedValueText.text = string.Format("{0}", level.Speed());
        CriticalValueText.text = string.Format("{0}", level.CriticalProbability());
    }

    public void StrengthLevelUp()
    {
        if (level.strengthLevel < maxLevel)
        {
            level.strengthLevel = level.strengthLevel + 1;
        }
        else
        {
            level.strengthLevel = maxLevel;
        }

    }
    public void AgilityLevelUp()
    {
        if (level.agilityLevel < maxLevel)
        {
            level.agilityLevel = level.agilityLevel + 1;
        }
        else
        {
            level.agilityLevel = maxLevel;
        }
    }
    public void SpeedLevelUp()
    {
        if (level.speedLevel < maxLevel)
        {
            level.speedLevel = level.speedLevel + 1;
        }
        else
        {
            level.speedLevel = maxLevel;
        }
    }
    public void CriticalLevelUp()
    {
        if (level.criticalLevel < maxLevel)
        {
            level.criticalLevel = level.criticalLevel + 1;
        }
        else
        {
            level.criticalLevel = maxLevel;
        }
    }
    //UP
    //==================================================================
    //DOWN
    public void StrengthLevelDown()
    {
        if (level.strengthLevel > 0)
        {
            level.strengthLevel = level.strengthLevel - 1;
        }
        else
        {
            level.strengthLevel = 0;
        }
    }

    public void AgilityLevelDown()
    {
        if (level.agilityLevel > 0)
        {
            level.agilityLevel = level.agilityLevel - 1;
        }
        else
        {
            level.agilityLevel = 0;
        }

    }

    public void SpeedLevelDown()
    {
        if (level.speedLevel > 0)
        {
            level.speedLevel = level.speedLevel - 1;
        }
        else
        {
            level.speedLevel = 0;
        }
    }

    public void CriticalLevelDown()
    {
        if (level.criticalLevel > 0)
        {
            level.criticalLevel = level.criticalLevel - 1;
        }
        else
        {
            level.criticalLevel = 0;
        }
    }
}
