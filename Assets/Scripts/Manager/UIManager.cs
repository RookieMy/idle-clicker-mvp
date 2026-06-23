using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI goldText;
    [SerializeField] private TMPro.TextMeshProUGUI dpsText;
    private void OnEnable()
    {
        GameManager.OnGoldChanged += UpdateGoldDisplay;
        GameManager.OnDPSChanged += UpdateDPSDisplay;
    }

    private void OnDisable()
    {
        GameManager.OnGoldChanged -= UpdateGoldDisplay;
        GameManager.OnDPSChanged -= UpdateDPSDisplay;
    }

    public void UpdateGoldDisplay(double totalGold)
    {
        goldText.text = "Gold: " + NumberFormatter.FormatNumber(GameManager.Instance.TotalGold);
    }
    
    public void UpdateDPSDisplay(double totalDPS)
    {
        dpsText.text = "DPS: " + NumberFormatter.FormatNumber(totalDPS)+"/sec";
    }
}
