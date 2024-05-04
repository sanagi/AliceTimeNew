using UnityEngine;
using System.Collections;

public class OptionManager : SingletonMonoBehaviour<OptionManager> 
{
	public enum LanguageType {
		Japanese = 1,
		English = 2,
	}

	public enum FoldAnimationSkip {
		OFF = 0,
		ON = 1,
	}

    public enum MoveSpeed
    {
        Normal = 0,
        Quick = 1,
    }

    public SaveManager.Option options;

	protected override void Init() {
		base.Init();
		options = SaveManager.Instance.GetOptions();
	}

	protected override void Deinit() {
		base.Deinit();
	}

	public void Save() {
		SaveManager.Instance.SetOptions (options);
	}

	public float GetBGMVolume() {
		return options.bgmVolume;
	}

	public float GetSEVolume() {
		return options.seVolume;
	}

	public LanguageType GetLanguateType() {
		return options.languageType;
	}

	public float GetControllerPositionRate() {
		return options.controllerPositionRate;
	}

	public float GetControllerSizeRate() {
		return options.controllerSizeRate;
	}

	public float GetTextSpeed() {
		return options.textSpeed;
	}

	public FoldAnimationSkip GetFoldAnimationSkip() {
		return options.foldAnimationSKip;
	}

    public MoveSpeed GetMoveSpeed()
    {
        return options.moveSpeed;
    }

    public void ChangeBGMVolume(float value) {
		options.bgmVolume = value;
		//Audio_Manage.ChangeVolume_BGM(value);
	}

	public void ChangeSEVolume(float value) {
		options.seVolume = value;
		//Audio_Manage.ChangeVolume_SE(value);
	}

	public void ChangeLanguage(LanguageType languageType) {
		options.languageType = languageType;
	}

	public void ChangeControllerPositionRate(float value) {
		options.controllerPositionRate = value;
	}

	public void ChangeControllerSizeRate(float value) {
		options.controllerSizeRate = value;
	}

	public void ChangeTextSpeed(float value) {
		options.textSpeed = value;
	}

	public void ChangeFoldAnimationSkip(FoldAnimationSkip value) {
		options.foldAnimationSKip = value;
	}

    public void ChangeMoveSpeed(MoveSpeed value)
    {
        options.moveSpeed = value;
    }
}
