using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using KamioriInput;
using Rewired;

public class WorldBackgroundController : MonoBehaviour, ITouchEventHandler {
	private EventSystem currentEventSystem;
	private PointerEventData pointer;
	private List<RaycastResult> result;

	private GameObject background;
	private RectTransform rectBackground;
	private static Vector2 screenUpRightPos;

	private Transform m_Transform;
	private RectTransform rectTransform;

	private static float orthographicSize;
	private static float defOrthographicSize;

	private static bool isInput = false;

	[SerializeField] private bool isPinch;
	public bool IsPinch() { return isPinch; }

	private float currentDistance;

	static private List<TouchInfo> touchList;

	public static bool IsInput() { return isInput; }
	public static void EnableInput() { isInput = true; }
	public static void DisableInput() { isInput = false; }

	void Start()
	{
		touchList = new List<TouchInfo> (); 

		m_Transform = transform;
		rectTransform = GetComponent<RectTransform> ();

		background = transform.Find ("BackGround").gameObject;
		rectBackground = background.GetComponent<RectTransform> ();
		screenUpRightPos = Camera.main.ViewportToScreenPoint (Vector3.one);

		currentEventSystem = EventSystem.current;
		pointer = new PointerEventData (currentEventSystem);
		result = new List<RaycastResult> ();

		defOrthographicSize = Camera.main.orthographicSize;
		orthographicSize = Camera.main.orthographicSize;

		AliceInputManager.RegisterTouchEventHandler (this);
	}

	void OnDestroy()
	{
		AliceInputManager.UnregisterTouchEventHandler (this);
	}

	public static void ZoomCamera(float zoomValue) 
	{
		orthographicSize += zoomValue;
		if (orthographicSize > defOrthographicSize) {
			orthographicSize = defOrthographicSize;
		} else if (orthographicSize < defOrthographicSize - 50f) {
			orthographicSize = defOrthographicSize - 50f;
		}
		Camera.main.orthographicSize = orthographicSize;
	}

	#region ITouchEventHandler implementation

	public bool OnTouchEventBegan (TouchInfo[] touchInfo)
	{
		if (touchList.Count > 2) return true;

		pointer.position = touchInfo [0].currentScreenPosition;
		currentEventSystem.RaycastAll (pointer, result);

		if (result.Count > 0) {
			for(int i=0; i<result.Count; i++) {
				if (result[i].gameObject == background) {
					if (!isPinch && touchList.Count == 2) {
						currentDistance = Vector3.Distance (touchList [0].currentScreenPosition, touchList [1].currentScreenPosition);
						isPinch = true;
					}
					touchList.Add (touchInfo [i]);
					return true;
				}
			}
		}


		return false;
	}

	public bool OnTouchEventEnded (TouchInfo[] touchInfo)
	{
		if (touchList.Count == 0) return false;
		touchList.Clear ();
		return true;
	}

	public bool OnTouchEventMoved (TouchInfo[] touchInfo)
	{
		if (touchList.Count == 0) return false;

		foreach (var touch in touchInfo) {
			if (touchList.Count > 2) {

			} else {
				if (touch.Id == touchList[0].Id) {
					m_Transform.position += touch.deltaDistance;

					var nowDiffX = Mathf.Abs (rectTransform.position.x) * 2f;
					var diffX = rectBackground.rect.width * rectBackground.localScale.x - screenUpRightPos.x * orthographicSize / defOrthographicSize;
					if (diffX < nowDiffX) {
						if (rectTransform.position.x > 0) {
							rectTransform.position = Vector3.right * diffX / 2f + Vector3.up * rectTransform.position.y;
						} else {
							rectTransform.position = Vector3.left * diffX / 2f + Vector3.up * rectTransform.position.y;
						}
					}

					var nowDiffY = Mathf.Abs (rectTransform.position.y) * 2f;
					var diffY = rectBackground.rect.height * rectBackground.localScale.y - screenUpRightPos.y * orthographicSize / defOrthographicSize;
					if (diffY < nowDiffY) {
						if (rectTransform.position.y > 0) {
							rectTransform.position = Vector3.up * diffY / 2f + Vector3.right * rectTransform.position.x;
						} else {
							rectTransform.position = Vector3.down * diffY / 2f + Vector3.right * rectTransform.position.x;
						}
					}
					break;
				}
			}
		}

		return true;
	}

	#endregion

	#region IInputEventHandler implementation

	public int Order {
		get {
			return 100;
		}
	}

	public bool Process {
		get {
			return isInput;
		}
	}

	#endregion
}
