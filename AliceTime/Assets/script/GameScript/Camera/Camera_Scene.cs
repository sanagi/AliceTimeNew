using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Camera_Scene : MonoBehaviour {
	static private Camera_Scene _instance;
	static public Camera_Scene Instance {
		get {
			if(_instance == null){
				_instance = FindObjectOfType<Camera_Scene>();
			}
			return _instance;
		}
	}

	public List<GameObject> ObjectCacheList = new List<GameObject>();
	public bool ZoomNow;
	Vector3 ZoomtargetPos;
	Vector3 Origin;
	string target_1,target_2;
	float zoom_time;
	float zoom_offtime;

	public float z_param;
	public float y_param;

	public Vector3 LP;
	// Use this for initialization
	void Start () {
		ZoomtargetPos = Vector3.zero;
		Origin = Vector3.zero;
		ObjectCache();
		ZoomNow = false;
		GameObject P = GameObject.FindGameObjectWithTag("Player").gameObject;
		gameObject.transform.parent = P.transform;
		gameObject.transform.localPosition = LP;
	}

	void ObjectCache(){//ズームイベントのためのキャッシュ（目標地点をを探す）
		GameObject[] tmpObjectCache = GameObject.FindGameObjectsWithTag("EventObj");
		ObjectCacheList.AddRange(tmpObjectCache);
		GameObject tmpP = GameObject.FindGameObjectWithTag("Player");
		ObjectCacheList.Add(tmpP);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetParam(string t1,string t2,string ztime,string ztime2){
		target_1 = t1;
		target_2 = t2;
		zoom_time = float.Parse(ztime);
		zoom_offtime = float.Parse(ztime2);
	}

	public void ZoomOn(){
		ZoomtargetPos = Decision_target(target_1,target_2);
		Origin = gameObject.transform.position;
		iTweenZoom(ZoomtargetPos,zoom_time,"CompleteZoom");
		ZoomNow = true;
	}

	public void ZoomOff(){
		if(ZoomNow){
			iTweenZoom(Origin,zoom_offtime,"OffCompleteZoom");
			ZoomNow = false;
		}
	}

	void iTweenZoom(Vector3 tar,float time,string Handler){
		
	}



	Vector3 Decision_target(string target1,string target2){
		Vector3 target_A = Vector3.zero;
		Vector3 target_B = Vector3.zero;
		Vector3 Rt = Vector3.zero;
		for(int i = 0;i<ObjectCacheList.Count;i++){
			if(ObjectCacheList[i].name == target1){
				target_A = ObjectCacheList[i].transform.position;
				target1 = "set";
			}
			if(ObjectCacheList[i].name == target2){
				target_B = ObjectCacheList[i].transform.position;
				target2 = "set";
			}
		}

		//バグチェックだからここ消そうねー
		if(target1 != "set" || target2 != "set"){
			Debug.Log ("targetない");
		}
		Rt = (target_A+target_B)/2.0f;
		Rt.y += y_param;
		Rt.z -= z_param;
		Debug.Log (Rt);
		return Rt;
	}
}
