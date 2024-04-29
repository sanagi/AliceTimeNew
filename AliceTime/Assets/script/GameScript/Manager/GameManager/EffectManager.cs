using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum ParticleEnum{
	TITLE,
	MAIN,
	RESULT,
	
	MAKE_LIFT,
	MAKE_BACK,
	MAKE_BLOCK,
	DELETE_LIFT,
	DELETE_BLOCK,
    BOM_FIRE,
	FIRE_IMPACT,
    FIREBALL,
	AIBOU_POWER,
	AIBOU_RELEASE,
	AIBOU_LINE,
    AIBOU_LINE2,
    AIBOU_CANCEL,
	ORIGAMI_SHOW,
	ORIGAMI_OUT,
	OPENTITLE
}

//ワンショットのパーティクル

public class EffectManager : MonoBehaviour {

	//List<ParticleSystem> ParticleList = new List<ParticleSystem>();

	static private EffectManager _instance;
	static public EffectManager Instance {
		get {
			if(_instance == null){
				_instance = FindObjectOfType<EffectManager>();
				
				if(_instance == null){
					_instance = new GameObject(typeof(EffectManager).Name).AddComponent<EffectManager>();
				}
			}
			return _instance;
		}
	}

	public class ParticleData{
		public string path;

		public ParticleData(string path){
			this.path = path;
		}
	}

	GameObject Sakura;

	Dictionary<ParticleEnum,ParticleData> ParticleTable = new Dictionary<ParticleEnum, ParticleData>(){
		{ParticleEnum.TITLE,new ParticleData("Sakura")},
		{ParticleEnum.MAIN,new ParticleData("Sakura_main")},
		{ParticleEnum.RESULT,new ParticleData("Sakura_Result")},
		{ParticleEnum.MAKE_LIFT,new ParticleData("Hanko_efect")},
		{ParticleEnum.MAKE_BACK,new ParticleData("Sumi_Center_Pre")},
		{ParticleEnum.DELETE_LIFT,new ParticleData("Sumi_Center_after")},
		{ParticleEnum.MAKE_BLOCK,new ParticleData("Hanko_efect_black")},
		{ParticleEnum.DELETE_BLOCK,new ParticleData("Sumi_Center_after_black")},
        {ParticleEnum.BOM_FIRE,new ParticleData("BomParticle")},
		{ParticleEnum.FIRE_IMPACT,new ParticleData("FireImpact")},
        {ParticleEnum.FIREBALL,new ParticleData("FireBall")},
        {ParticleEnum.AIBOU_POWER,new ParticleData("Aibou_Power")},
		{ParticleEnum.AIBOU_RELEASE,new ParticleData("Aibou_Release")},
		{ParticleEnum.AIBOU_LINE,new ParticleData("Aibou_Line")},
        {ParticleEnum.AIBOU_LINE2,new ParticleData("Aibou_Line_1")},
        {ParticleEnum.AIBOU_CANCEL,new ParticleData("Aibou_Cancel")},
		{ParticleEnum.ORIGAMI_SHOW,new ParticleData("OrigamiShow")},
		{ParticleEnum.ORIGAMI_OUT,new ParticleData("OrigamiOut")},
		{ParticleEnum.OPENTITLE,new ParticleData("TitleOpenParticle")}
	};

	public GameObject Play(ParticleEnum particle,Vector3 s_position){
		GameObject par = Resources.Load("Particles/" + ParticleTable[particle].path)as GameObject;
		//Quaternion trot = par.transform.rotation;
		Vector3 source_position = s_position;
		source_position.z = -6.0f;

		GameObject g = Instantiate(par,source_position,par.transform.rotation)as GameObject;
		if(particle == ParticleEnum.TITLE){
			Sakura = Instantiate(par,s_position,Quaternion.identity)as GameObject;
			Sakura.transform.rotation = Quaternion.Euler(180f,0f,0f);
		}
        return g;
	}
}
