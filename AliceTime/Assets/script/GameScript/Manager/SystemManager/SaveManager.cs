/***
 * 
 * リリース後にセーブデータの構造が変更された場合の対応
 * -> UpdateDataで変換処理を必ず実装する
 * 
 ***/

using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using MsgPack;


public class SaveManager : SingletonMonoBehaviour<SaveManager> 
{
	public enum StoryClearState{
		NONE,
		BAD,
		TRUE
	}

	/// セーブデータ情報
	[System.Serializable]
	public class SaveData
	{
		public int AppVersion;
		public int LastLoginTime;

		public Option option;
		public List<StoryData> story;
		public List<TrialData> trial;
		//トライアルがどれくらい進んでいるか(5区切り)
		public int trialPhase=1;

		public ContinueData continueData;

		public StoryClearState lastStory;
		public GameAllData allData;

		public SaveData()
		{
			option = new Option();
			story = new List<StoryData>();
			trial = new List<TrialData>();
			trialPhase = 1;
            continueData = new ContinueData();
			allData = new GameAllData();


			lastStory = StoryClearState.NONE;
        }
	}

	[System.Serializable]
	public class Option
	{
		public float bgmVolume;
		public float seVolume;
		public OptionManager.LanguageType languageType;
		public float controllerPositionRate;
		public float controllerSizeRate;
		public float textSpeed;
		public OptionManager.FoldAnimationSkip foldAnimationSKip;
        public OptionManager.MoveSpeed moveSpeed;


        public Option() {
			bgmVolume = 0.2f;
			seVolume = 1f;
			controllerPositionRate = 0.5f;
			controllerSizeRate = 0.5f;
			textSpeed = 0.5f;
			foldAnimationSKip = OptionManager.FoldAnimationSkip.OFF;
            moveSpeed = OptionManager.MoveSpeed.Quick; 
            languageType = OptionManager.LanguageType.Japanese;
        }
	}

	/// ストーリー用データ
	[System.Serializable]
	public class StoryData
	{
		public int StageNumber;

		public bool isClear;
		public bool isOpen;

		public List<int> OpenAreaNumber;
		public List<int> ReadEvent;
		public List<int> GotFragment;

		public StoryData(int stageNum) 
		{
			StageNumber = stageNum;

			isClear = false;
            isOpen = stageNum == 0 ? true : false;

			OpenAreaNumber = new List<int>();
			ReadEvent = new List<int>();
			GotFragment = new List<int>();
		}
	}

	/// トライアル用データ
	[System.Serializable]
	public class TrialData
	{
		public int StageNumber;

		public TrialData(int stageNum) 
		{
			StageNumber = stageNum;
		}
	}

    /// コンティニュー用データ
    [System.Serializable]
    public class ContinueData
    {
        public int stageNumber = -1;
        public int areaNumber = -1;
        public Vector3 respawnPosition;
        public bool isCreate = false;

        public void SetContinueData(int stageNum, int areaNum, Vector3 respornPos)
        {
            stageNumber = stageNum;
            areaNumber = areaNum;
            respawnPosition = respornPos;
            isCreate = true;
        }

        public void InitializeContinueData()
        {
            stageNumber = -1;
            areaNumber = -1;
            respawnPosition = Vector3.zero;
            isCreate = false;
        }

        public void ResetStageData()
        {
            stageNumber = -1;
            areaNumber = -1;
            respawnPosition = Vector3.zero;
        }
    }

	/// <summary>
	/// ゲーム全体の進行状況
	/// やり直しにより覆らないステータス
	/// </summary>
	[System.Serializable]
	public class GameAllData{
		//ギャラリーモードの解放
		public bool isGaralleyOpen = false;
		//TrueEndingを見たか
		public bool isStoryTrueClear = false;
		//トライアルモードの解放
		public bool isTrialOpen = false;
		//全クリ
		public bool isAllClear = false;
        //全クリチェックを見たか
        public bool isAllClearCheck = false;

        public GameAllData(){
			isGaralleyOpen = false;
			isStoryTrueClear = false;
			isTrialOpen = false;
            isAllClearCheck = false;

        }

		public void SetGalleryOpen(){
			isGaralleyOpen = true;
		}

		public void SetStoryTrueClear(){
			isStoryTrueClear = true;
		}
		public void SetTrialOpen(){
			isTrialOpen = true;
		}

		public void SetAllClear(){
			isAllClear = true;
		}
        public void SetAllClearChecked()
        {
            isAllClearCheck = true;
        }
	}

	private ObjectPacker packer;

	[SerializeField] private string pathSaveData;
	[SerializeField] private SaveData saveData;

	//次ステージの開放演出を入れるかどうか
	public bool clearNewStage;
	public int NowPlayingStage;
    public bool tmpFromEnding;

    protected override void Init() {
		base.Init();

        saveData = new SaveData();
        packer = new ObjectPacker();
        tmpFromEnding = false;
        pathSaveData = "";
        
        pathSaveData = Application.persistentDataPath + "/storage/SAVEDATA";

        Read();
        gameObject.AddComponent<OptionManager>();
    }

    protected override void OnDestroy()
    {
	    
    }

    /// セーブデータの読み込み
    private void Read()
	{
		// 対象のディレクトリが存在しない時
        if (!Directory.Exists (Directory.GetParent(pathSaveData).FullName)) {
			Directory.CreateDirectory (Directory.GetParent(pathSaveData).FullName);
#if UNITY_EDITOR
			Debug.Log(Directory.GetParent(pathSaveData).FullName + " is not found");
#endif
			//return;
		}

		// 対象のファイルが存在しない時
		if (!File.Exists (pathSaveData)) {
#if UNITY_EDITOR
			Debug.Log(pathSaveData + " is not found");
#endif
			saveData = new SaveData ();
			Write ();
			return;
		}
#if UNITY_EDITOR
		Debug.Log(pathSaveData);
#endif
		var Loaded_Data = File.ReadAllBytes(pathSaveData);
		if(Loaded_Data.Length == 0) {
			return;
		}

		var Dec_Loaded_Data = Decrypt (Loaded_Data);
		saveData = packer.Unpack<SaveData>(Dec_Loaded_Data);
	}

    /// セーブデータの書き込み
    private void Write()
	{
		// 対象のディレクトリが存在しない時
        if (!Directory.Exists (Directory.GetParent(pathSaveData).FullName)) {
			Directory.CreateDirectory (Directory.GetParent(pathSaveData).FullName);
		}
		
#if UNITY_EDITOR
			Debug.Log("DataSaved");

#endif

        var Saved_Data = packer.Pack (saveData); //シリアライズ
		var Enc_Saved_Data = Encrypt(Saved_Data);
		File.WriteAllBytes(pathSaveData, Enc_Saved_Data);
	}

#region ストーリー関連
    /// クリア済みステージデータの書き込み
    public void Write_ClearStoryStage(int stageNum)
    {
		var stageData = saveData.story.Find (s => s.StageNumber == stageNum);
		if (stageData == null) {
            var clearedStage = new StoryData(stageNum);
            clearedStage.isClear = true;
            clearedStage.isOpen = false;
            saveData.story.Add(clearedStage);
            var newStage = new StoryData(stageNum + 1);
            newStage.isOpen = true;
            saveData.story.Add(newStage);
        } else if (stageData.isOpen) {
            stageData.isOpen = false;
			stageData.isClear = true;
            var newStage = new StoryData(stageNum + 1);
            newStage.isOpen = true;
            saveData.story.Add (newStage);
		}
		Write ();
    }

    /// クリア済みステージデータの読み込み　
	public List<StoryData> Read_ClearStoryStage()
    {
		return saveData.story;
    }

	// クリア済みのステージデータの確認
	public bool Check_ClearStoryStage(int stageNum)
	{
		var stageData = saveData.story.Find (s => s.StageNumber == stageNum);
		if (stageData != null) {
			return stageData.isClear;
		}
		return false;
	}
#endregion

#region コンティニュー関連
    //書き込みは「Write_OpenStoryArea」関数内にて
    /// コンティニューデータの書き込み
    public void Write_ContinueData(int stageNum, int areaNum, Vector3 respornPosition)
    {
        //Debug.Log("Continue Data Updated\nRespawnPos = " + respornPosition);
        saveData.continueData.SetContinueData(stageNum, areaNum, respornPosition);
        Write();
    }

    /// コンティニューデータの読み込み
    public ContinueData Read_ContinueData()
    {
        //存在しなかったら中身作って初期化
        if (saveData.continueData == null)
        {
            saveData.continueData = new ContinueData();
            saveData.continueData.stageNumber = -1;
            saveData.continueData.areaNumber = -1;
            saveData.continueData.isCreate = false;
            saveData.continueData.respawnPosition = Vector3.zero;
        }
        return saveData.continueData;
    }
#endregion

#region ステージイベント関連
    /// 読み終えたステージイベントの書き込み
    public void Write_DoneStageEvent(int stageNum, int eventID)
	{
		var stageData = saveData.story.Find (s => s.StageNumber == stageNum);
		if (stageData == null) {
			stageData = new StoryData (stageNum);
			saveData.story.Add (stageData);
		}

		foreach (var readID in stageData.ReadEvent) {
			if (readID == eventID) {
				return;
			}
		}
		stageData.ReadEvent.Add (eventID);
		Write ();
	}

    /// 読み終えたステージイベントの読み込み
	public List<int> Read_DoneStageEvent(int stageNum)
    {
		var stageData = saveData.story.Find (s => s.StageNumber == stageNum);
		if (stageData == null) {
			stageData = new StoryData (stageNum);
			saveData.story.Add (stageData);
		} else {
			return stageData.ReadEvent;
		}
		return null;
	}

    /// 読み終えたステージイベントがあるかどうかチェック
	public bool Check_DoneStageEvent(int stageNum, int eventID)
	{
		var stageData = saveData.story.Find (s => s.StageNumber == stageNum);
		if (stageData == null) {
			return false;
		} else {
			foreach (var readID in stageData.ReadEvent) {
				if (readID == eventID) {
					return true;
				}
			}
		}
		return false;
	}
#endregion

#region フラグメントイベント関連
	/// ゲットしたかけらの個数の書き込み
	public void Write_GotFragment(int stageNum, int fragmentID)
	{
		if (Check_GotFragment (stageNum, fragmentID)) {
			return;
		}

		var stageData = saveData.story.Find (s => s.StageNumber == stageNum);
		stageData.GotFragment.Add (fragmentID);
		Write ();
	}

	/// ゲットしたかけらの個数の読み込み
	public List<int> Read_GotFragment(int stageNum)
	{
		var stageData = saveData.story.Find (s => s.StageNumber == stageNum);
		if (stageData == null) {
			Debug.LogWarning ("うそん。。。");
		} else {
			return stageData.GotFragment;
		}
		return null;
	}

	// ゲットしたかけらの個数を取得
	public int Count_GotFragment(int stageNum) {
		var stageData = saveData.story.Find (s => s.StageNumber == stageNum);
		if (stageData == null) {
			Debug.LogWarning ("うそん。。。");
		} else {
			return stageData.GotFragment.Count;
		}
		return 0;
	}

	//ゲットしたかけらの総数
	public int GetAllFragment(){
		int num = 0;
		for(int i=4; i<=16;i=i+4){
			var stageData = saveData.story.Find (s => s.StageNumber == i);
            if(stageData != null)
            {
                num += stageData.GotFragment.Count;
            }
        }
		return num;
	}

	public bool Check_GotFragment(int stageNum, int fragmentID)
	{
		var stageData = saveData.story.Find (s => s.StageNumber == stageNum);
		if (stageData == null) {
			stageData = new StoryData (stageNum);
			saveData.story.Add (stageData);
			return false;
		}

		foreach (var gotID in stageData.GotFragment) {
			if (gotID == fragmentID) {
				return true;
			}
		}
		//stageData.GotFragment.Add (fragmentID);
		return false;
	}
#endregion

#region エリア到達関連
	public void Write_OpenStoryArea(int stageNum, int areaNum)
	{
		var stageData = saveData.story.Find (s => s.StageNumber == stageNum);
		if (stageData == null) {
			stageData = new StoryData (stageNum);
			saveData.story.Add (stageData);
		}

		//コンティニュー用データ一時保存
		if(MainGameManager.CurrentStageID == 0) { 
			Write_ContinueData(stageNum, areaNum, MainGameManager.RespawnPosition);
		}

        foreach (var areaNumber in stageData.OpenAreaNumber)
        {
            if (areaNumber == areaNum)
            {
                return;
            }
        }
        stageData.OpenAreaNumber.Add(areaNum);
		Write();
	}

	public List<int> Read_OpenStoryArea(int stageNum)
	{
		var stageData = saveData.story.Find (s => s.StageNumber == stageNum);
		if (stageData == null) {
			return null;
		} else {
			return stageData.OpenAreaNumber;
		}
	}

	public bool Check_OpenStoryArea(int stageNum, int areaNum)
	{
		var OpenAreaList = SaveManager.Instance.Read_OpenStoryArea (stageNum);
		if (OpenAreaList == null)
			return false;
		
		foreach (var areaNumber in OpenAreaList) {
			if (areaNumber == areaNum) {
				return true;
			}
		}
		return false;
	}
#endregion

#region

	/// <summary>
	/// トライアル開ける演出走った時
	/// </summary>
	public void OpenTrial(){
		saveData.allData.SetTrialOpen();
		Write();
	}
	public bool GetOpenTrial(){
		return saveData.allData.isTrialOpen;
	}

	public void OpenGallrey(){
		saveData.allData.SetGalleryOpen();
		Write();
	}
	public bool GetGalleryOpen(){
		return saveData.allData.isGaralleyOpen;
	}
	public void TrueEndGet(){
		saveData.allData.SetStoryTrueClear();
        if(Read_ClearTrialStage().Length == 40)
        {
            SetAllClear();
        }
		Write();
	}
	public bool GetTrueEnd(){
		return saveData.allData.isStoryTrueClear;
	}
	public void SetAllClear(){
		saveData.allData.SetAllClear();
        Write();
    }
    public void SetAllClearCheck()
    {
        saveData.allData.SetAllClearChecked();
        Write();
    }

    public bool GetAllClear(){
		return saveData.allData.isAllClear;
	}
    public bool GetAllClearChecked()
    {
        return saveData.allData.isAllClearCheck;
    }

#endregion

	/// <summary>
	/// NormalEnd視聴
	/// </summary>
	public void ClearBadStory(){
		saveData.lastStory = StoryClearState.BAD;
		Write();
	}

	/// <summary>
	/// TrueEnd視聴
	/// </summary>
	public void ClearAllStory(){
		saveData.lastStory = StoryClearState.TRUE;
        TrueEndGet();
		Write();
	}

	public StoryClearState GetStoryState(){
		return saveData.lastStory;
	}

#region トライアルステージ関連
	/// クリア済みステージデータの書き込み
	public void Write_ClearTrialStage(int stageNum)
    {
		var currentStageData = saveData.trial.Find (t => t.StageNumber == stageNum);
		if (currentStageData == null) {
			saveData.trial.Add (new TrialData (stageNum));
			Write ();
		}
    }

	/// クリア済みステージデータの読み込み　
	public TrialData[] Read_ClearTrialStage()
    {
		return saveData.trial.ToArray();
    }

	public void IncTrialPhase(){
		saveData.trialPhase++;
		Write();
	}

	public int GetTrialPhase(){
		return saveData.trialPhase;
	}
	//次のトライアルフェーズに進んでいいか
	public bool CheckNextTrialPhase(){
		int begin = ((saveData.trialPhase-1)*5)+1;
		int count=0;
		for(int i = begin;i<=begin+5;i++){
			if(Check_ClearTrialStage(i)){
				count++;
			}
		}
		if(count>=3 && saveData.trialPhase < 8){
			return true;
		}
		else{
			return false;
		}
	}

	public bool Check_ClearTrialStage(int stageNum)
	{
		var stagesData = saveData.trial.Find (s => s.StageNumber == stageNum);
		return stagesData != null ? true : false;
	}
#endregion

    /// ストーリー「はじめから」選択(データ初期化)
    public void Story_Hajime()
    {
        var continueData = SaveManager.Instance.Read_ContinueData();
		continueData.InitializeContinueData();
	    saveData.story.Clear ();
		Write ();
    }

#region オプション関連
	public Option GetOptions() {
		if(saveData == null) {
			Read();
		}
		return saveData.option;
	}
	public void SetOptions(Option option) {
		saveData.option = option;
		Write();
	}
#endregion


#region MessagePack
	// 256bit(32byte)のInitVector（初期ベクタ）とKey（暗号キー）
	private const string AesInitVector  = @"C0JXHAjPnwPyONpAawM3QPnRJypbEQTf";
	private const string AesKey         = @"PnRJypbEQTfwwUpTJvvq6Wcyy4fu3-B7";
	private const int   BlockSize       = 256;
	private const int   KeySize         = 256;

	/// <summary>
	/// 暗号化
	/// </summary>
	public byte[]  Encrypt( byte[] binData )
	{
		RijndaelManaged myRijndael  = new RijndaelManaged();
		myRijndael.Padding          = PaddingMode.Zeros;
		myRijndael.Mode             = CipherMode.CBC;
		myRijndael.KeySize          = KeySize;
		myRijndael.BlockSize        = BlockSize;

		byte[] key            = new byte[0];
		byte[] InitVector     = new byte[0];

		key         = System.Text.Encoding.UTF8.GetBytes( AesKey );
		InitVector  = System.Text.Encoding.UTF8.GetBytes( AesInitVector );

		ICryptoTransform encryptor = myRijndael.CreateEncryptor( key, InitVector );

		MemoryStream msEncrypt  = new MemoryStream();
		CryptoStream csEncrypt  = new CryptoStream( msEncrypt, encryptor, CryptoStreamMode.Write );

		byte[] src = binData;

		// 暗号化する
		csEncrypt.Write( src, 0, src.Length );
		csEncrypt.FlushFinalBlock();

		byte[] dest = msEncrypt.ToArray();

		return dest;
	}
		
	/// <summary>
	/// 複合化
	/// </summary>
	public byte[] Decrypt ( byte[] binData )
	{

		RijndaelManaged myRijndael  = new RijndaelManaged();
		myRijndael.Padding          = PaddingMode.Zeros;
		myRijndael.Mode             = CipherMode.CBC;
		myRijndael.KeySize          = KeySize;
		myRijndael.BlockSize        = BlockSize;

		byte[] key                  = new byte[0];
		byte[] InitVector           = new byte[0];

		key         = System.Text.Encoding.UTF8.GetBytes( AesKey );
		InitVector  = System.Text.Encoding.UTF8.GetBytes( AesInitVector );

		ICryptoTransform decryptor = myRijndael.CreateDecryptor( key, InitVector );
		byte[] src  = binData;
		byte[] dest = new byte[ src.Length ];

		MemoryStream msDecrypt = new MemoryStream( src );
		CryptoStream csDecrypt = new CryptoStream( msDecrypt, decryptor, CryptoStreamMode.Read );

		// 複号化する
		csDecrypt.Read( dest, 0, dest.Length );

		return dest;
	}
#endregion
}
