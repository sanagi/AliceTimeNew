using InputSupport;
using KamioriInput;

public interface IVirtualControllerEvent {
	void OnFireEvent(KamioriInput.TouchInfo info);
	int ControlledTouchID{ get; }
	InputInfoManager<KeyInfo> InfoManager { set; }
}
