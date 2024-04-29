using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

public class GearStageManager : SingletonMonoBehaviour<GearStageManager>
{
    // 1エリアのサイズ定義
    private const float AREA_HEIGHT = 10f;
    private const float AREA_WIDTH = 17.775f;
    public Vector2 GetAreaSize() {
        return new Vector2(AREA_WIDTH, AREA_HEIGHT);
    }

    private class StageValues
    {
        public const string EOF = "EOF";
        public const string PAPER = "PAPER";
        public const string TICKET = "TICKET";
        public const string STORYITEM = "STORYITEM";
        public const string TUTORIALITEM = "TUTORIALITEM";
        public const string BLOCK = "BLOCK";
        public const string LIFT = "LIFT";
        public const string BACK_LIFT = "BACKLIFT";
        public const string EVENT = "EVENT";
        public const string FEVENT = "FEVENT";
        public const string GIMICK = "GIMICK";
        public const string STAGENUM = "STAGENUM";
        public const string TIME = "TIME";
        public const string PLAYER = "PLAYER";
        public const string DEATH = "DEATH";
        public const string DEATH2 = "DEATH2";
        public const string COLLIDER = "COLLIDER";
        public const string TUTORIAL = "TUTORIAL";
        public const string RIGHT = "RIGHT";
        public const string DOWN = "DOWN";
        public const string LEFT = "LEFT";
        public const string UP = "UP";
        public const string BACK = "BACK";
        public const string MAP = "MAP";
        public const string SETSUNA = "SETSUNA";
        public const string ORIGAMI = "ORIGAMI";
    }

    private class StageObjData
    {
        public StageObjData(int areaId, string path, Vector3 pos, Vector2 scale, float rot, string[] param, string objName = "") {
            AreaId = areaId;
            ObjPath = path;
            ObjPos = pos;
            ObjScale = scale;
            ObjRot = rot;
            ObjName = objName;
            Params = param;
        }

        public int AreaId;
        public string ObjPath;
        public Vector3 ObjPos;
        public Vector2 ObjScale;
        public float ObjRot;
        public string ObjName;
        public string[] Params;
    }

    private class StageData
    {
        public StageData() {
            Players = new List<StageObjData>();
            Papers = new List<StageObjData>();
            Stages = new List<StageObjData>();
            Lifts = new List<StageObjData>();
            Gimicks = new List<StageObjData>();
            Events = new List<StageObjData>();
            Backs = new List<StageObjData>();
            MapData = new List<MapData>();
        }

        public List<StageObjData> Players;
        public List<StageObjData> Papers;
        public List<StageObjData> Stages;
        public List<StageObjData> Lifts;
        public List<StageObjData> Gimicks;
        public List<StageObjData> Events;
        public List<StageObjData> Backs;
        public List<MapData>      MapData;
    }

    private class MapData
    {
        public MapData(int areaId, int x, int y) {
            AreaId = areaId;
            X = x;
            Y = y;
        }

        public int AreaId;
        public int X;
        public int Y;
    }

    public class LinkFlag
    {
        public LinkFlag() {
            IsRight = false;
            IsLeft = false;
            IsUp = false;
            IsDown = false;
        }

        public bool IsRight;
        public bool IsLeft;
        public bool IsUp;
        public bool IsDown;
    }

    public GameObject MapItemParent;
    private List<GameObject> MapItems = new List<GameObject>();

    private Transform[] areaTransforms;
	public Transform[] AreaTransforms {
		get {
			return areaTransforms;		
		}
	}
    private GameObject[] paperSystems;
	public GameObject[] PaperSystems {
		get {
			return paperSystems;
		}
	}
    private LinkFlag[] areaLinks;

    private Vector2 originMapPos;

    private bool isCreatedPlayer;
    private bool isCreatedPaper;
    private bool isCreatedStage;
    private bool isCreatedLifts;
    private bool isCreatedGimick;
    private bool isCreatedEvent;
    private bool isCreatedBack;

    //導火線ギミックの数
    private int fuseID;

    // 初期化
    protected override void Init() {
        base.Init();

        isCreatedPlayer = false;
        isCreatedPaper = false;
        isCreatedStage = false;
        isCreatedLifts = false;
        isCreatedGimick = false;
        isCreatedEvent = false;
        isCreatedBack = false;

        // マップ表示用オブジェクト郡の取得
        // TODO どうにかしたい
        /*Transform tMapItemParent = MapItemParent.transform;
        foreach (Transform child in tMapItemParent) {
            MapItems.Add(child.gameObject);
        }

        originMapPos = Vector2.zero;

        fuseID = 0;
        */
    }

    // 終了処理
    protected override void Deinit() {
        base.Deinit();
    }
		
	// ステージオブジェクトのリセット
	public void ResetStage() {
		foreach(Transform child in transform) {
			Destroy(child.gameObject);
		}
	}

    /// <summary>
    /// ステージの生成
    /// </summary>
    /// <param name="mode">Mode.</param>
    /// <param name="stageId">Stage identifier.</param>
    /// <param name="complete">Complete.</param>
    /*public void CreateStage(GAMEMODE mode, int stageId, Action complete) {
        instance.StartCoroutine(instance.Read(mode, stageId, (areaData) => {
            instance.GeneratePlayerObj(areaData.Players, (createdPlayers) => {
                isCreatedPlayer = true;
            });
        }));
    }


    /// <summary>
    /// ステージデータの読み込み
    /// </summary>
    /// <param name="mode">Mode.</param>
    /// <param name="StageID">Stage I.</param>
    private IEnumerator Read(GAMEMODE mode, int stageID, Action<StageData> complete) {
        var stageData = new StageData();
        var path = String.Format("StageData/{0}/{1}/", mode == GAMEMODE.STORY ? "Story" : "Trial", stageID);
        

        yield return null;
        

        Resources.UnloadUnusedAssets();

        complete(stageData);
    }*/

    private void GeneratePlayerObj(List<StageObjData> players, Action<GameObject[]> complete) {
        var view3D = GameObject.Find("View3D").transform;
        instance.StartCoroutine(instance.LoadObjectsAsync(players.ToArray(), (prefabs, objData) => {
            instance.StartCoroutine(instance.InstantiateAsync(objData, prefabs, view3D, (createdObjs) => {
                for(var i=0;  i < createdObjs.Length; i++) {
                    var createdObj = createdObjs[i];
                }
                complete(createdObjs);
            }));
        }));
    }

    private void GenerateBacksObj(List<StageObjData> backs, Action complete) {
        instance.StartCoroutine(instance.LoadTexturesAsync(backs.ToArray(), (textures, objData) => {
            for (var i = 0; i < textures.Length; i++) {
                var paper = paperSystems[i].transform.Find("Paper");
                Material backMaterial = paper.GetComponent<MeshRenderer>().material;
                backMaterial.mainTexture = textures[i];
            }
            complete();
        }));
    }


    /// <summary>
    /// 非同期なプレハブの読み込み
    /// </summary>
    /// <returns>The object async.</returns>
    /// <param name="objData">Object data.</param>
    /// <param name="complete">Complete.</param>
    private IEnumerator LoadObjectsAsync(StageObjData[] objData, Action<GameObject[], StageObjData[]> complete) {
        var prefabs = new GameObject[objData.Length];
        for (var i = 0; i < objData.Length; i++) {
            ResourceRequest request = Resources.LoadAsync<GameObject>(objData[i].ObjPath);
            while (request.isDone == false) {
                yield return null;
            }
            prefabs[i] = (GameObject)request.asset;
            yield return null;
        }

        Resources.UnloadUnusedAssets();
        complete(prefabs, objData);
    }

    /// <summary>
    /// 非同期なテクスチャの読み込み
    /// </summary>
    /// <returns>The textures async.</returns>
    /// <param name="objData">Object data.</param>
    /// <param name="complete">Complete.</param>
    private IEnumerator LoadTexturesAsync(StageObjData[] objData, Action<Texture2D[], StageObjData[]> complete) {
        var textures = new Texture2D[objData.Length];
        for (var i = 0; i < objData.Length; i++) {
            ResourceRequest request = Resources.LoadAsync<Texture2D>(objData[i].ObjPath);
            while (request.isDone == false) {
                yield return null;
            }
            textures[i] = (Texture2D)request.asset;
            yield return null;
        }

        Resources.UnloadUnusedAssets();
        complete(textures, objData);
    }

    /// <summary>
    /// 非同期なオブジェクト生成
    /// </summary>
    /// <returns>The async.</returns>
    /// <param name="objData">Object data.</param>
    /// <param name="prefabs">Prefabs.</param>
    /// <param name="parent">Parent.</param>
    /// <param name="complete">Complete.</param>
    private IEnumerator InstantiateAsync(StageObjData[] objData, GameObject[] prefabs, Transform parent, Action<GameObject[]> complete) {
        var createdObjs = new GameObject[objData.Length];
        yield return null;
        
        Resources.UnloadUnusedAssets();
        complete(createdObjs);
    }

    /// <summary>
    /// 非同期なオブジェクト生成
    /// </summary>
    /// <returns>The async.</returns>
    /// <param name="objData">Object data.</param>
    /// <param name="textures">Textures.</param>
    /// <param name="objPath">Object path.</param>
    /// <param name="parent">Parent.</param>
    /// <param name="complete">Complete.</param>
    private IEnumerator InstantiateAsync(StageObjData[] objData, Texture2D[] textures, string objPath, Transform parent, Action complete) {
        for (var i = 0; i < objData.Length; i++) {
            ResourceRequest request = Resources.LoadAsync<GameObject>(objPath);
            while (request.isDone == false) {
                yield return null;
            }
            var prefab = (GameObject)request.asset;

            var data = objData[i];
            var gameObj = GameObject.Instantiate(prefab, data.ObjPos, Quaternion.AngleAxis(data.ObjRot, Vector3.forward)) as GameObject;
            var scale = gameObj.transform.localScale;
            scale.x *= data.ObjScale.x;
            scale.y *= data.ObjScale.y;
            gameObj.transform.localScale = scale;
            gameObj.transform.SetParent(parent);
            gameObj.GetComponent<Renderer>().material.mainTexture = textures[i];
            yield return null;
        }

        Resources.UnloadUnusedAssets();
        complete();
    }

    /// <summary>
    /// ステージの生成待ち
    /// </summary>
    /// <returns>The generate stage.</returns>
    /// <param name="complete">Complete.</param>
    private IEnumerator WaitGenerateStage(Action complete) {
        while (!isCreatedEvent || !isCreatedGimick || !isCreatedStage || !isCreatedBack || !isCreatedPlayer || !isCreatedPaper || !isCreatedLifts) {
            yield return null;
        }

		isCreatedEvent = false;
		isCreatedGimick = false;
		isCreatedStage = false;
		isCreatedBack = false;
		isCreatedPlayer = false;
		isCreatedPaper = false;
		isCreatedLifts = false;

		System.GC.Collect();
		Resources.UnloadUnusedAssets();
        PlayerManager.Instance.WakeUpRigidBody(); //最初のエリアとのあたり判定用にRigidbodyにWakeup命令
        complete();
    }
}