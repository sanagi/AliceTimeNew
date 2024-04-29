using UnityEngine;

public class SingletonSubMonobehaviour : MonoBehaviour
{
    protected virtual void Init() { }
    protected virtual void Deinit() { }

    private bool isInit;
    private bool isDeinit;

    public void InitIfNeeded()
    {
        if (isInit)
        {
            return;
        }
        isInit = true;
        Init();
    }

    public void DeinitIfNeeded()
    {
        if (isDeinit)
        {
            return;
        }
        isDeinit = true;
        Deinit();
    }
}