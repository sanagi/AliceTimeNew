using UnityEngine;
using System.Collections;

public class AliceController : PlayerController {
	[SerializeField]
	private PlayerParams defaultParam = null;

	private PlayerParams _currentParam = null;
	public PlayerParams GetPlayerDefaultParam => _currentParam;

	/// <summary>
    /// 基本的にはKeyPointを同時に作る
    /// </summary>
    private void Start()
    {
	    transform.localScale = defaultParam.characterScale;
	    
	    //XY軸は自分
	    //PlayerManager.Instance.SetPlayerXY(this);
    }

    protected override void SetupDefaultPlayerParams()
	{
		if (defaultParam == null) {
			Debug.LogError ("「PlayerParamsをセットしてください…」");
			return;
		}
		
        //float limitJump = defaultLimitJump;

        if (OptionManager.Instance.GetMoveSpeed() == OptionManager.MoveSpeed.Quick)
        {
            //param.moveSpeed *= 1.6f;
            //param.moveLimitInJump = limitJump * 0.68f;
        }
        else
        {
	        //param.moveLimitInJump = defaultLimitJump;
        }

		SetPlayerParams (defaultParam);
		_currentParam = defaultParam;
	}

	protected override void SetupAnimation()
	{
		if (GetComponent<AliceAnimation>() == null) {
			Debug.LogError ("「Animationをセットしてください…」");
			return;
		}
		SetPlayerAnimation (GetComponent<AliceAnimation>());
	}
}
