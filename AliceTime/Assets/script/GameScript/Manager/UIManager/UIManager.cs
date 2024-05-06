using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// タッチ操作まで巻き取るかどうか(一旦気にしなくていいかも)
/// </summary>
public abstract class UIManager : MonoBehaviour
{
	protected Canvas _myCanvas;
	
	public virtual int MyOrder (){
		return 0;
	}

	private static bool isInput = false;

	public static bool IsInput() { return isInput; }
	public static void EnableInput() { isInput = true; }
	public static void DisableInput() { isInput = false; }

    private float border = 0.25f;

    public void Initialization(Canvas canvas) 
	{
		_myCanvas = canvas;
		_myCanvas.renderMode = RenderMode.ScreenSpaceCamera;
		_myCanvas.worldCamera = CameraManager.Instance.GetUiCamera();
	}

    public void Finalization()
	{
		
	}
    
    protected void Set3DScale(RawImage image3D)
    {
	    //16:9のスケール比にした上でwidthが画面いっぱいになるようにセット
	    var imageRect = image3D.rectTransform;
	    var localScale = imageRect.localScale;

	    float aspect = (float)Screen.width / (float)Screen.height;
	    localScale.y = aspect;
	    image3D.rectTransform.localScale = localScale;
    }

    public int Order {
		get {
			return MyOrder();
		}
	}

	public bool Process {
		get {
			return isInput;
		}
	}
}
