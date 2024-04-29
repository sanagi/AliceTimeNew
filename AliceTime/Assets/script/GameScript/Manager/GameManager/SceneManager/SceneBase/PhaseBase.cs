using UnityEngine;
using System.Collections;

[System.Serializable]
public class PhaseBase 
{
	/// <summary>フェーズの名称</summary>
	public virtual string PhaseName { 
		get {
			return "";
		}
	}
		
	/// <summary>フェーズに入った時に呼ばれる</summary>
	public virtual void OnEnter (PhaseBase prevPhase)
	{

	}

	/// <summary>フェーズを出るときに呼ばれる</summary>
	public virtual void OnExit (PhaseBase nextPhase)
	{

	}
}
