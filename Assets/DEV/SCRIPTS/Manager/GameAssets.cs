using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;

    public static GameAssets i
    {
        get
        {
            if (_i == null) _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            return _i;
        }
    }

    public GameObject characterPlacedParticle;

    public GameObject failedMark;

    public void CreateFailedMark(Vector3 pos)
    {
        GameObject f = Instantiate(failedMark, pos, failedMark.transform.rotation);
        Sequence fSeq = DOTween.Sequence();
        fSeq.Append(f.transform.DOScale(0f, 0.3f).From().SetEase(Ease.OutBack));
        fSeq.Append(f.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack).SetDelay(0.3f));
        fSeq.AppendCallback(() => Destroy(f.gameObject));
    }

    public GameObject correctMark;

    public void CreateCorrectMark(Vector3 pos)
    {
        GameObject f = Instantiate(correctMark, pos, correctMark.transform.rotation);
        Sequence fSeq = DOTween.Sequence();
        fSeq.Append(f.transform.DOScale(0f, 0.3f).From().SetEase(Ease.OutBack));
        fSeq.Append(f.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack).SetDelay(0.3f));
        fSeq.AppendCallback(() => Destroy(f.gameObject));
    }

    public List<string> cardNames;

    public bool IsCardName(string name)
    {
        foreach (var item in cardNames)
        {
            if (name.Contains(item))
            {
                return true;
            }
        }
        return false;
    }

}
