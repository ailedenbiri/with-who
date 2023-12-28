using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class RewardedButton : MonoBehaviour
{
    [SerializeField] Image boosterIndicatorGreen;
    [SerializeField] Image boosterIndicatorBlack;
    [SerializeField] float boosterTime = 30f;
    [SerializeField] GameObject adImageGameObject;
    [SerializeField] float adImageScaleDuration = 0.5f;

    private Button button;
    private bool isTweening = false;
    private Animator adImageAnimator;
    private Animator animator;
    private Vector3 initialScale;
    public UnityEvent boosterEvent;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Activate);

        // Get the Animator components
        animator = GetComponent<Animator>();
        adImageAnimator = adImageGameObject.GetComponent<Animator>();

        // Store the initial scale
        initialScale = transform.localScale;
    }

    public void Activate()
    {
        if (isTweening)
        {
            return; // If a tween is already playing, do nothing
        }

        isTweening = true;

        // Disable the Animator components
        DisableAnimatorComponents();

        // Set the object's scale to its initial scale
        ResetObjectScale();

        // Scale down the ad image simultaneously with the other sequence
        ScaleDownAdImage();

        // Start the tweens for the booster indicators
        StartBoosterTweens();

        // After scaling down, invoke the booster event
        InvokeBoosterEvent();
    }

    private void DisableAnimatorComponents()
    {
        if (animator != null)
        {
            animator.enabled = false;
        }

        if (adImageAnimator != null)
        {
            adImageAnimator.enabled = false;
        }
    }

    private void ResetObjectScale()
    {
        transform.localScale = initialScale;
    }

    private void ScaleDownAdImage()
    {
        if (adImageGameObject != null)
        {
            adImageGameObject.transform.DOScale(Vector3.zero, adImageScaleDuration)
                .SetEase(Ease.InOutQuad);
        }
    }

    private void StartBoosterTweens()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(boosterIndicatorBlack.DOFillAmount(0f, boosterTime).From(1f).SetEase(Ease.Linear));
        sequence.Join(boosterIndicatorGreen.transform.DOScale(1.2f, 0.4f).From(1f).SetEase(Ease.OutBack, 4.5f));
        sequence.Join(boosterIndicatorGreen.DOFillAmount(0f, boosterTime).From(1f).SetEase(Ease.Linear));
        sequence.OnComplete(() =>
        {
            // Tweening is complete, smoothly scale up the ad image and re-enable its Animator component
            if (adImageGameObject != null)
            {
                adImageGameObject.transform.DOScale(Vector3.one, adImageScaleDuration)
                    .SetEase(Ease.InOutQuad)
                    .OnComplete(() =>
                    {
                        EnableAnimatorComponents();

                        // Reset the flag
                        isTweening = false;
                    });
            }
        });
    }

    private void EnableAnimatorComponents()
    {
        if (adImageAnimator != null)
        {
            adImageAnimator.enabled = true;
        }

        if (animator != null)
        {
            animator.enabled = true;
        }
    }

    private void InvokeBoosterEvent()
    {
        boosterEvent?.Invoke();
    }
}
