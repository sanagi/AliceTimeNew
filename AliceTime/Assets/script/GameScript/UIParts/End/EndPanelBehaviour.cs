using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class EndPanelBehaviour : MonoBehaviour
{
    public abstract void DoStart();
    public abstract void DoDestroy();
    public abstract ENDSCENE TargetScene();

    public virtual void Start()
    {
        EndUIManager.RegistedPanel(this);
        gameObject.SetActive(false);
        DoStart();
    }

    public virtual void OnDestroy()
    {
        EndUIManager.UnregistedPanel(TargetScene(), this);
        DoDestroy();
    }

    public void Display()
    {
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }


    public virtual IButtonEvent CheckTouchedButton(Vector3 screenPosition)
    {
        return null;
    }
}