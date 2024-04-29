using UnityEngine;
using System.Collections;

public class Game_Tutorial : PhaseBase {
	public override string PhaseName { 
		get {
			return this.GetType ().FullName;
		}
	}

	public override void OnEnter (PhaseBase prevPhase)
	{

	}

	public override void OnExit (PhaseBase nextPhase)
	{

	}

    public void OnControl()
    {

    }
    public void OffControl()
    {

    }

    public void EnableTouch()
    {

    }
    public void DisableTouch()
    {

    }
}
