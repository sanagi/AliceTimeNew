using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionButtonBehaviour : MonoBehaviour, IButtonEvent
{
    // Use this for initialization

    public Image buttonImage;

    public OptionManager.MoveSpeed moveSpeed;

    void Start()
    {
        SwitchColor();
    }

    public virtual void FireButtonEvent()
    {
        if (buttonImage == null)
        {
            buttonImage = gameObject.GetComponent<Image>();
        }
        buttonImage.color = new Color(0.55f, 0.55f, 0.55f, 1.0f);
    }
    public virtual void ReleaseButtonEvent()
    {
    }

    public virtual void ReleaseOutButtonEvent()
    {
        SwitchColor();
    }

    public virtual void SwitchColor()
    {
        if (buttonImage == null)
        {
            buttonImage = gameObject.GetComponent<Image>();
        }

        if (OptionManager.Instance.GetMoveSpeed() == moveSpeed)
        {
            buttonImage.color = new Color(0.55f, 0.55f, 0.55f, 1.0f);
        }
        else
        {
            buttonImage.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
