using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI goldText;
    [SerializeField] private TMPro.TextMeshProUGUI dpsText;
    [SerializeField] private TMPro.TextMeshProUGUI offlineEarningsText;
    [SerializeField] private GameObject offlineEarningPanel;
    private void OnEnable()
    {
        GameManager.OnGoldChanged += UpdateGoldDisplay;
        GameManager.OnDPSChanged += UpdateDPSDisplay;
        GameManager.OnOfflineEarningsCalculated += UpdateOfflineEarningsDisplay;
    }

    private void OnDisable()
    {
        GameManager.OnGoldChanged -= UpdateGoldDisplay;
        GameManager.OnDPSChanged -= UpdateDPSDisplay;
        GameManager.OnOfflineEarningsCalculated -= UpdateOfflineEarningsDisplay;
    }

    public void UpdateGoldDisplay(double totalGold)
    {
        if (goldText == null)
        {
            Debug.LogError("Gold Text is not assigned in the inspector.");
            return;
        }
        goldText.text = "Gold: " + NumberFormatter.FormatNumber(GameManager.Instance.TotalGold);
    }
    
    public void UpdateDPSDisplay(double totalDPS)
    {
        if (dpsText == null)
        {
            Debug.LogError("DPS Text is not assigned in the inspector.");
            return;
        }
        dpsText.text = "DPS: " + NumberFormatter.FormatNumber(totalDPS)+"/sec";
    }

    public void UpdateOfflineEarningsDisplay(double offlineEarnings)
    {
        if (offlineEarningsText == null || offlineEarningPanel == null)
        {
            Debug.LogError("Offline Earnings Text or Panel is not assigned in the inspector.");
            return;
        }

        offlineEarningsText.text = "Offline Earnings: " + NumberFormatter.FormatNumber(offlineEarnings);
        offlineEarningPanel.SetActive(true);
    }
}
