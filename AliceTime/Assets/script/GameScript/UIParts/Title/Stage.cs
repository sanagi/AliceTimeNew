using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    private Image result;

    private void Awake()
    {
        if (result == null)
        {
            Init();            
        }
    }

    private void Init()
    {
        var children = transform.GetComponentsInChildren<Image>();
        for (var i = 0; i < children.Length; i++)
        {
            if (children[i].gameObject.name == "result")
            {
                result = children[i].GetComponent<Image>();
                result.gameObject.SetActive(false);
            }
        }
    }

    public void ShowClear()
    {
        if (result == null)
        {
            Init();
        }
        result.gameObject.SetActive(true);
    }
}
