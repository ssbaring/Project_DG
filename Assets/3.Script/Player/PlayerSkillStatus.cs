using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;



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

    public TextMeshProUGUI SkillPointCount;

    [SerializeField] private int maxLevel = 20;

    [SerializeField] private PlayerStatus level;


    private void OnEnable()
    {
        level = FindObjectOfType<PlayerStatus>();
    }

    private void Start()
    {
        StrengthValueText.text = string.Format("{0}", level.TrueDamage(level.defalutDamage));
        StunValueText.text = string.Format("{0}", level.TrueStunDamage(level.defaultStunDamage));
        SpeedValueText.text = string.Format("{0}", level.Speed());
        CriticalValueText.text = string.Format("{0}%", level.CriticalProbability());
    }

    private void Update()
    {
        if (level.playerCurrentEXP >= level.playerMaxEXP)
        {
            SkillPointCount.text = string.Format("{0}", level.skillPoint);
        }
    }

    //Button

    public void StrengthLevelUp()
    {
        if (level.strengthLevel < maxLevel && level.skillPoint >= 1)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (level.strengthLevel >= maxLevel) return;
                else if (level.strengthLevel < maxLevel)
                {
                    if (level.strengthLevel + level.skillPoint <= maxLevel)
                    {
                        level.strengthLevel += level.skillPoint;
                        level.skillPoint -= level.skillPoint;
                    }
                    else if (level.strengthLevel + level.skillPoint > maxLevel)
                    {
                        level.skillPoint -= maxLevel - level.strengthLevel;
                        level.strengthLevel += maxLevel - level.strengthLevel;
                    }
                }
            }
            else
            {
                level.strengthLevel++;
                level.skillPoint--;
            }
        }
        else if (level.strengthLevel >= maxLevel)
        {
            level.strengthLevel = maxLevel;
        }
        else
        {
            return;
        }
        StrengthLevelText.text = string.Format("+{0}", level.strengthLevel);
        SkillPointCount.text = string.Format("{0}", level.skillPoint);
        StrengthValueText.text = string.Format("{0}", level.TrueDamage(level.defalutDamage));
        StunValueText.text = string.Format("{0}", level.TrueStunDamage(level.defaultStunDamage));
    }
    public void AgilityLevelUp()
    {
        if (level.agilityLevel < maxLevel && level.skillPoint >= 1)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (level.agilityLevel >= maxLevel) return;
                else if (level.agilityLevel < maxLevel)
                {
                    if (level.agilityLevel + level.skillPoint <= maxLevel)
                    {
                        level.agilityLevel += level.skillPoint;
                        level.skillPoint -= level.skillPoint;
                    }
                    else if (level.agilityLevel + level.skillPoint > maxLevel)
                    {
                        level.skillPoint -= maxLevel - level.agilityLevel;
                        level.agilityLevel += maxLevel - level.agilityLevel;
                    }
                }
            }
            else
            {
                level.agilityLevel++;
                level.skillPoint--;
            }
        }
        else if (level.agilityLevel >= maxLevel)
        {
            level.agilityLevel = maxLevel;
        }
        else
        {
            return;
        }
        AgilityLevelText.text = string.Format("+{0}", level.agilityLevel);
        SkillPointCount.text = string.Format("{0}", level.skillPoint);
    }
    public void SpeedLevelUp()
    {
        if (level.speedLevel < maxLevel && level.skillPoint >= 1)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (level.speedLevel >= maxLevel) return;
                else if (level.speedLevel < maxLevel)
                {
                    if (level.speedLevel + level.skillPoint <= maxLevel)
                    {
                        level.speedLevel += level.skillPoint;
                        level.skillPoint -= level.skillPoint;
                    }
                    else if (level.speedLevel + level.skillPoint > maxLevel)
                    {
                        level.skillPoint -= maxLevel - level.speedLevel;
                        level.speedLevel += maxLevel - level.speedLevel;
                    }
                }
            }
            else
            {
                level.speedLevel++;
                level.skillPoint--;
            }
        }
        else if (level.speedLevel >= maxLevel)
        {
            level.speedLevel = maxLevel;
        }
        else
        {
            return;
        }
        SpeedLevelText.text = string.Format("+{0}", level.speedLevel);
        SkillPointCount.text = string.Format("{0}", level.skillPoint);
        SpeedValueText.text = string.Format("{0}", level.Speed());
    }
    public void CriticalLevelUp()
    {
        if (level.criticalLevel < maxLevel && level.skillPoint >= 1)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (level.criticalLevel >= maxLevel) return;
                else if (level.criticalLevel < maxLevel)
                {
                    if (level.criticalLevel + level.skillPoint <= maxLevel)
                    {
                        level.criticalLevel += level.skillPoint;
                        level.skillPoint -= level.skillPoint;
                    }
                    else if (level.criticalLevel + level.skillPoint > maxLevel)
                    {
                        level.skillPoint -= maxLevel - level.criticalLevel;
                        level.criticalLevel += maxLevel - level.criticalLevel;
                    }
                }
            }
            else
            {
                level.criticalLevel++;
                level.skillPoint--;
            }
        }
        else if (level.criticalLevel >= maxLevel)
        {
            level.criticalLevel = maxLevel;
        }
        else
        {
            return;
        }
        CriticalLevelText.text = string.Format("+{0}", level.criticalLevel);
        SkillPointCount.text = string.Format("{0}", level.skillPoint);
        CriticalValueText.text = string.Format("{0}%", level.CriticalProbability());
    }

    //UP
    //==================================================================
    //DOWN
    public void StrengthLevelDown()
    {
        if (level.strengthLevel > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                level.skillPoint += level.strengthLevel;
                level.strengthLevel -= level.strengthLevel;
            }
            else
            {
                level.strengthLevel--;
                level.skillPoint++;
            }
        }
        else
        {
            level.strengthLevel = 0;
        }
        StrengthLevelText.text = string.Format("+{0}", level.strengthLevel);
        SkillPointCount.text = string.Format("{0}", level.skillPoint);
        StrengthValueText.text = string.Format("{0}", level.TrueDamage(level.defalutDamage));
        StunValueText.text = string.Format("{0}", level.TrueStunDamage(level.defaultStunDamage));
    }
    public void AgilityLevelDown()
    {
        if (level.agilityLevel > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                level.skillPoint += level.agilityLevel;
                level.agilityLevel -= level.agilityLevel;
            }
            else
            {
                level.agilityLevel--;
                level.skillPoint++;
            }
        }
        else
        {
            level.agilityLevel = 0;
        }
        AgilityLevelText.text = string.Format("+{0}", level.agilityLevel);
        SkillPointCount.text = string.Format("{0}", level.skillPoint);
    }
    public void SpeedLevelDown()
    {
        if (level.speedLevel > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                level.skillPoint += level.speedLevel;
                level.speedLevel -= level.speedLevel;
            }
            else
            {
                level.speedLevel--;
                level.skillPoint++;
            }
        }
        else
        {
            level.speedLevel = 0;
        }
        SpeedLevelText.text = string.Format("+{0}", level.speedLevel);
        SkillPointCount.text = string.Format("{0}", level.skillPoint);
        SpeedValueText.text = string.Format("{0}", level.Speed());
    }
    public void CriticalLevelDown()
    {
        if (level.criticalLevel > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                level.skillPoint += level.criticalLevel;
                level.criticalLevel -= level.criticalLevel;
            }
            else
            {
                level.criticalLevel--;
                level.skillPoint++;
            }
        }
        else
        {
            level.criticalLevel = 0;
        }
        CriticalLevelText.text = string.Format("+{0}", level.criticalLevel);
        SkillPointCount.text = string.Format("{0}", level.skillPoint);
        CriticalValueText.text = string.Format("{0}%", level.CriticalProbability());
    }
}
