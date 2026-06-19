using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "IdleGame/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    public float baseCost;
    public float costMultiplier;
    public float dpsBonus;

    [Header("Runtime Data")]
    public int currentLevel;

    public double GetCurrentCost()
    {
        return baseCost * Mathf.Pow(costMultiplier, currentLevel);
    }
}
