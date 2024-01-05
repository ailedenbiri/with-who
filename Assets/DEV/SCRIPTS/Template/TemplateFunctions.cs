using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using System.Threading.Tasks;
using System.Linq;

namespace FlushTemplate
{
    public class GameObjectPool<T> where T : Object
    {
        private Queue<T> _pool;
        private T _prefab;

        public GameObjectPool(T prefab, int initialCapacity = 10)
        {
            _prefab = prefab;
            _pool = new Queue<T>(initialCapacity);
        }

        public T Get()
        {
            if (_pool.Count == 0)
            {
                return GameObject.Instantiate(_prefab);
            }

            return _pool.Dequeue();
        }

        public void Return(T gameObject)
        {
            _pool.Enqueue(gameObject);
        }
    }
    public class TemplateFunctions : MonoBehaviour
    {
        [SerializeField] private TemplateSettings settings;
        public static TemplateFunctions Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        GameObjectPool<GameObject> coinPool;
        private void Start()
        {
            coinPool = new GameObjectPool<GameObject>(coinImagePrefab);
        }

        [Header("Coin ui")]
        public Transform coinUiParent;
        public GameObject coinImagePrefab;
        public void CoinBoink(float parentBoinkAmount, float coinBoinkAmount)
        {

            if (coinUiParent == null || coinUiParent.childCount == 0)
            {
                Debug.LogError("coinUiParent is not assigned or does not have a child.");
                return;
            }

            Transform firstChild = coinUiParent.GetChild(0);

            coinUiParent.DOKill();
            Sequence boinkSequence = DOTween.Sequence().SetUpdate(true);

            boinkSequence
                .Append(coinUiParent.DOScale(Vector3.one * parentBoinkAmount, settings.defaultBoinkDuration))
                .Join(firstChild.DOScale(Vector3.one * coinBoinkAmount, settings.defaultBoinkDuration))
                .Append(coinUiParent.DOScale(Vector3.one, settings.defaultBoinkDuration))
                .Join(firstChild.DOScale(Vector3.one, settings.defaultBoinkDuration));

            boinkSequence.SetId(coinUiParent);
        }
        public async void CreateCanvasMoney(int amount, RectTransform rectTransform = null)
        {
            Vector3 coinPos = coinUiParent.transform.GetChild(0).position;
            for (int i = 0; i < amount; i++)
            {
                await Task.Delay(settings.canvasCoinDelay);
                Vector3 pos = rectTransform.position + Random.Range(-settings.canvasCoinRange, settings.canvasCoinRange) * Vector3.up
                    + Random.Range(-settings.canvasCoinRange, settings.canvasCoinRange) * Vector3.right;
                GameObject coin = coinPool.Get();
                coin.transform.SetParent(coinUiParent.parent, false);
                coin.transform.position = rectTransform.position;
                coin.SetActive(true);
                coin.transform.DOMove(pos, settings.canvasCoinInitialMoveTime).OnComplete(() =>
                {
                    coin.transform.DOMove(coinPos, settings.canvasCoinMoveTime).SetEase(settings.canvasCoinCurve).OnComplete(() =>
                    {
                        CoinBoink(1.13f, 1.2f);
                        coin.SetActive(false);
                        DOVirtual.DelayedCall(0.2f, () => coinPool.Return(coin));

                    });
                });
            }
        }

        [Header("Upgrade panel")]
        public Transform upgradePanelUi;
        public RectTransform upgradeButton;

        public void OpenUpgradePanel(float time)
        {
            switch (settings.panelAnimation)
            {
                case TemplateSettings.PanelAnimation.Scale:
                    OpenUpgradePanelBoink(time);
                    break;
                case TemplateSettings.PanelAnimation.Slide:
                    OpenUpgradePanelSlide(time);
                    break;
                default:
                    break;
            }
        }

        public void CloseUpgradePanel(float time)
        {
            switch (settings.panelAnimation)
            {
                case TemplateSettings.PanelAnimation.Scale:
                    CloseUpgradePanelBoink(time);
                    break;
                case TemplateSettings.PanelAnimation.Slide:
                    CloseUpgradePanelSlide(time);
                    break;
                default:
                    break;
            }
        }

        #region Upgrade Panel Boink Animation
        private void OpenUpgradePanelBoink(float time)
        {
            if (upgradePanelUi == null || upgradePanelUi.childCount < 2)
            {
                Debug.LogError("upgradePanelUi is not assigned or does not have enough children.");
                return;
            }

            Transform panelHolder = upgradePanelUi.GetChild(1);
            Image upgradeBackground = upgradePanelUi.GetChild(0).GetComponent<Image>();

            panelHolder.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            panelHolder.transform.localScale = Vector3.zero;

            upgradePanelUi.DOKill(true);
            upgradePanelUi.gameObject.SetActive(true);

            panelHolder.GetComponent<ScrollRect>().enabled = false;

            upgradeBackground.DOFade(settings.defaultFadeAlpha, time).SetUpdate(true);
            Sequence panelSequence = DOTween.Sequence().SetUpdate(true);
            panelSequence
                .Append(panelHolder.DOScale(1.1f, time / 3).SetUpdate(true))
                .Append(panelHolder.DOScale(0.9f, time / 3).SetUpdate(true))
                .Append(panelHolder.DOScale(1f, time / 3).SetUpdate(true))
                .OnComplete(() => panelHolder.GetComponent<ScrollRect>().enabled = true);

            panelSequence.SetId(upgradePanelUi);
        }
        private void CloseUpgradePanelBoink(float time)
        {
            if (upgradePanelUi == null || upgradePanelUi.childCount < 2)
            {
                Debug.LogError("upgradePanelUi is not assigned or does not have enough children.");
                return;
            }

            Transform panelHolder = upgradePanelUi.GetChild(1);
            Image upgradeBackground = upgradePanelUi.GetChild(0).GetComponent<Image>();

            upgradePanelUi.DOKill(true);

            panelHolder.GetComponent<ScrollRect>().enabled = false;

            upgradeBackground.DOFade(0, time).SetUpdate(true);
            Sequence panelSequence = DOTween.Sequence().SetUpdate(true);
            panelSequence
                .Append(panelHolder.DOScale(0.9f, time / 3).SetUpdate(true))
                .Append(panelHolder.DOScale(1.1f, time / 3).SetUpdate(true))
                .Append(panelHolder.DOScale(0f, time / 3).SetUpdate(true))
                .OnComplete(() => upgradePanelUi.gameObject.SetActive(false));

            panelSequence.SetId(upgradePanelUi);
        }

        #endregion

        #region Upgrade Panel Slide Animation

        private void OpenUpgradePanelSlide(float time)
        {
            upgradePanelUi.DOKill(true);
            upgradePanelUi.gameObject.SetActive(true);
            Sequence panelSequence = DOTween.Sequence().SetUpdate(true);
            Transform panelHolder = upgradePanelUi.GetChild(1);
            panelHolder.localScale = Vector3.one;
            Image upgradeBackground = upgradePanelUi.GetChild(0).GetComponent<Image>();
            upgradePanelUi.GetChild(1).GetChild(0).GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            upgradeBackground.DOFade(settings.defaultFadeAlpha, time).SetUpdate(true);
            panelHolder.GetComponent<RectTransform>().anchoredPosition = (Screen.width + 500f) * Vector2.right;
            panelSequence
                .Append(upgradeButton.DOAnchorPosX(-upgradeButton.anchoredPosition.x, (time / 3)).SetEase(Ease.InBack).SetUpdate(true))
                .Append(panelHolder.GetComponent<RectTransform>().DOAnchorPosX(0f, 2f * (time / 3)).SetEase(Ease.OutBack).SetUpdate(true)
                .OnComplete(() => panelHolder.GetComponent<ScrollRect>().enabled = true))
                .SetId(upgradePanelUi);
        }
        private void CloseUpgradePanelSlide(float time)
        {
            upgradePanelUi.DOKill(true);
            Sequence panelSequence = DOTween.Sequence();
            panelSequence.SetUpdate(true);
            Transform panelHolder = upgradePanelUi.GetChild(1);
            Image upgradeBackground = upgradePanelUi.GetChild(0).GetComponent<Image>();
            upgradeBackground.DOFade(0f, time).SetUpdate(true);
            panelSequence
                .Append(panelHolder.GetComponent<RectTransform>().DOAnchorPosX(Screen.width + 500f, 2f * (time / 3)).SetEase(Ease.InBack).SetUpdate(true))
                .Append(upgradeButton.DOAnchorPosX(-upgradeButton.anchoredPosition.x, (time / 3)).SetEase(Ease.OutBack).SetUpdate(true)
                .OnComplete(() => upgradePanelUi.gameObject.SetActive(false)))
                .SetId(upgradePanelUi);
        }
        #endregion

        #region Settings Panel Animation
        [Header("Settings Panel")]
        public Transform settingsPanelParent;
        public RectTransform settingsButton;

        public void OpenSettingsPanel(float time)
        {
            if (GameManager.i)
            {
                GameManager.i.isGamePlaying = false;
            }
            settingsPanelParent.DOKill(true);
            settingsPanelParent.gameObject.SetActive(true);
            Sequence panelSequence = DOTween.Sequence().SetUpdate(true);
            Transform panelHolder = settingsPanelParent.GetChild(1);
            //panelHolder.localScale = Vector3.one * 0.85f;
            Image settingsBackground = settingsPanelParent.GetChild(0).GetComponent<Image>();
            settingsBackground.DOFade(settings.defaultFadeAlpha, time).SetUpdate(true);
            panelHolder.GetComponent<RectTransform>().anchoredPosition = (-Screen.width - 500f) * Vector2.right;
            panelSequence
                .Append(settingsButton.DOAnchorPosX(-settingsButton.anchoredPosition.x, (time / 3)).SetEase(Ease.InBack).SetUpdate(true))
                .Append(panelHolder.GetComponent<RectTransform>().DOAnchorPosX(0f, 2f * (time / 3)).SetEase(Ease.OutBack).SetUpdate(true))
                .SetId(settingsPanelParent);
        }
        public void CloseSettingsPanel(float time)
        {
            settingsPanelParent.DOKill(true);
            Sequence panelSequence = DOTween.Sequence();
            panelSequence.SetUpdate(true);
            Transform panelHolder = settingsPanelParent.GetChild(1);
            Image settingsBackground = settingsPanelParent.GetChild(0).GetComponent<Image>();
            settingsBackground.DOFade(0f, time).SetUpdate(true);
            panelSequence
                .Append(panelHolder.GetComponent<RectTransform>().DOAnchorPosX(-Screen.width - 500f, 2f * (time / 3)).SetEase(Ease.InBack).SetUpdate(true))
                .Append(settingsButton.DOAnchorPosX(-settingsButton.anchoredPosition.x, (time / 3)).SetEase(Ease.OutBack).SetUpdate(true)
                .OnComplete(() =>
                {
                    settingsPanelParent.gameObject.SetActive(false);
                    if (GameManager.i)
                    {
                        GameManager.i.isGamePlaying = true;
                    }
                }))
                .SetId(settingsPanelParent);
        }
        #endregion
    }

    public static class FlushTF
    {

        public static void CoinBoink(float parentBoinkAmount = 1.2f, float coinBoinkAmount = 1.5f)
        {
            TemplateFunctions.Instance.CoinBoink(parentBoinkAmount, coinBoinkAmount);
        }

        public static void CreateCanvasMoney(RectTransform rect, int coinAmount = 5)
        {
            TemplateFunctions.Instance.CreateCanvasMoney(coinAmount, rect);
        }

        public static void OpenUpgradePanel(float time = 0.9f)
        {
            TemplateFunctions.Instance.OpenUpgradePanel(time);
        }

        public static void CloseUpgradePanel(float time = 0.9f)
        {
            TemplateFunctions.Instance.CloseUpgradePanel(time);
        }

        public static void OpenSettingsPanel(float time = 0.9f)
        {
            TemplateFunctions.Instance.OpenSettingsPanel(time);
        }

        public static void CloseSettingsPanel(float time = 0.9f)
        {
            TemplateFunctions.Instance.CloseSettingsPanel(time);
        }
    }
}

