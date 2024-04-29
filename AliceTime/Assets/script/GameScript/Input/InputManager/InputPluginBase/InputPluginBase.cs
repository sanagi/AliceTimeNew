using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InputSupport;

public abstract class InputPluginBase<IEventHandler, EventHandlerManager, InputParam> : SingletonMonoBehaviour<InputPluginBase<IEventHandler, EventHandlerManager, InputParam>>
    where IEventHandler : IInputEventHandler
    where EventHandlerManager : InputEventHandlerManager<IEventHandler, InputParam>
    where InputParam : InputParamBase<InputParam>
{
    protected EventHandlerManager eventHandlerManager;
    public EventHandlerManager HandlerManager
    {
        get
        {
            return eventHandlerManager;
        }
    }
    protected InputInfoManager<InputParam> infoManager;
    public InputInfoManager<InputParam> InfoManager
    {
        get
        {
            return infoManager;
        }
    }

    protected override void Init()
    {
        base.Init();
        infoManager = new InputInfoManager<InputParam>();
        eventHandlerManager = gameObject.AddComponent<EventHandlerManager>();
    }

    void Update()
    {
        if (infoManager != null && infoManager.InfoCount > 0)
        {
            eventHandlerManager.FireEvent(infoManager.InputInfo);
        }
    }

    public void RegisterEventHandler(IEventHandler handler)
    {
        eventHandlerManager.RegisterEventHandler(handler);
    }

    public void UnregisterEventHandler(IEventHandler handler)
    {
        eventHandlerManager.UnregisterEventHandler(handler);
    }

    public List<IEventHandler> CurrentRegistedEventHandlers
    {
        get
        {
            return eventHandlerManager.CurrentRegistedHandlers;
        }
    }

    public void ClearInput()
    {
        InfoManager.Clear();
    }
}
