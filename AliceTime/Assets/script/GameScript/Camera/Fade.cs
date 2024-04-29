using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private Image fadeImage;
    [SerializeField]
    public CanvasGroup FadeCanvasGroup;
    public float Value
    {
        get
        {
            return FadeCanvasGroup.alpha;
        }
    }

    #endregion

    #region Methods
    
    public void FadeEnable(Color color)
    {
        fadeImage.color = color;
        fadeImage.gameObject.SetActive(true);
    }

    public void FadeDisable()
    {
        fadeImage.gameObject.SetActive(false);
    }

    #endregion
}