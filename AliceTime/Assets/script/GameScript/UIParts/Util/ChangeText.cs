using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChangeText : MonoBehaviour
{
    [SerializeField]
    private LabelDefine.TEXTLABEL label;

    void Start()
    {
        Text text = gameObject.GetComponent<Text>();
        text.text = LabelDefine.GetString(label);
    }
}
