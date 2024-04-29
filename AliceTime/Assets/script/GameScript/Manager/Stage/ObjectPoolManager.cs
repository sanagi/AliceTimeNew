using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPoolManager : MonoBehaviour {

	private List<GameObject> pooledObjects;

	private int maxPooledValue = 10;

	private bool willGrow = true;

	private string prefabPath = "";

	public void Setup(string prefabPath, int maxPooledValue = 10, bool willGrow = true) {
		this.prefabPath = prefabPath;
		this.pooledObjects = new List<GameObject>();
		this.maxPooledValue = maxPooledValue;
		this.willGrow = willGrow;

		for(int i = 0; i < maxPooledValue; i++){
			GameObject poolObject = Instantiate(PrefabPoolManager.Instance.GetSourcePrefab(prefabPath), Vector3.zero, Quaternion.identity) as GameObject;
			poolObject.SetActive(false);
			pooledObjects.Add(poolObject);
		}
	}

	/// <summary>
	/// ActiveなObjectを返す関数
	/// </summary>
	/// <returns>ActiveなGameObject</returns>
	public GameObject GetPooledObject(){
		for(int i = 0; i < pooledObjects.Count; i++){
			if(!pooledObjects[i].activeSelf){
				return pooledObjects[i];
			}
		}

		if(willGrow){
			GameObject poolObject = Instantiate(PrefabPoolManager.Instance.GetSourcePrefab(prefabPath), Vector3.zero, Quaternion.identity) as GameObject;
			pooledObjects.Add(poolObject);
			return poolObject;
		}
		return null;
	}
}