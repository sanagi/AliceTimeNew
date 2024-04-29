using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AlphaAnimation : MonoBehaviour {
	private float animationTime = 0.3f;	
	public bool animEnd = false;

	private Image img = null;
	[SerializeField]
	private RawImage rawImg = null;
	private TitlePanelBehaviour titlePanel;
	private bool anim = false;
	private float alpha = 0f;
	[SerializeField]
	private Text num;

	[SerializeField]
	private AlphaAnimation nextAlpha = null;
	[SerializeField]
	private bool isTrialStage;

	private float beginTime = 0;
	private float endTime = 0;
	private float nowTime = 0;

	void Start(){
		if(!isTrialStage){
			img = gameObject.GetComponent<Image>();
		}
		else{
			if(rawImg == null){
				rawImg = gameObject.GetComponent<RawImage>();
			}
			if(num == null){
				num = gameObject.transform.Find("num").GetComponent<Text>();
			}
		}
		titlePanel = gameObject.transform.parent.GetComponent<TitlePanelBehaviour>();
	}

	// Update is called once per frame
	void Update () {
		if(anim){
			Color col = Color.white;
			//if(!titlePanel.isAnimation){
			if(nowTime > animationTime){
				col = new Color(1.0f,1.0f,1.0f,1.0f);
				anim = false;
				animEnd = true;
			}
			else{
				alpha = Mathf.InverseLerp(beginTime,endTime,Time.fixedTime);
				col = new Color(1.0f,1.0f,1.0f,alpha);
				nowTime += Time.deltaTime;
				if(nowTime*1.5f > animationTime){
					
				}
			}
			//}
			ColorSet(col);
		}
	}

	private void ColorSet(Color c){
		if(img != null){
			img.color = c;
		}
		else if(rawImg != null){
			rawImg.color = c;
			num.color = new Color(num.color.r,num.color.g,num.color.b,c.a);
		}
	}

	/// <summary>
	/// Animations the start.
	/// </summary>
	public void AnimationStart(){
		ColorSet(new Color(1.0f,1.0f,1.0f,0.0f));
		alpha = 0f;
		anim = true;

		beginTime = Time.time;
		endTime = Time.time + animationTime;
		gameObject.SetActive(true);
	}

	public float GetEndTime(){
		return animationTime;
	}
}
