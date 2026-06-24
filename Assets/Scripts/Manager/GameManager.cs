using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private double totalGold = 0;
    [SerializeField] private double currentDPS = 0;
    [SerializeField] private float timeToAutoSave = 60f;

    public static Action<double> OnGoldChanged;
    public static Action<double> OnDPSChanged;
    public static Action<double> OnOfflineEarningsCalculated;

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

        LoadGame();
        StartCoroutine(AutoSaveCoroutine());
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
        }
    }

    private IEnumerator AutoSaveCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeToAutoSave);
            SaveGame();
            Debug.Log("Game auto-saved.");
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
        PlayerPrefs.SetString("LastTimePlayed", DateTime.UtcNow.ToString());
        UpgradeManager.Instance.SaveUpgradeLevels();
    }

    public void ResetGame()
    {
        totalGold = 0;
        currentDPS = 0;
        OnGoldChanged?.Invoke(totalGold);
        OnDPSChanged?.Invoke(currentDPS);
        PlayerPrefs.DeleteAll();
    }

    public void LoadGame()
    {
        if(PlayerPrefs.HasKey("TotalGold"))
        {
            totalGold = double.Parse(PlayerPrefs.GetString("TotalGold"));
            OnGoldChanged?.Invoke(totalGold);
        }
        if(PlayerPrefs.HasKey("CurrentDPS"))
        {
            currentDPS = double.Parse(PlayerPrefs.GetString("CurrentDPS"));
            OnDPSChanged?.Invoke(currentDPS);
        }
        
        UpgradeManager.Instance.LoadUpgradeLevels();
        RecalculateDPS();

        if(PlayerPrefs.HasKey("LastTimePlayed"))
        {
            DateTime lastTimePlayed = DateTime.Parse(PlayerPrefs.GetString("LastTimePlayed"));
            TimeSpan timeAway = DateTime.UtcNow - lastTimePlayed;
            double offlineEarnings = currentDPS * timeAway.TotalSeconds;
            totalGold += offlineEarnings;
            OnGoldChanged?.Invoke(totalGold);
            if (offlineEarnings > 0)
                OnOfflineEarningsCalculated?.Invoke(offlineEarnings);
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
