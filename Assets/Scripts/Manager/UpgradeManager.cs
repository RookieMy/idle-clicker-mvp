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
        for (int i = 0; i < UpgradeManager.Instance.upgrades.Count; i++)
        {
            PlayerPrefs.SetInt($"UpgradeLevel_{i}", UpgradeManager.Instance.upgrades[i].currentLevel);
        }
    }

    public void LoadUpgradeLevels()
    {
        for (int i = 0; i < UpgradeManager.Instance.upgrades.Count; i++)
        {
            if (PlayerPrefs.HasKey($"UpgradeLevel_{i}"))
            {
                upgrades[i].currentLevel = PlayerPrefs.GetInt($"UpgradeLevel_{i}");
            }
        }
    }
}
