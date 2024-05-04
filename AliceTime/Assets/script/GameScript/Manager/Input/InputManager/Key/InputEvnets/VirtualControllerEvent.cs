using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using InputSupport;

namespace KamioriInput
{
	public class VirtualControllerEvent : MonoBehaviour, ITouchEventHandler
	{
		public bool isStick = false;

		private bool isTouch;
		private EventSystem currentEventSystem;
		private PointerEventData pointer;
		private List<RaycastResult> result;

		private static GameObject virtualControllerCanvas;
		private Stick stick;
		public List<Image> stickImg = new List<Image>();

		private Jump jump;
		private Image jumpImage;

		RaycastHit[] hits;
		private bool isOverlapStick;
		private bool isOverlapJump;

		//private OptionManager.ControllerType controllerType;

        public static GameObject GetVirtual()
        {
            return virtualControllerCanvas;
        }

		void Awake() {
			result = new List<RaycastResult>();
			currentEventSystem = EventSystem.current;
			pointer = new PointerEventData(currentEventSystem);

			virtualControllerCanvas = Instantiate((GameObject)Resources.Load<GameObject>("VirtualControllerCanvas"));

			var infoManager = new InputInfoManager<KeyInfo>();

			stick = virtualControllerCanvas.transform.Find("controller/Stick/button").GetComponent<Stick>();
			stick.InfoManager = infoManager;
			jump = virtualControllerCanvas.transform.Find("controller/Jump/button").GetComponent<Jump>();
			jump.InfoManager = infoManager;

			stickImg.Add(virtualControllerCanvas.transform.Find("controller/Stick").GetComponent<Image>());
			stickImg.Add(virtualControllerCanvas.transform.Find("controller/Stick/button").GetComponent<Image>());
			stickImg.Add(virtualControllerCanvas.transform.Find("controller/Stick/button/Right").GetComponent<Image>());
			stickImg.Add(virtualControllerCanvas.transform.Find("controller/Stick/button/Up").GetComponent<Image>());
			stickImg.Add(virtualControllerCanvas.transform.Find("controller/Stick/button/Down").GetComponent<Image>());

			jumpImage = jump.GetComponent<Image>();
		}

		void Start() {
			DontDestroyOnLoad(virtualControllerCanvas);
			AliceInputManager.RegisterTouchEventHandler(this);

        }

		void Update() {
			// オプションによって表示するコントローラを判定があれば
			CheckOverlap();
			//SetControllerType();
		}
		
		/*public void SetControllerType() {
			var controllerType = OptionManager.Instance.GetControllerType();
			if (controllerType == OptionManager.ControllerType.Single) {
				jump.gameObject.SetActive(false);
			} else if (controllerType == OptionManager.ControllerType.Dual) {
				jump.gameObject.SetActive(true);
			}
		}*/

		void CheckOverlap() {
			isOverlapStick = false;
			isOverlapJump = false;

			var rectStick = stick.GetComponent<RectTransform>();
            var originStick = Camera.main.ScreenToWorldPoint(rectStick.position);
            var stickEdge = Camera.main.ScreenToWorldPoint(rectStick.position + Vector3.up * rectStick.rect.height * 0.15f + Vector3.right * rectStick.rect.width * 0.15f);
            var stickHalfExtents = stickEdge - originStick;
			var rectJump = jump.GetComponent<RectTransform>();
            var originJump = Camera.main.ScreenToWorldPoint(jump.transform.position);
            var jumpEdge = Camera.main.ScreenToWorldPoint(rectJump.position + Vector3.up * rectJump.rect.height * 0.15f + Vector3.right * rectJump.rect.width * 0.15f);
            var jumpHalfExtents = jumpEdge - originJump;

			hits = Physics.BoxCastAll(originStick, stickHalfExtents, Vector3.forward);
			foreach (var hit in hits) {
				if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Click") && hit.transform.tag == "Untagged") {
					isOverlapStick = true;
				}
			}
				
			hits = Physics.BoxCastAll(originJump, jumpHalfExtents, Vector3.forward);
			foreach (var hit in hits) {
				if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Click") && hit.transform.tag == "Untagged") {
					isOverlapJump = true;
				}
			}

			if (isOverlapStick) {
				for(int i=0;i<stickImg.Count;i++){
					stickImg[i].color = new Color(1, 1, 1, 0.25f);
				}
			} else {
				for(int i=0;i<stickImg.Count;i++){
					stickImg[i].color = new Color(1, 1, 1, 1.0f);
					if(MainSceneManager.init && MainSceneManager.CurrentPhase != null){
						if(MainSceneManager.CurrentPhase.GetType() == typeof(Game) && GameSceneManager.initialized){
							if(GameSceneManager.CurrentPhaseState == GAMESCENE.MAIN){
								
							}
						}
					}
				}
			}

			if (isOverlapJump) {
				jumpImage.color = new Color(1, 1, 1, 0.25f);
			} else {
				jumpImage.color = new Color(1, 1, 1, 1.0f);;
			}

		}

		void OnDestroy() {
			AliceInputManager.UnregisterTouchEventHandler(this);
		}

		public static void EnableVirtualController() {
			if (virtualControllerCanvas != null) {
				virtualControllerCanvas.SetActive(true);
				
			} else {
				Debug.LogError("VirtualControllerCanvas is null");
			}
		}

		public static void DisableVirtualController() {
			if (virtualControllerCanvas != null) {
				//なくすのではなくコンポーネントをオフにして半透明にする
				virtualControllerCanvas.SetActive(false);
			} else {
				Debug.LogError("VirtualControllerCanvas is null");
			}
		}

		#region ITouchEventHandler implementation

		public bool OnTouchEventBegan(TouchInfo[] touchInfo) {
			if (!virtualControllerCanvas.activeSelf)
				return false;

			var isAction = false;

			if (currentEventSystem == null) {
				currentEventSystem = EventSystem.current;
				pointer = new PointerEventData(currentEventSystem);
			}

			foreach (var info in touchInfo) {
				pointer.position = info.currentScreenPosition;
				currentEventSystem.RaycastAll(pointer, result);

				if (result.Count == 0)
					continue;

				foreach (var res in result) {
					IVirtualControllerEvent component = res.gameObject.GetComponent<IVirtualControllerEvent>();
					if (component != null) {
						var objName = res.gameObject.transform.parent.name;
						if ((objName == "Stick" && isOverlapStick) || (objName == "Jump" && isOverlapJump)) {
							continue;
						}
						component.OnFireEvent(info);
						isAction = true;
					}
				}
			}

			return isAction;
		}

		public bool OnTouchEventEnded(TouchInfo[] touchInfo) {
			if (!virtualControllerCanvas.activeSelf)
				return false;

			var isAction = false;

			foreach (var info in touchInfo) {
				if (stick.ControlledTouchID == info.Id) {
					info.currentScreenPosition = -1f * (Vector3.up + Vector3.right);
					stick.OnFireEvent(info);
					isAction = true;
				}
				if (jump.ControlledTouchID == info.Id) {
					jump.OnFireEvent(info);
					isAction = true;
				}
			}

			return isAction;
		}

		public bool OnTouchEventMoved(TouchInfo[] touchInfo) {
			if (!virtualControllerCanvas.activeSelf)
				return false;

			var isAction = false;

			foreach (var info in touchInfo) {
				if (stick.ControlledTouchID == info.Id) {
					stick.OnFireEvent(info);
				}
				if (jump.ControlledTouchID == info.Id) {
					jump.OnFireEvent(info);
				}
			}

			return isAction;
		}

		#endregion

		#region IInputEventHandler implementation

		public int Order
		{
			get
			{
				return 100;
			}
		}

		public bool Process
		{
			get
			{
				return virtualControllerCanvas != null ? virtualControllerCanvas.activeSelf : false;
			}
		}

		#endregion
	}
}