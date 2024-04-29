using UnityEngine;
using System.Collections;
using System;

public class TitleOptionSelectPanel : MonoBehaviour {

	public GameObject UpArrow;
	public GameObject DownArrow;

	private OptionGroup[] optionGroups;

	private int minPage = 0;
	private int maxPage = 1;
	public int currentPage;

    public void Awake()
	{
		optionGroups = transform.GetComponentsInChildren<OptionGroup>();
		currentPage = 0;
		SwitchPage ();
    }

	private void SwitchPage()
	{
		for (var i = minPage; i <= maxPage; i++)
		{
			var optionGroup = optionGroups [i];
			if (i == currentPage) {
				optionGroup.gameObject.SetActive (true);
			} else {
				optionGroup.gameObject.SetActive (false);
			}
		}

		SwitchPageButton ();
	}

	private void SwitchPageButton()
	{
		// Up Arrow
		if (currentPage <= minPage) {
			UpArrow.SetActive (false);	
		} else {
			UpArrow.SetActive (true);
		}

		// Down Arrow
		if (currentPage >= maxPage) {
			DownArrow.SetActive (false);
		} else {
			DownArrow.SetActive (true);
		}
	}

	public void PrevPage()
	{
		//Audio_Manage.Play(SoundEnum.SE_TouchBegan);

		if (currentPage <= minPage) {
			return;
		}

		HidePage (currentPage--, Vector3.right * 100f, Vector3.right * 800f, null);
		ShowPage (currentPage, Vector3.right * -700f, Vector3.zero, null);

		SwitchPage ();
	}

	public void NextPage()
	{

		//Audio_Manage.Play(SoundEnum.SE_TouchBegan);

		if (currentPage >= maxPage) {
			return;
		}

		HidePage (currentPage++, Vector3.right * 100f, Vector3.right * -600f, null);
		ShowPage (currentPage, Vector3.right * 700f, Vector3.zero, null);

		SwitchPage ();
	}



	private void ShowPage(int page, Vector3 fromPos, Vector3 toPos, Action callback=null)
	{
		var optionGroup = optionGroups[page];
		optionGroup.transform.localPosition = fromPos;

		UIAnimationUtil.Move(optionGroup.gameObject, fromPos, toPos, 0.2f, () =>
			{
				if (callback != null)
				{
					callback();				
				}
			});
	}

	private void HidePage(int page, Vector3 fromPos, Vector3 toPos, Action callback=null)
	{
		var optionGroup = optionGroups[page];
		optionGroup.transform.localPosition = fromPos;

		UIAnimationUtil.Move(optionGroup.gameObject, fromPos, toPos, 0.2f, () =>
			{
				if (callback != null)
				{
					callback();
				}
			});
	}
}
