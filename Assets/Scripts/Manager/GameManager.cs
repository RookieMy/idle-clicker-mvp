using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private double totalGold;
    [SerializeField] private double currentDPS;

    public static Action<double> OnGoldChanged;

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

    public void AddGold(double amount)
    {
        totalGold += amount;
        OnGoldChanged?.Invoke(totalGold);
    }
}
