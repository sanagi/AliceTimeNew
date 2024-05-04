using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using InputSupport;
using System;

namespace KamioriInput
{
	public class Jump : MonoBehaviour, IVirtualControllerEvent
	{
		private InputInfoManager<KeyInfo> infoManager;
		private KeyInfo jumpKey;

		private Transform m_Transform;
		private Vector3 defaultPosition;
        private Vector3 parentPosition;

		private Image image;
		private Sprite sptJumpNormal;
		private Sprite sptJumpPushed;

		private float baseSize = 222; // 設定で変更できるサイズのMax値
		private float buttonPosDiffY = 67.5f; // 設定で変更できるポジションのY差分（0.5で設定されている時、差分0）
		private float buttonPosDiffX = -120f; // 設定で変更できるポジションのX差分（0.5で設定されている時、差分0）

		void Awake() {
			infoManager = KeyInputManager.Instance.InfoManager;

			jumpKey = new KeyInfo();

			m_Transform = transform;
			defaultPosition = m_Transform.position;
            parentPosition = transform.parent.localPosition;

        }

		void Start() {
			sptJumpNormal = Resources.LoadAsync<Sprite>("Input/Jump").asset as Sprite;
			sptJumpPushed = Resources.LoadAsync<Sprite>("Input/Jump_Push").asset as Sprite;

			image = GetComponent<Image>();
			image.sprite = sptJumpNormal;
		}

		void Update() {
			gameObject.transform.localPosition = Vector3.zero;
			if (infoManager == null) {
				infoManager = KeyInputManager.Instance.InfoManager;
			}

			if (jumpKey.Phase == InputPhase.Ended) {
				jumpKey.Jump = 0;
				jumpKey.Id = int.MaxValue;
				jumpKey.Phase = InputPhase.Missing;
				infoManager.UpdateParam(jumpKey, null);
				return;
			}
		}

		void OnEnable() {
			infoManager.Clear();
            SetScaleSize();
		}

        public void SetScaleSize()
        {
            // 設定からボタンのサイズを変更する
            var controllerSizeRate = 0.55f + ((SaveManager.Instance.GetOptions().controllerSizeRate - 0.5f) * 0.15f);
            var rect = GetComponent<RectTransform>();
            rect.sizeDelta = Vector2.one * baseSize * controllerSizeRate;

            // 設定からボタンの位置を変更する
            var controllerPositionRate = (SaveManager.Instance.GetOptions().controllerPositionRate - 0.75f) * 0.5f;
            var buttonPosDiff = new Vector3(buttonPosDiffX, buttonPosDiffY) * controllerPositionRate;
            transform.parent.localPosition = parentPosition + buttonPosDiff;

            m_Transform.position = defaultPosition;
        }

		#region IVirtualControllerEvent implementation
		public void OnFireEvent(TouchInfo info) {
			jumpKey.Jump = 0;
			jumpKey.Phase = info.Phase;
			if (info.Phase == InputPhase.Ended) {
				jumpKey.Id = info.Id;
				infoManager.UpdateParam(jumpKey, null);
				//image.sprite = sptJumpNormal;
				return;
			} else if (info.Phase == InputPhase.Began) {
				jumpKey.Jump = 1;
				jumpKey.Id = info.Id;
				infoManager.UpdateParam(jumpKey, null);
				//image.sprite = sptJumpPushed;
				return;
			} else if (info.Phase == InputPhase.Stay)  {
				jumpKey.Id = info.Id;
				infoManager.UpdateParam(jumpKey, null);
				return;
			}
		}

		public int ControlledTouchID
		{
			get{
				return jumpKey == null ? int.MaxValue : jumpKey.Id ;
			}
		}

		public InputInfoManager<KeyInfo> InfoManager
		{
			set
			{
				infoManager = value;
			}
		}
		#endregion
	}
}