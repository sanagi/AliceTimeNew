using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleBlackPanel : TitlePanelBehaviour
{
    protected override void Init()
    {
        base.Init();

        gameObject.transform.position = new Vector3(0f, 0f, 0f);

        gameObject.SetActive(false);
    }

    void Update()
    {
        //ちょっとどこで手前に出されているか分からない(ロゴ系の操作？)
        if (gameObject.transform.position.z != 0f)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0f);
        }
    }
}
