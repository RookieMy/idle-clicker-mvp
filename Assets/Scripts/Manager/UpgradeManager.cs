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
}
