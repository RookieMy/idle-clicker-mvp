using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FloatingText : MonoBehaviour
{
    [Header("Floating Text Settings")]
    [SerializeField] private float floatSpeed = 1f; // Speed at which the text floats upwards
    [SerializeField] private float fadeDuration = 1f; // Duration for the text to fade out
    [SerializeField] private Vector3 floatDirection = Vector3.up; // Direction in which the text floats

    private TextMeshProUGUI textMesh;
    private float _timer;
    private Color originalColor;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        _timer = fadeDuration;
        originalColor = textMesh.color;
    }

    private void Update()
    {
        GetComponent<RectTransform>().anchoredPosition += Vector2.up * floatSpeed * Time.deltaTime;
        Color currentColor = textMesh.color;
        currentColor.a -= Time.deltaTime / fadeDuration; // Fade out the text over time
        _timer -= Time.deltaTime;

        if(_timer <= 0)
        {
            _timer = fadeDuration; // Reset the timer for the next use
            textMesh.color = originalColor; // Reset the text color to its original state
            ObjectPoolManager.Instance.ReturnToPool(gameObject); // Return the text to the pool when done
        }
        else
        {
            textMesh.color = currentColor; // Update the text color with the new alpha value
        }
    }
}
