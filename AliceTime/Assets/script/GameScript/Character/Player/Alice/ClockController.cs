using UnityEngine;
using System.Collections;

public class ClockController : PlayerController {
	[SerializeField]
	private PlayerParams defaultParam = null;
	//[SerializeField]
	//private PlayerParams childParam = null;
	//[SerializeField]
	//private HistoryObjParam _historyObjParam = null;

	//[SerializeField]
	//private GameObject _keyPrefab = null;

	private PlayerParams _currentParam = null;
	public PlayerParams GetPlayerDefaultParam => _currentParam;
	public PlayerAnimation anim = null;

	//現在の姿から始める
	//private HistoryStatus _historyStatus;

	//private KeyPointController _keyPointController;
	//const float defaultLimitJump = 1.0f;

    /// <summary>
    /// 基本的にはKeyPointを同時に作る
    /// </summary>
    private void Start()
    {
	    //_keyPointController = GameObject.Instantiate(_keyPrefab).GetComponent<KeyPointController>();

	    //プレイヤーのそばに移動
	    /*var initialPos =
		    new Vector3(transform.position.x + _keyPointController.KeyPointParameter.playerNearXOffset,
			    transform.position.y + _keyPointController.KeyPointParameter.playerNearYOffset,
			    GameDefine.Z_POS_DANGION);
			    */
	    //KeyPointManager.MoveToPosition(initialPos);

	    //条件で表示か非表示か決める
	    //KeyPointManager.Display();
	    //KeyPointManager.SetState(KeyPointManager.STATE.WAIT);
	    transform.localScale = defaultParam.characterScale;

	    /*_historyStatus = GetComponent<HistoryStatus>();
	    //History関連の初期化
	    if (_historyStatus != null)
	    {
		    _historyStatus.Init(HistoryStatus.TimeStatus.Now, _historyObjParam.isChangeSize);
		    _historyStatus.SetActionOnChange(ChangeChild, ResetAdult);
	    }*/
	    
	    //リスポーン時の挙動設定
	    /*OnRespawn = () =>
	    {
	    /*
		    if (_historyStatus.CurrentStatus == HistoryStatus.TimeStatus.Past)
		    {
			    //子どもだったら大人に戻す
			    _historyStatus.OnChangeNow();
		    }
		    //魔法陣は消す
		    _keyPointController.ResetCircle();
	    
	    };
	    //YZ軸は自分
	    PlayerManager.Instance.SetPlayerYZ(this);
		*/
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

	/*
	private void ChangeChild()
	{
		//SetPlayerParams(childParam);
		//_currentParam = childParam;

		ChangeDisplay();
	}

	private void ResetAdult()
	{
		SetPlayerParams(defaultParam);
		_currentParam = defaultParam;
		
		ChangeDisplay();
	}
	

	private void ChangeDisplay()
	{
		//見た目サイズ変更
		transform.localScale = _currentParam.characterScale;

		//一定時間操作不可にして見た目変更のアニメーション走らせる
		ChangeHistoryAnimation();
	}
	

	/// <summary>
	/// 見た目変えるアニメーション走らせる
	/// </summary>
	private void ChangeHistoryAnimation()
	{
		//円の内側や外側にちょっと補正する挙動があった方が綺麗かも
		//var pos = transform.localPosition;
		//pos.y += _currentParam.historyChangeJump;
		//transform.localPosition = pos;

		//hijackMove += ChangeHistoryAnimationMove;dd
	}
	

	private Vector3 ChangeHistoryAnimationMove(PlayerController player, Vector3 input, Vector3 currentMove)
	{
		return Vector3.zero;
	}
	*/
}
