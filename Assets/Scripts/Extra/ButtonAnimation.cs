using UnityEngine;


public class ButtonAnimation : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float scaleMultiplier = 1.2f;
    [SerializeField] private float animationDuration = 0.2f;

    public void AnimateButton()
    {
        LeanTween.scale(gameObject, Vector3.one * scaleMultiplier, animationDuration).setEaseInOutSine().setOnComplete(() =>
        {
            LeanTween.scale(gameObject, Vector3.one, animationDuration).setEaseInOutSine();
        });
    }


}
