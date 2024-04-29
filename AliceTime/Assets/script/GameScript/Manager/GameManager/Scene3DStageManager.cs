using UnityEngine;
using System;
using System.Collections;
using Unity.Mathematics;

public class Scene3DStageManager : SingletonMonoBehaviour<Scene3DStageManager>
{
    public enum CreateType
    {
        AreaSelect,
        Explore
    }
    private bool isCreatedPlayer;
    private bool isCreatedFloorStage;

    private CreateType _createType = CreateType.AreaSelect;
    private GameObject _createdStage;

    // 初期化
    protected override void Init() {
        base.Init();

        isCreatedPlayer = false;
        isCreatedFloorStage = false;
    }

    // 終了処理
    protected override void Deinit() {
        base.Deinit();
    }
		
	// ステージオブジェクトのリセット
	public void ResetStage() {
		Destroy(_createdStage);
    }

    private string GetFolderPath(CreateType createType)
    {
        switch (createType)
        {
            case CreateType.AreaSelect:
                return "AreaSelect";
            case CreateType.Explore:
                return "Explore";
        }

        return "";
    }

    /// <summary>
    /// ステージの生成
    /// </summary>
    /// <param name="mode">Mode.</param>
    /// <param name="stageId">Stage identifier.</param>
    /// <param name="complete">Complete.</param>
    public void CreateStage(CreateType createType, string floorId, Action<Scene3DParam> complete) {
        instance.StartCoroutine(instance.Read(createType ,floorId, (floorInfo) => {
            //ステージ生成
            var folderPath = GetFolderPath(createType);
            var stagePath = String.Format("StageData/{0}/{1}", folderPath, floorInfo.StagePath);
            instance.GenerateObj(stagePath, floorInfo.Stage_Pos, Quaternion.Euler(floorInfo.Stage_Rot), (createdStage) => {
                isCreatedFloorStage = true;
                _createdStage = createdStage;
                
                DebugManager.Instance.SetAreaName(floorInfo.DisplayName);
                
                var playerPath = String.Format("Player/{0}", floorInfo.PlayerPath);
                //プレイヤー生成
                instance.GenerateObj(playerPath, floorInfo.Player_InitPos, quaternion.Euler(floorInfo.Player_InitRot), (createdPlayers) => {
                    isCreatedPlayer = true;
                    instance.StartCoroutine(instance.WaitGenerateStage(() => {
                            complete(floorInfo);
                    }));
                });
            });
        }));
    }


    /// <summary>
    /// ステージデータの読み込み
    /// </summary>
    /// <param name="mode">Mode.</param>
    /// <param name="StageID">Stage I.</param>
    private IEnumerator Read(CreateType createType, string sceneID, Action<Scene3DParam>　complete)
    {
        var folderPath = GetFolderPath(createType);
        var path = String.Format("StageData/{0}/{1}SceneParam", folderPath, sceneID);;

        // ステージデータの読み込み（1area/frame）
        var floorInfo = Resources.Load<Scene3DParam>(path);

        yield return null;
        
        Resources.UnloadUnusedAssets();
        complete(floorInfo);
    }

    private void GenerateObj(string prefabPath, Vector3 pos, Quaternion rot, Action<GameObject> complete) {
        instance.StartCoroutine(instance.LoadObjectsAsync(prefabPath, (prefab) => {
            instance.StartCoroutine(instance.InstantiateAsync(prefab, pos, rot,(createdStageObj) =>
            {
                complete(createdStageObj);
            }));
        }));
    }

    /// <summary>
    /// 非同期なプレハブの読み込み
    /// </summary>
    /// <returns>The object async.</returns>
    /// <param name="objData">Object data.</param>
    /// <param name="complete">Complete.</param>
    private IEnumerator LoadObjectsAsync(string objPath, Action<GameObject> complete) {
        var prefab = new GameObject();
        ResourceRequest request = Resources.LoadAsync<GameObject>(objPath);
        while (request.isDone == false) {
            yield return null;
        }
        prefab = (GameObject)request.asset;
        yield return null;
        

        Resources.UnloadUnusedAssets();
        complete(prefab);
    }

    /// <summary>
    /// 非同期なテクスチャの読み込み
    /// </summary>
    /// <returns>The textures async.</returns>
    /// <param name="objData">Object data.</param>
    /// <param name="complete">Complete.</param>
    /*private IEnumerator LoadTexturesAsync(StageObjData[] objData, Action<Texture2D[], StageObjData[]> complete) {
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
    */

    /// <summary>
    /// 非同期なオブジェクト生成
    /// </summary>
    /// <returns>The async.</returns>
    /// <param name="objData">Object data.</param>
    /// <param name="prefabs">Prefabs.</param>
    /// <param name="parent">Parent.</param>
    /// <param name="complete">Complete.</param>
    private IEnumerator InstantiateAsync(GameObject prefab, Vector3 pos, Quaternion rot, Action<GameObject> complete) {
        var createdObj = GameObject.Instantiate(prefab, pos, rot);
        yield return null;
        
        Resources.UnloadUnusedAssets();
        complete(createdObj);
    }

    /// <summary>
    /// ステージの生成待ち
    /// </summary>
    /// <returns>The generate stage.</returns>
    /// <param name="complete">Complete.</param>
    private IEnumerator WaitGenerateStage(Action complete) {
        while (!isCreatedFloorStage || !isCreatedPlayer) {
            yield return null;
        }
        
		isCreatedPlayer = false;
        isCreatedFloorStage = false;

        System.GC.Collect();
		Resources.UnloadUnusedAssets();
        PlayerManager.Instance.WakeUpRigidBody(); //最初のエリアとのあたり判定用にRigidbodyにWakeup命令
        complete();
    }
}