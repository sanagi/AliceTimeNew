using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class EndTruePanel : EndPanelBehaviour
{
    private EventSystem currentEventSystem;
    private PointerEventData pointer;
    private List<RaycastResult> result;

    public override IButtonEvent CheckTouchedButton(Vector3 screenPosition)
    {
        pointer.position = screenPosition;
        currentEventSystem.RaycastAll(pointer, result);

        if (result.Count > 0)
        {
            if (result.Count > 1)
            {
                Debug.LogWarning("There is a possibility that the image is overlapped");
            }
            return result[0].gameObject.GetComponent<IButtonEvent>();
        }
        return null;
    }

    #region implemented abstract members of GalleryPanelBehaviour

    public override void DoStart()
    {
        currentEventSystem = EventSystem.current;
        pointer = new PointerEventData(currentEventSystem);
        result = new List<RaycastResult>();
    }

    public override void DoDestroy()
    {

    }

    public override ENDSCENE TargetScene()
    {
        return ENDSCENE.TRUE;
    }

    #endregion
}
