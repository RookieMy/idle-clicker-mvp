using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    public List<UpgradeData> upgrades;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if(PlayerPrefs.HasKey("UpgradeLevels"))
        {
            string[] levels = PlayerPrefs.GetString("UpgradeLevels").Split(',');
            for(int i = 0; i < upgrades.Count && i < levels.Length; i++)
            {
                upgrades[i].currentLevel = int.Parse(levels[i]);
            }
        }

        LoadUpgradeLevels();
    }

    public void TryBuyUpgrade(UpgradeData data)
    {
        if(GameManager.Instance.TotalGold >= data.GetCurrentCost())
        {
            GameManager.Instance.AddGold(-data.GetCurrentCost());
            data.currentLevel++;
            GameManager.Instance.RecalculateDPS();
        }
    }

    public void ResetUpgradeLevels()
    {
        foreach(UpgradeData data in upgrades)
        {
            data.currentLevel = 0;
        }
        GameManager.Instance.RecalculateDPS();
    }

    public void SaveUpgradeLevels()
    {
        string levels = string.Join(",", upgrades.ConvertAll(u => u.currentLevel.ToString()).ToArray());
        PlayerPrefs.SetString("UpgradeLevels", levels);
    }

    public void LoadUpgradeLevels()
    {
        if(PlayerPrefs.HasKey("UpgradeLevels"))
        {
            string[] levels = PlayerPrefs.GetString("UpgradeLevels").Split(',');
            for(int i = 0; i < upgrades.Count && i < levels.Length; i++)
            {
                upgrades[i].currentLevel = int.Parse(levels[i]);
            }
        }
    }
}
