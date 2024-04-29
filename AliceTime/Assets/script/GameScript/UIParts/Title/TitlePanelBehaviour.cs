using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class TitlePanelBehaviour : UIMonobehaviour
{
    public bool isAnimation;
    public bool animEnd;
    public float DestroyTime = 1f;

    private float startAppearanceTime;
    private float startDestroyTime;

    private readonly float width = 17.77778f;
    float timer = 0f;

    [SerializeField]
    protected TitleButtonBehaviour backButton = null;

    public List<TitleButtonBehaviour> activeButtonArray = new List<TitleButtonBehaviour>();

    protected const float MARGINTIME = 0.15f;
    protected const float BORDER = 0.75f;
    public int selectIndex = 0;

    protected override void Init()
    {
    }

    protected override void Deinit()
    {
        StopAllCoroutines();
    }

    public void Enable()
    {
        gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        if (gameObject.activeSelf == true)
        {
            StartCoroutine(DestroyAnimation());
        }

        for (int i = activeButtonArray.Count - 1; i >= 0; i--)
        {
            selectIndex = 0;
            activeButtonArray[i].isSelectedActive = false;
            activeButtonArray[i].SelectedAnimation(0f);
        }
    }

    protected virtual void InitSelect() { }

    IEnumerator DestroyAnimation()
    {
        TitleUIManager.IsContorollable = false;

        isAnimation = true;
        UIManager.DisableInput();

        startDestroyTime = Time.time;
        while (Time.time - startDestroyTime < DestroyTime)
        {
            var diff = Time.time - startDestroyTime;
            transform.position = Vector3.left * width * (diff / DestroyTime);
            yield return null;
        }

        transform.position = Vector3.left * width;
        gameObject.SetActive(false);
        isAnimation = false;
    }

    public virtual IButtonEvent CheckTouchedButton(Vector3 screenPosition)
    {
        return null;
    }
}