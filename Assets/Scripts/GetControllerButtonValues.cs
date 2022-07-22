using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Events;

public class GetControllerButtonValues : MonoBehaviour
{
    [SerializeField] private InputDeviceCharacteristics controllerCharacteristics;
    private InputDevice targetDevice;

    [HideInInspector] public bool primaryButtonValue;
    [HideInInspector] public bool secondaryButtonValue;
    [HideInInspector] public float triggerValue;
    [HideInInspector] public float gripValue;
    [HideInInspector] public bool triggerButtonValue;
    [HideInInspector] public bool gripButtonValue;
    [HideInInspector] public Vector2 joystickValue;
    [HideInInspector] public bool joystickClickValue;

    public UnityEvent primaryButtonPressEvent;
    public UnityEvent primaryButtonReleaseEvent;
    private bool canSendPrimaryEvent = true;
    public UnityEvent secondaryButtonPressEvent;
    public UnityEvent secondaryButtonReleaseEvent;
    private bool canSendSecondaryEvent = true;

    public UnityEvent triggerButtonPressEvent;
    public UnityEvent triggerButtonReleaseEvent;
    private bool canSendTriggerEvent = true;
    public UnityEvent gripButtonPressEvent;
    public UnityEvent gripButtonReleaseEvent;
    private bool canSendGripEvent = true;

    public UnityEvent joystickPressEvent;
    public UnityEvent joystickReleaseEvent;
    private bool canSendJoystickEvent = true;

    // Start is called before the first frame update
    void Start()
    {
        GetControllerDevice();
    }

    private void GetControllerDevice()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        if (devices.Count > 0)
            targetDevice = devices[0];
        else
            Debug.Log("No controller device detected");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateValues();
        CheckToSendEvents();
    }

    private void UpdateValues()
    {
        //Face buttons
        if (targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool valuePrimary))
            primaryButtonValue = valuePrimary;
        else
            primaryButtonValue = false;

        if (targetDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool valueSecondary))
            secondaryButtonValue = valueSecondary;
        else
            secondaryButtonValue = false;

        //Trigger and Grip
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float valueTrig))
            triggerValue = valueTrig;
        else
            triggerValue = 0;

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float valueGrip))
            gripValue = valueGrip;
        else
            gripValue = 0;

        if (targetDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool valueTrigButton))
            triggerButtonValue = valueTrigButton;
        else
            triggerButtonValue = false;

        if (targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool valueGripButton))
            gripButtonValue = valueGripButton;
        else
            triggerButtonValue = false;

        //Joystick
        if (targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 axisVector2D))
            joystickValue = axisVector2D;
        else
            joystickValue = new Vector2(0,0);

        if (targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool axisClick2D))
            joystickClickValue = axisClick2D;
        else
            joystickClickValue = false;

    }

    private void CheckToSendEvents()
    {
        //Face buttons
        if (primaryButtonValue && canSendPrimaryEvent)
        {
            primaryButtonPressEvent?.Invoke();
            canSendPrimaryEvent = false;
        }
        else if (!primaryButtonValue && !canSendPrimaryEvent)
        {
            primaryButtonReleaseEvent?.Invoke();
            canSendPrimaryEvent = true;
        }

        if (secondaryButtonValue && canSendSecondaryEvent)
        {
            secondaryButtonPressEvent?.Invoke();
            canSendSecondaryEvent = false;
        }
        else if (!secondaryButtonValue && !canSendSecondaryEvent)
        {
            secondaryButtonReleaseEvent?.Invoke();
            canSendSecondaryEvent = true;
        }

        //Trigger and Grip
        if(triggerButtonValue && canSendTriggerEvent)
        {
            triggerButtonPressEvent?.Invoke();
            canSendTriggerEvent = false;
        }
        else if (!triggerButtonValue && !canSendTriggerEvent)
        {
            triggerButtonReleaseEvent?.Invoke();
            canSendTriggerEvent = true;
        }

        if (gripButtonValue && canSendGripEvent)
        {
            gripButtonPressEvent?.Invoke();
            canSendGripEvent = false;
        }
        else if (!gripButtonValue && !canSendGripEvent)
        {
            gripButtonReleaseEvent?.Invoke();
            canSendGripEvent = true;
        }

        //Joystick
        if (joystickClickValue && canSendJoystickEvent)
        {
            joystickPressEvent?.Invoke();
            canSendJoystickEvent = false;
        }
        else if (!joystickClickValue && !canSendJoystickEvent)
        {
            joystickReleaseEvent?.Invoke();
            canSendJoystickEvent = true;
        }
    }
}
