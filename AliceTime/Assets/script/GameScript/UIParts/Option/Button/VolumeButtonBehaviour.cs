using UnityEngine;
using UnityEngine.UI;
using KamioriInput;
using System.Collections;

public class VolumeButtonBehaviour : MonoBehaviour, IButtonEvent
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
    private Stick stick;
    private Jump jump;

    private float offTimer = 0f;
    private const float showLimit = 3.750f;

    void Start()
    {
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        buttonImage = gameObject.GetComponent<Image>();
        VirtualCanvas = VirtualControllerEvent.GetVirtual();
        jump = VirtualCanvas.transform.Find("controller").gameObject.transform.Find("Jump").gameObject.transform.Find("button").gameObject.GetComponent<Jump>();
        stick = VirtualCanvas.transform.Find("controller").gameObject.transform.Find("Stick").gameObject.transform.Find("button").gameObject.GetComponent<Stick>();
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
                    if (VirtualCanvas == null)
                    {
                        VirtualCanvas = VirtualControllerEvent.GetVirtual();
                    }
                    if (VirtualCanvas != null)
                    {
                        VirtualCanvas.SetActive(false);
                    }
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

        if (VirtualCanvas == null)
        {
            VirtualCanvas = VirtualControllerEvent.GetVirtual();
        }
        if (VirtualCanvas != null)
        {
            VirtualCanvas.SetActive(false);
        }
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
            if (VirtualCanvas == null)
            {
                VirtualCanvas = VirtualControllerEvent.GetVirtual();
            }
            VirtualCanvas.SetActive(true);
            if (stick == null)
            {
                stick = VirtualCanvas.transform.Find("controller").gameObject.transform.Find("Stick").gameObject.transform.Find("button").gameObject.GetComponent<Stick>();
            }
            if (stick != null)
            {
                stick.SetScaleSize();
            }
            if (jump == null)
            {
                jump = VirtualCanvas.transform.Find("controller").gameObject.transform.Find("Jump").gameObject.transform.Find("button").gameObject.GetComponent<Jump>();
            }
            if (jump != null)
            {
                jump.SetScaleSize();
            }
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
            if (VirtualCanvas == null)
            {
                VirtualCanvas = VirtualControllerEvent.GetVirtual();
            }
            if (stick == null)
            {
                stick = VirtualCanvas.transform.Find("controller").gameObject.transform.Find("Stick").gameObject.transform.Find("button").gameObject.GetComponent<Stick>();
            }
            if (stick != null)
            {
                stick.SetScaleSize();
            }
            if (jump == null)
            {
                jump = VirtualCanvas.transform.Find("controller").gameObject.transform.Find("Jump").gameObject.transform.Find("button").gameObject.GetComponent<Jump>();
            }
            if (jump != null)
            {
                jump.SetScaleSize();
            }
        }
        else if (volumeType == VolumeType.TEXT_SPEED)
        {
            OptionManager.Instance.ChangeTextSpeed(defaultValue);
        }
    }

    #region IButtonEvent implementation
    public void FireButtonEvent()
    {
        if (buttonImage == null)
        {
            buttonImage = gameObject.GetComponent<Image>();
        }
        buttonImage.color = new Color(0.55f, 0.55f, 0.55f, 1.0f);
    }
    public void ReleaseButtonEvent()
    {
        if (volumeType == VolumeType.CONTROL_SIZE_RATE || volumeType == VolumeType.CONTROL_POS_RATE)
        {
            if (VirtualCanvas == null)
            {
                VirtualCanvas = VirtualControllerEvent.GetVirtual();
            }
            if (VirtualCanvas != null)
            {
                VirtualCanvas.SetActive(false);
            }
        }
    }
    public void ReleaseOutButtonEvent()
    {
    }
    #endregion
}
