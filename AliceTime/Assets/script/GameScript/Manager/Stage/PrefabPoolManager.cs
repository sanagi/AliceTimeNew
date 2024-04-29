using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



/// <summary>
/// Prefab Poolクラス。
/// </summary>
[AddComponentMenu("Utility/PrefabPoolManager")]
public class PrefabPoolManager : SingletonMonoBehaviour<PrefabPoolManager>
{

    [SerializeField] private Dictionary<string, PrefabFamiliy> prefabFamilies = new Dictionary<string, PrefabFamiliy>();
    //インスペクタ設定用. 必要なデータを保存する.
    public PreloadPrefabInfo[] preloadPrefabInfo;

    //起動時にPreloadPrefabに設定されたObjectを指定個数確保.
    //シーン遷移時にも追加で確保できるようにbase.Awake()前にロードする.
    //このタイミングで行うことによって、廃棄される新しいシーンのMPrefabが,
    //前のシーンから引き継がれているMPrefabにアクセスできる.
    //
    new void Awake() {
        PrefabPoolManager.Instance.PreloadPrefab(preloadPrefabInfo);
        base.Awake();
    }

    //----------------------------　LoadPrefab系　--------------------------//
    public bool LoadPrefab(GameObject prefab) {
        if (prefabFamilies.ContainsKey(prefab.name)) return false;
        prefabFamilies[prefab.name] = new PrefabFamiliy(prefab);
        return true;
    }


    public bool LoadPrefab(string prefabPath) {
        //同名ファイルはロードさせない.
        string prefabName = GetPrefabName(prefabPath);
        if (prefabFamilies.ContainsKey(prefabName)) return false;
        prefabFamilies[prefabName] = new PrefabFamiliy(prefabPath);
        return true;
    }

    public void UnloadPrefab(string prefabPath) {
        string prefabName = GetPrefabName(prefabPath);
        prefabFamilies[prefabName].Unload();
        prefabFamilies[prefabName] = null;
        Resources.UnloadUnusedAssets();
    }

    /// <summary>
    /// プレハブの一括確保を行い、バックグラウンドに待機させる.
    /// 現在新規prefabFamilyのみの適応可能。すでにロードされているけど、確保する量を増やす、ってのは不可能.
    /// </summary>

    public void PreloadPrefab(PreloadPrefabInfo[] ppis) {
        GameObject srcPrefab, instance;
        Transform _transform;
        int amount, i;
        string name;

        if (ppis == null || ppis.Length == 0) {
            return;
        }

        _transform = transform;



        foreach (PreloadPrefabInfo ppi in ppis) {
            srcPrefab = ppi.prefab;
            amount = ppi.amount;
            name = srcPrefab.name;
            //すでに読み込んでいたら次へ.
            if (!LoadPrefab(srcPrefab)) continue;

            for (i = 0; i < amount; i++) {
                instance = (GameObject)Instantiate(srcPrefab);
                instance.name = name;
                instance.transform.parent = _transform;
                instance.SetActive(false);
                prefabFamilies[name].Add(instance);
            }
        }

    }

    //----------------------------  GetPrefab系　--------------------------//

    /// <summary>
    /// Gets the source prefab.
    /// Resources.Loadしたあとの一時保存に使いたいときなどに.
    /// NGUIだと独自プレファブ管理がどうもやりにくいための苦肉の策.
    /// </summary>
    /// <returns>
    /// The source prefab.(GameObject)
    /// </returns>
    /// <param name='prefabName'>
    /// Prefab name.
    /// </param>
    public GameObject GetSourcePrefab(string prefabPath) {
        string prefabName = GetPrefabName(prefabPath);
        if (!prefabFamilies.ContainsKey(prefabName)) {
            LoadPrefab(prefabPath);
        }
        return prefabFamilies[prefabName].sourcePrefab;
    }


    public GameObject GetPrefab(GameObject prefab) {
        Transform _transform = prefab.transform;
        string prefabName = prefab.name;
        return GetPrefab(prefabName, _transform.position, _transform.rotation);

    }

    public GameObject GetPrefab(string prefabPath) {
        Transform _transform = transform;
        string prefabName = GetPrefabName(prefabPath);
        return GetPrefab(prefabName, _transform.position, _transform.rotation);
    }

    public GameObject GetPrefab(string prefabPath, Transform _transform) {
        string prefabName = GetPrefabName(prefabPath);
        return GetPrefab(prefabName, _transform.position, _transform.rotation);
    }

    public GameObject GetPrefab(string prefabPath, Vector3 position, Quaternion rotation) {
        PrefabFamiliy familiy;
        GameObject prefab;
        string prefabName = GetPrefabName(prefabPath);

        if (!prefabFamilies.ContainsKey(prefabName)) {
            LoadPrefab(prefabName);
        }

        //対象を一時変数に代入.
        familiy = prefabFamilies[prefabName];
        //1.リリースしてあるInstanceがなければ新しくinstantiate.
        if (familiy.releasedAmount == 0) {
            prefab = (GameObject)Instantiate(familiy.sourcePrefab, position, rotation);
            prefab.name = prefabName;
            familiy.Add(prefab);
            return prefab;
        }


        //2.リリースしてあるのがあればそれを使う.
        if (familiy.releasedAmount > 0) {
            prefab = familiy.GetReleasedPrefab();
            prefab.transform.position = position;
            prefab.transform.rotation = rotation;
            prefab.SetActive(true);
            return prefab;
        }


        return null;
    }


    //----------------------------　Release　--------------------------//

    public bool ReleasePrefab(GameObject prefab) {
        return ReleasePrefab(prefab.name, prefab);
    }


    public bool ReleasePrefab(string prefabPath, GameObject prefab) {
        PrefabFamiliy family;
        string prefabName = GetPrefabName(prefabPath);

        if (!prefabFamilies.ContainsKey(prefabName)) {
            Debug.LogWarning("Source Prefab is Not loaded");
            return false;
        }

        //解放するとき自分の子になおす.
        prefab.transform.parent = transform;
        family = prefabFamilies[prefabName];

        return family.Release(prefab);
    }


    //----------------------------　Util　--------------------------//

    /// <summary>
    /// Gets the name of the prefab.
    /// パスからプレハブ名を取得する.　末尾ファイル名を取り出すだけ.
    /// </summary>
    private string GetPrefabName(string prefabPath) {
        string[] splited = prefabPath.Split("/"[0]);
        return splited[splited.Length - 1];
    }



    //----------------------------　Utillity Class　--------------------------//

    //プレファブファミリィクラス。 private.
    //コピー元prefabと、それに連なる子供たちを保存する.
    [System.Serializable]
    class PrefabFamiliy
    {
        //----------------------------メンバ群.--------------------------//
        public GameObject sourcePrefab;
        public List<GameObject> instancies = new List<GameObject>();
        public List<bool> isUsing = new List<bool>();
        public int releasedAmount;
        public List<int> releasedIndex = new List<int>();

        //----------------------------コンストラクタ群.--------------------------//
        public PrefabFamiliy() { }

        public PrefabFamiliy(string prefabPath) {
            sourcePrefab = (GameObject)Resources.Load(prefabPath);
        }

        public PrefabFamiliy(GameObject sourcePrefab) {
            this.sourcePrefab = sourcePrefab;
        }



        //----------------------------　メンバ群　--------------------------//
        public void Unload() {
            sourcePrefab = null;
            instancies.Clear();
            isUsing = null;
            return;
        }

        public void Add(GameObject instance) {

            instancies.Add(instance);
            isUsing.Add(true);
        }

        public bool Release(GameObject instance) {
            int index;

            if (instancies.Contains(instance) == false) {
                return false;
            }
            index = instancies.IndexOf(instance);

            releasedAmount++;
            releasedIndex.Add(index);
            isUsing[index] = false;

            instancies[index].SetActive(false);
            return true;
        }

        public GameObject GetReleasedPrefab() {
            int index = releasedIndex[0];
            GameObject target;

            target = instancies[index];
            target.SetActive(true);

            releasedAmount--;
            releasedIndex.RemoveAt(0);
            isUsing[index] = true;

            return target;
        }
    }

    //事前読み込みするファイルと量を設定.
    [Serializable]
    public class PreloadPrefabInfo
    {
        public GameObject prefab;
        public int amount;
    }
}