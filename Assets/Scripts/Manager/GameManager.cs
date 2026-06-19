using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private double totalGold = 0;
    [SerializeField] private double currentDPS = 0;

    public static Action<double> OnGoldChanged;
    public static Action<double> OnDPSChanged;

    private float goldTimer = 0f;
    private float currentDPSTimer = 0f;

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

    private void Update()
    {
        goldTimer += Time.deltaTime;
        currentDPSTimer += Time.deltaTime;

        if(goldTimer >= 1f)
        {
            totalGold += currentDPS;
            OnGoldChanged?.Invoke(totalGold);
            goldTimer = 0f;
        }
    }

    public double TotalGold => totalGold;

    public void AddGold(double amount)
    {
        totalGold += amount;
        OnGoldChanged?.Invoke(totalGold);
    }

    public void DamageTarget()
    {
        AddGold(1);
    }

    public void RecalculateDPS()
    {
        double newDPS = 0;

        foreach(UpgradeData data in UpgradeManager.Instance.upgrades)
        {
            newDPS += data.dpsBonus * data.currentLevel;
        }

        currentDPS = newDPS;
        OnDPSChanged?.Invoke(currentDPS);
    }
}
