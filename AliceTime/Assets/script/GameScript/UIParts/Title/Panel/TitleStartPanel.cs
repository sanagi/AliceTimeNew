using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleStartPanel : TitlePanelBehaviour
{
    protected override void Init()
    {
        base.Init();

        gameObject.transform.position = new Vector3(0f, 0f, 0f);
        gameObject.SetActive(false);
    }
}
