using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI goldText;
    private void OnEnable()
    {
        GameManager.OnGoldChanged += UpdateGoldDisplay;
    }

    private void OnDisable()
    {
        GameManager.OnGoldChanged -= UpdateGoldDisplay;
    }

    public void UpdateGoldDisplay(double totalGold)
    {
        goldText.text = $"Gold: {totalGold:N0}";
    }
}
