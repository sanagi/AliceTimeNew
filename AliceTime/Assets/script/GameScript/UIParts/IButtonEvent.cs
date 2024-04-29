using UnityEngine;
using System.Collections;

public interface IButtonEvent {
	void FireButtonEvent();
	void ReleaseButtonEvent();
	void ReleaseOutButtonEvent();
}
