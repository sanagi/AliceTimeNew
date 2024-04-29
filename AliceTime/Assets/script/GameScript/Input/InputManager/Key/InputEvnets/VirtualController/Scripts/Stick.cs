using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using InputSupport;

namespace KamioriInput
{
	public class Stick : MonoBehaviour, IVirtualControllerEvent 
	{
		const string IMAGE_UP_NORMAL = "Input/juuji_up";
		const string IMAGE_UP_PUSH = "Input/juuji_up_red";
		const string IMAGE_DOWN_NORMAL = "Input/juuji_down";
		const string IMAGE_DOWN_PUSH = "Input/juuji_down_red";
		const string IMAGE_LEFT_NORMAL = "Input/juuji_left";
		const string IMAGE_LEFT_PUSH = "Input/juuji_left_red";
		const string IMAGE_RIGHT_NORMAL = "Input/juuji_right";
		const string IMAGE_RIGHT_PUSH = "Input/juuji_right_red";

		private InputInfoManager<KeyInfo> infoManager;
		private KeyInfo crossKey;

		public float limitMoveDistance;

		private Transform m_Transform;
		private Vector3 defaultPosition;
        private Vector3 parentPosition;

		private Image leftImage;
		private Image rightImage;
		private Image upImage;
		private Image downImage;

		private Sprite sptCrossUp;
		private Sprite sptCrossDown;
		private Sprite sptCrossLeft;
		private Sprite sptCrossRight;
		private Sprite sptCrossPushedUp;
		private Sprite sptCrossPushedDown;
		private Sprite sptCrossPushedLeft;
		private Sprite sptCrossPushedRight;

		private float baseSize = 260f; // 設定で変更できるサイズのMax値
		private float buttonPosDiffY = 67.5f; // 設定で変更できるポジションのY差分（0.5で設定されている時、差分0）
		private float buttonPosDiffX = 120f; // 設定で変更できるポジションのX差分（0.5で設定されている時、差分0）

		void Awake()
		{
            infoManager = KeyInputManager.Instance.InfoManager;

			crossKey = new KeyInfo ();

			m_Transform = transform;
            parentPosition = m_Transform.parent.localPosition;

        }

		void Start()
		{
			sptCrossUp = Resources.LoadAsync<Sprite>(IMAGE_UP_NORMAL).asset as Sprite;
			sptCrossDown = Resources.LoadAsync<Sprite>(IMAGE_DOWN_NORMAL).asset as Sprite;
			sptCrossRight = Resources.LoadAsync<Sprite>(IMAGE_RIGHT_NORMAL).asset as Sprite;
			sptCrossLeft = Resources.LoadAsync<Sprite>(IMAGE_LEFT_NORMAL).asset as Sprite;
			sptCrossPushedUp = Resources.LoadAsync<Sprite>(IMAGE_UP_PUSH).asset as Sprite;
			sptCrossPushedDown = Resources.LoadAsync<Sprite>(IMAGE_DOWN_PUSH).asset as Sprite;
			sptCrossPushedRight = Resources.LoadAsync<Sprite>(IMAGE_RIGHT_PUSH).asset as Sprite;
			sptCrossPushedLeft = Resources.LoadAsync<Sprite>(IMAGE_LEFT_PUSH).asset as Sprite;

			leftImage = GetComponent<Image> ();
			leftImage.sprite = sptCrossLeft;
			rightImage = transform.Find("Right").GetComponent<Image>();
			rightImage.sprite = sptCrossRight;
			upImage = transform.Find("Up").GetComponent<Image>();
			upImage.sprite = sptCrossUp;
			downImage = transform.Find("Down").GetComponent<Image>();
			downImage.sprite = sptCrossDown;
		}

		void Update()
		{
			gameObject.transform.localPosition = Vector3.zero;
            if (infoManager == null) {
                infoManager = KeyInputManager.Instance.InfoManager;
            }

            if (crossKey.Phase == InputPhase.Ended) {
                crossKey.Id = int.MaxValue;
                crossKey.Phase = InputPhase.Missing;
                infoManager.UpdateParam(crossKey, null);
                return;
            }
        }

        void OnEnable() {
            if(MainSceneManager.CurrentPhase.PhaseName == "Game")
            {
                m_Transform.parent.localPosition = parentPosition;
            }
            infoManager.Clear();
            SetScaleSize();
		}

        public void SetScaleSize()
        {
            // 設定からボタンのサイズを変更する
            var controllerSizeRate = 0.55f + ((OptionManager.Instance.GetControllerSizeRate() - 0.5f) * 0.15f);
            var rect = GetComponent<RectTransform>();
            rect.sizeDelta = Vector2.one * baseSize * controllerSizeRate;
            var parentRect = m_Transform.parent.GetComponent<RectTransform>();
            parentRect.sizeDelta = Vector2.one * baseSize * controllerSizeRate;

            // 設定からボタンの位置を変更する
            var controllerPositionRate = (OptionManager.Instance.GetControllerPositionRate() - 0.75f) * 0.5f;
            var buttonPosDiff = new Vector3(buttonPosDiffX, buttonPosDiffY) * controllerPositionRate;
            if (MainSceneManager.CurrentPhase.PhaseName != "Game")
            {
                m_Transform.parent.localPosition = parentPosition + buttonPosDiff;
            }
            else
            {
                m_Transform.parent.localPosition = m_Transform.parent.localPosition + buttonPosDiff;
                defaultPosition = m_Transform.parent.position;
            }
            
        }

		#region IVirtualControllerEvent implementation

		public void OnFireEvent (KamioriInput.TouchInfo info)
		{
			// Ended処理
			if (info.Phase == InputPhase.Ended || (info.currentScreenPosition.x == -1 && info.currentScreenPosition.y == -1)) {
				/*leftImage.sprite = sptCrossLeft;
				rightImage.sprite = sptCrossRight;
				upImage.sprite = sptCrossUp;
				downImage.sprite = sptCrossDown;
				*/
				crossKey.Right = 0;
                crossKey.Left = 0;
                crossKey.Up = 0;
                crossKey.Down = 0;
                crossKey.Phase = InputPhase.Ended;
				infoManager.UpdateParam (crossKey, null);
				return;
			}

            var diff = info.currentScreenPosition - defaultPosition;
			if (diff.magnitude > limitMoveDistance) {
				diff = diff.normalized * limitMoveDistance;
			}

			// キャラクターの移動処理
			diff /= limitMoveDistance;
			if (diff.x > 0) {
				crossKey.Right = diff.x;
				crossKey.Left = 0f;
				//rightImage.sprite = sptCrossPushedRight;
				//leftImage.sprite = sptCrossLeft;
			} else {
				crossKey.Left = -diff.x;
				crossKey.Right = 0f;
				//rightImage.sprite = sptCrossRight;
				//leftImage.sprite = sptCrossPushedLeft;
			}


			if (diff.y > 0) {
				crossKey.Up = diff.y;
				crossKey.Down = 0f;
				//upImage.sprite = sptCrossPushedUp;
				//downImage.sprite = sptCrossDown;
			}
			else {
				crossKey.Down = -diff.y;
				crossKey.Up = 0f;
				//upImage.sprite = sptCrossUp;
				//downImage.sprite = sptCrossPushedDown;
			}

            crossKey.Phase = info.Phase;
            if (info.Phase == InputPhase.Began) {
                crossKey.Id = info.Id;
            	infoManager.UpdateParam (crossKey, null);
			} else {
                infoManager.UpdateParam(crossKey, null);
            }
        }

		public int ControlledTouchID {
			get {
				return crossKey.Id;
			}
		}

		public InputInfoManager<KeyInfo> InfoManager {
			set {
				infoManager = value;
			}
		}
		#endregion
	}
}