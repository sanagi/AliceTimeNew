using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VolumeButtonBehaviour : MonoBehaviour
{
    public enum VolumeType
    {
        BGM, SE, CONTROL_POS_RATE, CONTROL_SIZE_RATE, TEXT_SPEED
    }

    public float defaultValue = 0f;
    public float preValue = 0f;
    public VolumeType volumeType;
    public Slider slider;
    public Image buttonImage;

    private GameObject VirtualCanvas;

    private float offTimer = 0f;
    private const float showLimit = 3.750f;

    void Start()
    {
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        buttonImage = gameObject.GetComponent<Image>();
    }

    void Update()
    {
        if (volumeType == VolumeType.CONTROL_POS_RATE || volumeType == VolumeType.CONTROL_SIZE_RATE)
        {
            if (preValue != defaultValue)
            {
                offTimer = 0f;
            }
            else
            {
                offTimer += Time.deltaTime;
                if (offTimer > showLimit)
                {
                    offTimer = 0f;
                }
            }
        }
        preValue = defaultValue;
    }

    void OnEnable()
    {
        if (volumeType == VolumeType.BGM)
        {
            defaultValue = OptionManager.Instance.GetBGMVolume();
        }
        else if (volumeType == VolumeType.SE)
        {
            defaultValue = OptionManager.Instance.GetSEVolume();
        }
        else if (volumeType == VolumeType.CONTROL_POS_RATE)
        {
            defaultValue = OptionManager.Instance.GetControllerPositionRate();
        }
        else if (volumeType == VolumeType.CONTROL_SIZE_RATE)
        {
            defaultValue = OptionManager.Instance.GetControllerSizeRate();
        }
        else if (volumeType == VolumeType.TEXT_SPEED)
        {
            defaultValue = OptionManager.Instance.GetTextSpeed();
        }
        slider.value = defaultValue;
    }

    void OnDisable()
    {
        var options = SaveManager.Instance.GetOptions();
        if (volumeType == VolumeType.BGM)
        {
            options.bgmVolume = defaultValue;
        }
        else if (volumeType == VolumeType.SE)
        {
            options.seVolume = defaultValue;
        }
        else if (volumeType == VolumeType.CONTROL_POS_RATE)
        {
            options.controllerPositionRate = defaultValue;
        }
        else if (volumeType == VolumeType.CONTROL_SIZE_RATE)
        {
            options.controllerSizeRate = defaultValue;
        }
        else if (volumeType == VolumeType.TEXT_SPEED)
        {
            options.textSpeed = defaultValue;
        }
        SaveManager.Instance.SetOptions(options);
    }

    public void ValueChangeCheck()
    {
        defaultValue = slider.value;
        if (volumeType == VolumeType.BGM)
        {
            OptionManager.Instance.ChangeBGMVolume(defaultValue);
        }
        else if (volumeType == VolumeType.SE)
        {
            OptionManager.Instance.ChangeSEVolume(defaultValue);
        }
        else if (volumeType == VolumeType.CONTROL_POS_RATE)
        {
            OptionManager.Instance.ChangeControllerPositionRate(defaultValue);
            VirtualCanvas.SetActive(true);
        }
        else if (volumeType == VolumeType.CONTROL_SIZE_RATE)
        {
            var value = defaultValue;
            if (defaultValue < 0.1f)
            {
                value = 0.1f;
            }
            else if (defaultValue > 0.9f)
            {
                value = 0.9f;
            }
            OptionManager.Instance.ChangeControllerSizeRate(value);

            VirtualCanvas.SetActive(true);
        }
        else if (volumeType == VolumeType.TEXT_SPEED)
        {
            OptionManager.Instance.ChangeTextSpeed(defaultValue);
        }
    }
}
