using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private double totalGold = 0;
    [SerializeField] private double currentDPS = 0;

    public static Action<double> OnGoldChanged;
    public static Action<double> OnDPSChanged;

    private float goldTimer = 0f;
    private float currentDPSTimer = 0f;
    [SerializeField] private float tickInterval = 5f;

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

        if(PlayerPrefs.HasKey("TotalGold"))
        {
            totalGold = double.Parse(PlayerPrefs.GetString("TotalGold"));
        }
        if(PlayerPrefs.HasKey("CurrentDPS"))
        {
            currentDPS = double.Parse(PlayerPrefs.GetString("CurrentDPS"));
        }
    }

    private void Update()
    {
        goldTimer += Time.deltaTime;
        currentDPSTimer += Time.deltaTime;

        if(goldTimer >= tickInterval)
        {
            totalGold += currentDPS;
            OnGoldChanged?.Invoke(totalGold);
            goldTimer = 0f;
            SaveGame();
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
        Vector2 clickPos = Pointer.current.position.ReadValue();
        ObjectPoolManager.Instance.GetPooledObject(clickPos, "+1");
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

    public void SaveGame()
    {
        PlayerPrefs.SetString("TotalGold", totalGold.ToString());
        PlayerPrefs.SetString("CurrentDPS", currentDPS.ToString());
        for(int i = 0; i < UpgradeManager.Instance.upgrades.Count; i++)
        {
            PlayerPrefs.SetInt($"UpgradeLevel_{i}", UpgradeManager.Instance.upgrades[i].currentLevel);
        }
    }

    public void ResetGame()
    {
        totalGold = 0;
        currentDPS = 0;
        OnGoldChanged?.Invoke(totalGold);
        OnDPSChanged?.Invoke(currentDPS);
        PlayerPrefs.DeleteAll();
    }
}
