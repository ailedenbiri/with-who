using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class FlushButton : MonoBehaviour, IPointerClickHandler
{
    public float scaleBoinkStrength = 0.2f; // Strength of the scale boink effect
    public float scaleBoinkDuration = 0.15f; // Duration of the scale boink effect
    public float positionBoinkStrength = 0.6f; // Strength of the position boink effect
    public float positionBoinkDuration = 0.15f; // Duration of the position boink effect

    private Vector3 originalScale;
    private Vector3 originalPosition;

    private void Start()
    {
        // Store the original scale and position of the object
        originalScale = transform.localScale;
        originalPosition = transform.position;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (TryGetComponent<Button>(out Button b))
        {
            if (b.interactable == false)
            {
                return;
            }

        }
        if (TryGetComponent<Animator>(out Animator a))
        {
            a.enabled = false;
        }

        if (scaleBoinkStrength != 0)
        {
            // Scale up and then back to the original scale using DoTween
            transform.DOScale(originalScale * (1 + scaleBoinkStrength), scaleBoinkDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    transform.DOScale(originalScale, scaleBoinkDuration)
                        .SetEase(Ease.OutQuad)
                        .OnComplete(() =>
                        {
                            if (a) a.enabled = true;
                        });
                });
        }

        if (positionBoinkStrength != 0)
        {
            // Move the object upward and then back to its original position using DoTween
            transform.DOMoveY(originalPosition.y + positionBoinkStrength, positionBoinkDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    transform.DOMove(originalPosition, positionBoinkDuration)
                        .SetEase(Ease.OutQuad)
                        .OnComplete(() =>
                        {
                            if (a) a.enabled = true;
                        });
                });
        }
    }
}