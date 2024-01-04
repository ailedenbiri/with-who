using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuBehaviour : MonoBehaviour
{
    public RectTransform levelsParent;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        Time.timeScale = 1f;
        yield return new WaitForEndOfFrame();
        GameObject.Find("GameLogo").transform.DOScale(0f, 0.5f).SetDelay(0.2f).SetEase(Ease.OutBack).From();
        int level = PlayerPrefs.GetInt("SelectedLevel", 0);
        levelsParent.DOAnchorPosX(-412.5f * level, 0f);
        levelsParent.transform.GetChild(level + 1).GetComponent<Image>().color = Color.yellow;
        foreach (Transform item in levelsParent.transform)
        {
            if (item.GetComponentInChildren<TextMeshProUGUI>() != null)
            {
                item.GetComponentInChildren<TextMeshProUGUI>().text = item.GetSiblingIndex().ToString();
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene($"New Level - {PlayerPrefs.GetInt("SelectedLevel", 0) + 1}");
    }
}
