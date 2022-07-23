using System.Collections;
using UnityEngine;

//TODO

//PACKAGING
    //Write summaries for all public functions
    //Thorough commenting of every line of code

//BACKLOG
    //Angular speed: makes it so it can calculate the angular speed from the transform
    //Add Velocity: pass data based on each individual velocity vector axis 
    //Rotation: make it so each axis can act as an endless encoder

/// <summary>
/// Provides general methods to pass transform and rigidbody behavior data from Unity to Csound
/// </summary>
public class CsoundTransformAndPhysicsSender : MonoBehaviour
{
    [Header("REFERENCES")]
    [Tooltip("Reference to the CsoundUnity component to send values to. Will automatically get the component attached to the same object if left empty.")]
    public CsoundUnity csoundUnity;
    [Tooltip("Assign this field if you want to take the physics/transform data from another game object. Leave blank to use this same object for the physics/transform data.")]
    public GameObject referenceObject;
    private Rigidbody rigidbody;
    [Space]
    [Header("TRANSFORM")]
    public CsoundPosition PositionSender;
    [Space]
    public CsoundRotation RotationSender;
    [Space]
    public CsoundScaleAxis ScaleAxisSender;
    [Space]
    public CsoundScaleMagnitude ScaleMagnitudeSender;
    [Header("PHYSICS")]
    public CsoundSpeed SpeedSender;
    [Space]
    public CsoundAngularSpeed AngularSpeedSender;


    #region UNITY LIFE CYCLE
    private void Awake()
    {
        if (referenceObject == null)
            referenceObject = gameObject;

        //Gets Rigidbody attached to object
        if (rigidbody == null)
        {
            rigidbody = referenceObject.GetComponent<Rigidbody>();

            if (rigidbody == null)
                Debug.LogError("No Rigidbody component attached to " + referenceObject.name);
        }

        //Gets the CsoundUnity component attached to the object.
        if (csoundUnity == null)
        {
            csoundUnity = GetComponent<CsoundUnity>();

            if (csoundUnity == null)
                Debug.LogError("No CsoundUnity component attached to " + gameObject.name);
        }

        //Gets reference to the main camera object
        PositionSender.camera = Camera.main.transform;
    }

    void Start()
    {
        StartCoroutine(Initialization());
    }

    private IEnumerator Initialization()
    {
        while (!csoundUnity.IsInitialized)
        {
            Debug.Log("CSOUND NOT INITIALIZED");
            yield return null;
        }

        Debug.Log("CSOUND INITIALIZED");

        //Send values to Csound based on object speed if updateSpeedOnStart is true.
        if (SpeedSender.updateSpeedOnStart)
            UpdateSpeed(true);

        if (PositionSender.updatePositionOnStart)
            UpdatePosition(true);

        if (AngularSpeedSender.updateAngularSpeedOnStart)
            UpdateAngularSpeed(true);

        if (ScaleMagnitudeSender.updateScaleMagnitudeOnStart)
            UpdateScaleMagnitude(true);

        if (ScaleAxisSender.updateScaleOnStart)
            UpdateScaleAxis(true);

        if (RotationSender.updateRotationOnStart)
            UpdateRotation(true);
    }

    void FixedUpdate()
    {
        if (!csoundUnity.IsInitialized) return;

        //If the updateSpeed bool is true, calculate speed and send values to Csound.
        if (SpeedSender.updateSpeed && SpeedSender.speedSource != CsoundSpeed.SpeedSource.None)
            SendCsoundDataBasedOnSpeed();

        if (AngularSpeedSender.updateAngularSpeed && AngularSpeedSender.angularSpeedSource != CsoundAngularSpeed.AngularSpeedSource.None)
            SendCsoundDataBasedOnAngularSpeed();
    }

    private void LateUpdate()
    {
        if (!csoundUnity.IsInitialized) return;

        if (PositionSender.updatePosition)
        {
            if (PositionSender.calculateRelativePos)
                CaculateRelativePos();

            if (PositionSender.calculateRelativeCameraPos)
                CalculateRelativeCameraPos();

            if (PositionSender.csoundChannelsPosX != null && PositionSender.setXPositionTo != CsoundPosition.PositionVectorReference.None)
                SetCsoundValuesPosX();
            if (PositionSender.csoundChannelsPosY != null && PositionSender.setYPositionTo != CsoundPosition.PositionVectorReference.None)
                SetCsoundValuesPosY();
            if (PositionSender.csoundChannelsPosZ != null && PositionSender.setZPositionTo != CsoundPosition.PositionVectorReference.None)
                SetCsoundValuesPosZ();
        }

        if (RotationSender.updateRotation)
        {
            CalculateRelativeRotation();

            if (RotationSender.csoundChannelsRotationX != null && RotationSender.setXRotationTo != CsoundRotation.RotationVectorReference.None)
                SetCsoundValuesXRotation();

            if (RotationSender.csoundChannelsRotationY != null && RotationSender.setYRotationTo != CsoundRotation.RotationVectorReference.None)
                SetCsoundValuesYRotation();

            if (RotationSender.csoundChannelsRotationZ != null && RotationSender.setZRotationTo != CsoundRotation.RotationVectorReference.None)
                SetCsoundValuesZRotation();
        }

        if (ScaleAxisSender.updateScaleAxis)
        {
            if (ScaleAxisSender.calculateRelativeScale)
                CalculateRelativeScale();

            if (ScaleAxisSender.csoundChannelsScaleX != null && ScaleAxisSender.setXScaleTo != CsoundScaleAxis.ScaleVectorReference.None)
                SetCsoundValuesScaleX();
            if (ScaleAxisSender.csoundChannelsScaleY != null && ScaleAxisSender.setYScaleTo != CsoundScaleAxis.ScaleVectorReference.None)
                SetCsoundValuesScaleY();
            if (ScaleAxisSender.csoundChannelsScaleZ != null && ScaleAxisSender.setZScaleTo != CsoundScaleAxis.ScaleVectorReference.None)
                SetCsoundValuesScaleZ();
        }

        if (ScaleMagnitudeSender.updateScaleMagnitude && ScaleMagnitudeSender.setScaleMagnitudeTo != CsoundScaleMagnitude.ScaleMagnitudeVectorReference.None)
            SendCsoundDataBasedOnScaleMagnitude();
    }
    #endregion

    #region SPEED
    private void SendCsoundDataBasedOnSpeed()
    {
        //Checks if speed should be calculated from the objects Rigidbody or from the Transform. 
        if (SpeedSender.speedSource == CsoundSpeed.SpeedSource.Rigidbody)
        {
            //Gets speed from the Rigidbody's velocity.
            SpeedSender.speed = rigidbody.velocity.magnitude;
        }
        else if (SpeedSender.speedSource == CsoundSpeed.SpeedSource.Transform)
        {
            //Calculates speed based on the transform
            SpeedSender.speed = (referenceObject.transform.position - SpeedSender.previousPosSpeed).magnitude / Time.deltaTime;
            SpeedSender.previousPosSpeed = referenceObject.transform.position;
        }

        //Assign values to Csound channels based on the object's speed.
        foreach (CsoundChannelDataSO.CsoundChannelData channelData in SpeedSender.speedChannelData.channelData)
        {
            //Scales the value passed to Csound based on the minValue and maxValue defined for each channel.
            float scaledSpeedValue =
                Mathf.Clamp(ScaleFloat(0, SpeedSender.maxSpeedValue, channelData.minValue, channelData.maxValue, SpeedSender.speed), channelData.minValue, channelData.maxValue);
            //Passes values to Csound.
            csoundUnity.SetChannel(channelData.name, scaledSpeedValue);
        }

        if (SpeedSender.debugSpeed)
            Debug.Log(SpeedSender.speed);
    }

    /// <summary>
    /// Starts calcualting the object speed and passing that value into the defined Csound if the bool is true and stops it if false.
    /// </summary>
    /// <param name="update"></param>
    public void UpdateSpeed(bool update)
    {
        SpeedSender.updateSpeed = update;

        if (SpeedSender.debugSpeed)
            Debug.Log("CSOUND " + gameObject.name + " update speed = " + SpeedSender.updateSpeed);
    }

    /// <summary>
    /// Toggles the update speed bool between true and false. Starts calcualting the object speed and passing that value into the defined Csound if the bool is true and stops it if false.
    /// </summary>
    public void UpdateSpeedToggle()
    {

        if (SpeedSender.updateSpeed)
            SpeedSender.updateSpeed = false;
        else if (!SpeedSender.updateSpeed)
            SpeedSender.updateSpeed = true;

        if (SpeedSender.debugSpeed)
            Debug.Log("CSOUND " + referenceObject.name + " update speed = " + SpeedSender.updateSpeed);
    }
    #endregion

    #region POSITION

    public void UpdatePosition(bool update)
    {
        PositionSender.updatePosition = update;

        if (update)
            GetRelativeStartingPosition();

        if (PositionSender.setXPositionTo == CsoundPosition.PositionVectorReference.RelativeToCamera ||
            PositionSender.setYPositionTo == CsoundPosition.PositionVectorReference.RelativeToCamera ||
            PositionSender.setZPositionTo == CsoundPosition.PositionVectorReference.RelativeToCamera)
        {
            PositionSender.calculateRelativeCameraPos = true;

        }

        if (PositionSender.setXPositionTo == CsoundPosition.PositionVectorReference.Relative ||
        PositionSender.setYPositionTo == CsoundPosition.PositionVectorReference.Relative ||
        PositionSender.setZPositionTo == CsoundPosition.PositionVectorReference.Relative)
        {
            PositionSender.calculateRelativePos = true;
        }

        if (PositionSender.debugPosition)
            Debug.Log("CSOUND " + gameObject.name + " update position = " + PositionSender.updatePosition);
    }

    public void UpdatePositionToggle()
    {
        if (PositionSender.updatePosition)
            PositionSender.updatePosition = false;
        else
            PositionSender.updatePosition = true;

        UpdatePosition(PositionSender.updatePosition);

    }

    private void GetRelativeStartingPosition()
    {
        PositionSender.startPos = referenceObject.transform.position;

        if (PositionSender.calculateRelativeCameraPos)
            PositionSender.startPosCameraRelative = PositionSender.camera.transform.InverseTransformPoint(referenceObject.transform.position);
    }

    private void CaculateRelativePos()
    {
        PositionSender.relativePos = referenceObject.transform.position - PositionSender.startPos;

        if (PositionSender.debugPosition)
            Debug.Log("CSOUND " + referenceObject.name + " relative position: " + PositionSender.relativePos);
    }

    private void CalculateRelativeCameraPos()
    {
        Vector3 currentTransform = PositionSender.camera.transform.InverseTransformPoint(transform.position);

        PositionSender.relativeCameraPos = currentTransform - PositionSender.startPosCameraRelative;

        if (PositionSender.debugPosition)
            Debug.Log("CSOUND " + referenceObject.name + " relative position: " + PositionSender.relativeCameraPos);
    }


    private void SetCsoundValuesPosX()
    {
        if (PositionSender.setXPositionTo == CsoundPosition.PositionVectorReference.Absolute)
            SetCsoundChannelBasedOnAxis(PositionSender.csoundChannelsPosX, PositionSender.posVectorRangesMin.x, PositionSender.posVectorRangesMax.x, transform.position.x, PositionSender.returnAbsoluteValuesPosX);
        else if (PositionSender.setXPositionTo == CsoundPosition.PositionVectorReference.RelativeToCamera)
            SetCsoundChannelBasedOnAxis(PositionSender.csoundChannelsPosX, PositionSender.posVectorRangesMin.x, PositionSender.posVectorRangesMax.x, PositionSender.relativeCameraPos.x, PositionSender.returnAbsoluteValuesPosX);
        else
            SetCsoundChannelBasedOnAxis(PositionSender.csoundChannelsPosX, PositionSender.posVectorRangesMin.x, PositionSender.posVectorRangesMax.x, PositionSender.relativePos.x, PositionSender.returnAbsoluteValuesPosX);
    }

    private void SetCsoundValuesPosY()
    {
        if (PositionSender.setYPositionTo == CsoundPosition.PositionVectorReference.Absolute)
            SetCsoundChannelBasedOnAxis(PositionSender.csoundChannelsPosY, PositionSender.posVectorRangesMin.y, PositionSender.posVectorRangesMax.y, transform.position.y, PositionSender.returnAbsoluteValuesPosY);
        else if (PositionSender.setXPositionTo == CsoundPosition.PositionVectorReference.RelativeToCamera)
            SetCsoundChannelBasedOnAxis(PositionSender.csoundChannelsPosY, PositionSender.posVectorRangesMin.y, PositionSender.posVectorRangesMax.y, PositionSender.relativeCameraPos.y, PositionSender.returnAbsoluteValuesPosY);
        else
            SetCsoundChannelBasedOnAxis(PositionSender.csoundChannelsPosY, PositionSender.posVectorRangesMin.y, PositionSender.posVectorRangesMax.y, PositionSender.relativePos.y, PositionSender.returnAbsoluteValuesPosY);
    }

    private void SetCsoundValuesPosZ()
    {
        if (PositionSender.setZPositionTo == CsoundPosition.PositionVectorReference.Absolute)
            SetCsoundChannelBasedOnAxis(PositionSender.csoundChannelsPosZ, PositionSender.posVectorRangesMin.z, PositionSender.posVectorRangesMax.z, transform.position.z, PositionSender.returnAbsoluteValuesPosZ);
        else if (PositionSender.setZPositionTo == CsoundPosition.PositionVectorReference.RelativeToCamera)
            SetCsoundChannelBasedOnAxis(PositionSender.csoundChannelsPosZ, PositionSender.posVectorRangesMin.z, PositionSender.posVectorRangesMax.z, PositionSender.relativeCameraPos.z, PositionSender.returnAbsoluteValuesPosZ);
        else
            SetCsoundChannelBasedOnAxis(PositionSender.csoundChannelsPosZ, PositionSender.posVectorRangesMin.z, PositionSender.posVectorRangesMax.z, PositionSender.relativePos.z, PositionSender.returnAbsoluteValuesPosZ);
    }
    #endregion

    #region ANGULAR SPEED/TORQUE

    public void UpdateAngularSpeed(bool update)
    {
        AngularSpeedSender.updateAngularSpeed = update;

        if (AngularSpeedSender.debugAngularSpeed)
            Debug.Log("CSOUND " + gameObject.name + " update andgular speed = " + AngularSpeedSender.updateAngularSpeed);
    }

    public void UpdateAngularSpeedToggle()
    {
        if (AngularSpeedSender.updateAngularSpeed)
            AngularSpeedSender.updateAngularSpeed = false;
        else
            AngularSpeedSender.updateAngularSpeed = true;

        if (AngularSpeedSender.debugAngularSpeed)
            Debug.Log("CSOUND " + referenceObject.name + " update andgular speed = " + AngularSpeedSender.updateAngularSpeed);
    }

    private void SendCsoundDataBasedOnAngularSpeed()
    {
        AngularSpeedSender.rotationSpeed = rigidbody.angularVelocity.magnitude;

        foreach (CsoundChannelDataSO.CsoundChannelData data in AngularSpeedSender.angularSpeedChannels.channelData)
        {
            float value =
                Mathf.Clamp(ScaleFloat(0, AngularSpeedSender.maxAngularSpeedValue, data.minValue, data.maxValue, rigidbody.angularVelocity.magnitude), data.minValue, data.maxValue);

            csoundUnity.SetChannel(data.name, value);
        }

        if (AngularSpeedSender.debugAngularSpeed)
            Debug.Log("CSOUND " + referenceObject.name + " angular speed: " + AngularSpeedSender.rotationSpeed);
    }
    #endregion

    #region ROTATION
    public void UpdateRotation(bool update)
    {
        RotationSender.updateRotation = update;

        GetInitialRotation();

        if (RotationSender.debugRotation)
            Debug.Log("CSOUND " + gameObject.name + " update rotation  = " + RotationSender.updateRotation);
    }

    public void UpdateRotationToggle()
    {
        if (RotationSender.updateRotation)
            RotationSender.updateRotation = false;
        else
            RotationSender.updateRotation = true;

        UpdateRotation(RotationSender.updateRotation);
    }

    private void GetInitialRotation()
    {
        if (!RotationSender.useLocalEulerAngles)
            RotationSender.rotationStart = transform.eulerAngles;
        else
            RotationSender.rotationStart = transform.localEulerAngles;
    }

    private void CalculateRelativeRotation()
    {
        if (!RotationSender.useLocalEulerAngles)
            RotationSender.localRotation = transform.eulerAngles;
        else
            RotationSender.localRotation = transform.localEulerAngles;

        RotationSender.rotationRelative = RotationSender.localRotation - RotationSender.rotationStart;

        if (RotationSender.debugRotation)
            Debug.Log("CSOUND " + referenceObject.name + " relative rotation  = " + RotationSender.rotationRelative);
    }

    private float CircularAxisValue(float rotationAxis)
    {
        float value;

        if (rotationAxis >= 180)
            value = ((180 * 2) - rotationAxis) * 2;
        else
            value = rotationAxis * 2;

        //if (RotationSender.debugRotation)
        //    Debug.Log("CSOUND " + referenceObject.name + " circular rotation value  = " + value);

        return value;
    }


    private void SetCsoundValuesXRotation()
    {
        if (RotationSender.rotationMode == CsoundRotation.RotationMode.Circular)
        {
            if (RotationSender.setXRotationTo == CsoundRotation.RotationVectorReference.Absolute)
                SetCsoundChannelBasedOnAxis(RotationSender.csoundChannelsRotationX, 0, 180, CircularAxisValue(RotationSender.localRotation.x));
            else if (RotationSender.setXRotationTo == CsoundRotation.RotationVectorReference.Relative)
                SetCsoundChannelBasedOnAxis(RotationSender.csoundChannelsRotationX, 0, 180, CircularAxisValue(RotationSender.rotationRelative.x));
        }
    }

    private void SetCsoundValuesYRotation()
    {
        if (RotationSender.rotationMode == CsoundRotation.RotationMode.Circular)
        {
            if (RotationSender.setYRotationTo == CsoundRotation.RotationVectorReference.Absolute)
                SetCsoundChannelBasedOnAxis(RotationSender.csoundChannelsRotationY, 0, 360, CircularAxisValue(RotationSender.localRotation.y));
            else if (RotationSender.setYRotationTo == CsoundRotation.RotationVectorReference.Relative)
                SetCsoundChannelBasedOnAxis(RotationSender.csoundChannelsRotationY, 0, 360, CircularAxisValue(RotationSender.rotationRelative.y));
        }
    }

    private void SetCsoundValuesZRotation()
    {
        if (RotationSender.rotationMode == CsoundRotation.RotationMode.Circular)
        {
            if (RotationSender.setZRotationTo == CsoundRotation.RotationVectorReference.Absolute)
                SetCsoundChannelBasedOnAxis(RotationSender.csoundChannelsRotationZ, 0, 360, CircularAxisValue(RotationSender.localRotation.z));
            else if (RotationSender.setZRotationTo == CsoundRotation.RotationVectorReference.Relative)
                SetCsoundChannelBasedOnAxis(RotationSender.csoundChannelsRotationZ, 0, 360, CircularAxisValue(RotationSender.rotationRelative.z));
        }
    }

    #endregion

    #region SCALE MAGNITUDE

    public void UpdateScaleMagnitude(bool update)
    {
        ScaleMagnitudeSender.updateScaleMagnitude = update;

        if (ScaleMagnitudeSender.updateScaleMagnitude)
        {
            if (ScaleMagnitudeSender.useLocalScaleMagnitude)
                ScaleMagnitudeSender.scaleMagnitudeStart = referenceObject.transform.localScale.magnitude;
            else
                ScaleMagnitudeSender.scaleMagnitudeStart = referenceObject.transform.lossyScale.magnitude;
        }


        if (ScaleMagnitudeSender.debugScaleMagnitude)
            Debug.Log("CSOUND " + gameObject.name + " update scale magnitude = " + ScaleMagnitudeSender.updateScaleMagnitude);
    }

    public void UpdateScaleMagnitudeToggle()
    {
        if (ScaleMagnitudeSender.updateScaleMagnitude)
            ScaleMagnitudeSender.updateScaleMagnitude = false;
        else
            ScaleMagnitudeSender.updateScaleMagnitude = true;

        UpdateScaleMagnitude(ScaleMagnitudeSender.updateScaleMagnitude);
    }

    private void SendCsoundDataBasedOnScaleMagnitude()
    {
        if (ScaleMagnitudeSender.useLocalScaleMagnitude)
            ScaleMagnitudeSender.scaleMagnitudeCurrent = referenceObject.transform.localScale.magnitude;
        else
            ScaleMagnitudeSender.scaleMagnitudeCurrent = referenceObject.transform.lossyScale.magnitude;

        if (ScaleMagnitudeSender.setScaleMagnitudeTo == CsoundScaleMagnitude.ScaleMagnitudeVectorReference.Relative)
        {
            ScaleMagnitudeSender.scaleMagnitudeFinal = ScaleMagnitudeSender.scaleMagnitudeCurrent - ScaleMagnitudeSender.scaleMagnitudeStart;
        }
        else if (ScaleMagnitudeSender.setScaleMagnitudeTo == CsoundScaleMagnitude.ScaleMagnitudeVectorReference.Absolute)
        {
            ScaleMagnitudeSender.scaleMagnitudeFinal = ScaleMagnitudeSender.scaleMagnitudeCurrent;
        }

        foreach (CsoundChannelDataSO.CsoundChannelData channelData in ScaleMagnitudeSender.scaleMagnitudeChannels.channelData)
        {
            //Scales the value passed to Csound based on the minValue and maxValue defined for each channel.
            float scaledValue =
                Mathf.Clamp(ScaleFloat(0, ScaleMagnitudeSender.scaleMagnitudeMax, channelData.minValue, channelData.maxValue, ScaleMagnitudeSender.scaleMagnitudeFinal), channelData.minValue, channelData.maxValue);
            //Passes values to Csound.
            csoundUnity.SetChannel(channelData.name, scaledValue);
        }

        if (ScaleMagnitudeSender.debugScaleMagnitude)
            Debug.Log("CSOUND " + referenceObject.name + " scale magnitude = " + ScaleMagnitudeSender.scaleMagnitudeFinal);
    }

    #endregion

    #region SCALE AXIS
    public void UpdateScaleAxis(bool update)
    {
        ScaleAxisSender.updateScaleAxis = update;

        if (update)
            GetRelativeStartingScale();

        if (ScaleAxisSender.setXScaleTo == CsoundScaleAxis.ScaleVectorReference.Relative ||
            ScaleAxisSender.setYScaleTo == CsoundScaleAxis.ScaleVectorReference.Relative ||
            ScaleAxisSender.setZScaleTo == CsoundScaleAxis.ScaleVectorReference.Relative)
            ScaleAxisSender.calculateRelativeScale = true;

        if (ScaleAxisSender.debugScaleAxis)
            Debug.Log("CSOUND " + referenceObject.name + " update scale axis = " + ScaleAxisSender.updateScaleAxis);
    }

    public void UpdateScaleAxisToggle()
    {
        if (ScaleAxisSender.updateScaleAxis)
            ScaleAxisSender.updateScaleAxis = false;
        else
            ScaleAxisSender.updateScaleAxis = true;

        UpdateScaleAxis(ScaleAxisSender.updateScaleAxis);
    }

    private void GetRelativeStartingScale()
    {
        if (ScaleAxisSender.useLocalScale)
            ScaleAxisSender.startScale = referenceObject.transform.localScale;
        else
            ScaleAxisSender.startScale = referenceObject.transform.lossyScale;
    }

    private void CalculateRelativeScale()
    {
        if (ScaleAxisSender.useLocalScale)
        {
            ScaleAxisSender.relativeScale = referenceObject.transform.localScale - ScaleAxisSender.startScale;
        }
        else
        {
            ScaleAxisSender.relativeScale = referenceObject.transform.lossyScale - ScaleAxisSender.startScale;
        }

        if (ScaleAxisSender.debugScaleAxis)
            Debug.Log("CSOUND " + referenceObject.name + " relative scale: " + ScaleAxisSender.relativeScale);
    }

    private void SetCsoundValuesScaleX()
    {
        if (ScaleAxisSender.setXScaleTo == CsoundScaleAxis.ScaleVectorReference.Absolute)
        {
            if (ScaleAxisSender.useLocalScale)
                SetCsoundChannelBasedOnAxis(ScaleAxisSender.csoundChannelsScaleX, ScaleAxisSender.scaleVectorRangesMin.x, ScaleAxisSender.scaleVectorRangesMax.x, referenceObject.transform.localScale.x);
            else
                SetCsoundChannelBasedOnAxis(ScaleAxisSender.csoundChannelsScaleX, ScaleAxisSender.scaleVectorRangesMin.x, ScaleAxisSender.scaleVectorRangesMax.x, referenceObject.transform.lossyScale.x);
        }
        else
            SetCsoundChannelBasedOnAxis(ScaleAxisSender.csoundChannelsScaleX, ScaleAxisSender.scaleVectorRangesMin.x, ScaleAxisSender.scaleVectorRangesMax.x, ScaleAxisSender.relativeScale.x);
    }

    private void SetCsoundValuesScaleY()
    {
        if (ScaleAxisSender.setYScaleTo == CsoundScaleAxis.ScaleVectorReference.Absolute)
        {
            if (ScaleAxisSender.useLocalScale)
                SetCsoundChannelBasedOnAxis(ScaleAxisSender.csoundChannelsScaleY, ScaleAxisSender.scaleVectorRangesMin.y, ScaleAxisSender.scaleVectorRangesMax.y, referenceObject.transform.localScale.y);
            else
                SetCsoundChannelBasedOnAxis(ScaleAxisSender.csoundChannelsScaleY, ScaleAxisSender.scaleVectorRangesMin.y, ScaleAxisSender.scaleVectorRangesMax.y, referenceObject.transform.lossyScale.y);
        }
        else
            SetCsoundChannelBasedOnAxis(ScaleAxisSender.csoundChannelsScaleY, ScaleAxisSender.scaleVectorRangesMin.y, ScaleAxisSender.scaleVectorRangesMax.y, ScaleAxisSender.relativeScale.y);
    }

    private void SetCsoundValuesScaleZ()
    {
        if (ScaleAxisSender.setZScaleTo == CsoundScaleAxis.ScaleVectorReference.Absolute)
        {
            if (ScaleAxisSender.useLocalScale)
                SetCsoundChannelBasedOnAxis(ScaleAxisSender.csoundChannelsScaleZ, ScaleAxisSender.scaleVectorRangesMin.z, ScaleAxisSender.scaleVectorRangesMax.z, referenceObject.transform.localScale.z);
            else
                SetCsoundChannelBasedOnAxis(ScaleAxisSender.csoundChannelsScaleZ, ScaleAxisSender.scaleVectorRangesMin.z, ScaleAxisSender.scaleVectorRangesMax.z, referenceObject.transform.lossyScale.z);
        }
        else
            SetCsoundChannelBasedOnAxis(ScaleAxisSender.csoundChannelsScaleZ, ScaleAxisSender.scaleVectorRangesMin.z, ScaleAxisSender.scaleVectorRangesMax.z, ScaleAxisSender.relativeScale.z);
    }
    #endregion

    #region UTILITIES
    private float ScaleFloat(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {
        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }


    private void SetCsoundChannelBasedOnAxis(CsoundChannelDataSO csoundChannels, float minVectorRange, float maxVectorRange, float vectorAxis, bool returnAbsoluteValue)
    {
        foreach (CsoundChannelDataSO.CsoundChannelData data in csoundChannels.channelData)
        {
            float value =
                Mathf.Clamp(ScaleFloat(minVectorRange, maxVectorRange, data.minValue, data.maxValue, vectorAxis), data.minValue, data.maxValue);

            if (!returnAbsoluteValue)
            {
                csoundUnity.SetChannel(data.name, value);
            }
            else
            {
                csoundUnity.SetChannel(data.name, Mathf.Abs(value));
            }
        }
    }

    private void SetCsoundChannelBasedOnAxis(CsoundChannelDataSO csoundChannels, float minVectorRange, float maxVectorRange, float vectorAxis)
    {
        foreach (CsoundChannelDataSO.CsoundChannelData data in csoundChannels.channelData)
        {
            float value =
                Mathf.Clamp(ScaleFloat(minVectorRange, maxVectorRange, data.minValue, data.maxValue, vectorAxis), data.minValue, data.maxValue);

            csoundUnity.SetChannel(data.name, value);
        }
    }
    #endregion
}

[System.Serializable]
public class CsoundPosition
{
    public enum PositionVectorReference { Absolute, Relative, RelativeToCamera, None };
    [Tooltip("Define if the position X axis is taken as an absolute value, a relative value to its starting rotation, or a value relative to the object's orientation to the camera.")]
    public PositionVectorReference setXPositionTo = PositionVectorReference.None;

    [Tooltip("Define if the position Y axis is taken as an absolute value, a relative value to its starting rotation, or a value relative to the object's orientation to the camera.")]
    public PositionVectorReference setYPositionTo = PositionVectorReference.None;

    [Tooltip("Define if the rotation Z axis is taken as an absolute value, a relative value to its starting rotation, or a value relative to the object's orientation to the camera.")]
    public PositionVectorReference setZPositionTo = PositionVectorReference.None;

    [Tooltip("Minimum and maximum transform position values for scaling Csound channel values.")]
    public Vector3 posVectorRangesMax, posVectorRangesMin;

    [Tooltip("Returns only positive value for each position axis")]
    public bool returnAbsoluteValuesPosX, returnAbsoluteValuesPosY, returnAbsoluteValuesPosZ;
    [Space]
    [Tooltip("Csound channels that will be affected by the position X axis.")]
    public CsoundChannelDataSO csoundChannelsPosX;

    [Tooltip("Csound channels that will be affected by the position Y axis.")]
    public CsoundChannelDataSO csoundChannelsPosY;

    [Tooltip("Csound channels that will be affected by the position Z axis.")]
    public CsoundChannelDataSO csoundChannelsPosZ;

    [Tooltip("Starts passing position values to Csound on Start.")]
    public bool updatePositionOnStart = false;

    [Tooltip("Prints the object's relative position on Update.")]
    public bool debugPosition = false;

    [HideInInspector] public Transform camera;
    [HideInInspector] public Vector3 startPos, startPosCameraRelative;
    [HideInInspector] public Vector3 relativePos, relativeCameraPos;
    [HideInInspector] public bool calculateRelativePos;
    [HideInInspector] public bool calculateRelativeCameraPos;
    [HideInInspector] public bool updatePosition = false;
}

[System.Serializable]
public class CsoundRotation
{
    public enum RotationVectorReference { Absolute, Relative, None };
    public enum RotationMode { Circular };

    [Tooltip("Rotation values will be passed in a circular fashion. Values for the Z and Y axis will wrap around every 180 degress while the X axis will wrap around every 90 degrees.")]
    public RotationMode rotationMode = RotationMode.Circular;

    [Tooltip("Define if the rotation X axis is taken as an absolute value or relative to its starting rotation.")]
    public RotationVectorReference setXRotationTo = RotationVectorReference.Relative;

    [Tooltip("Define if the rotation Y axis is taken as an absolute value or relative to its starting rotation.")]
    public RotationVectorReference setYRotationTo = RotationVectorReference.Relative;

    [Tooltip("Define if the rotation Z axis is taken as an absolute value or relative to its starting rotation.")]
    public RotationVectorReference setZRotationTo = RotationVectorReference.Relative;
    [Space]
    [Tooltip("Csound channels that will be affected by the rotation X axis.")]
    public CsoundChannelDataSO csoundChannelsRotationX;

    [Tooltip("Csound channels that will be affected by the rotation Y axis.")]
    public CsoundChannelDataSO csoundChannelsRotationY;

    [Tooltip("Csound channels that will be affected by the rotation Z axis.")]
    public CsoundChannelDataSO csoundChannelsRotationZ;

    [Tooltip("If true, uses local Euler angles as rotation values")]
    public bool useLocalEulerAngles;

    [Tooltip("Starts passing rotation values to Csound on Start.")]
    public bool updateRotationOnStart = false;

    [Tooltip("Prints the object's rotation on Update.")]
    public bool debugRotation = false;

    [HideInInspector] public bool updateRotation;
    [HideInInspector] public Vector3 rotationStart, rotationRelative, localRotation;
}

[System.Serializable]
public class CsoundScaleAxis
{
    public enum ScaleVectorReference { Absolute, Relative, None };
    [Tooltip("Define if the scale X axis is taken as an absolute value or relative to its starting scale.")]
    public ScaleVectorReference setXScaleTo = ScaleVectorReference.None;

    [Tooltip("Define if the scale Y axis is taken as an absolute value or relative to its starting scale.")]
    public ScaleVectorReference setYScaleTo = ScaleVectorReference.None;

    [Tooltip("Define if the scale Z axis is taken as an absolute value or relative to its starting scale.")]
    public ScaleVectorReference setZScaleTo = ScaleVectorReference.None;

    [Tooltip("Minimum and maximum transform scale values for scaling Csound channel values.")]
    public Vector3 scaleVectorRangesMax, scaleVectorRangesMin;
    [Space]
    [Tooltip("Csound channels that will be affected by the scale X axis.")]
    public CsoundChannelDataSO csoundChannelsScaleX;

    [Tooltip("Csound channels that will be affected by the scale Y axis.")]
    public CsoundChannelDataSO csoundChannelsScaleY;

    [Tooltip("Csound channels that will be affected by the scale Z axis.")]
    public CsoundChannelDataSO csoundChannelsScaleZ;

    [Tooltip("If true, uses local scale. If false, uses lossy scale values.")]
    public bool useLocalScale = true;

    [Tooltip("Starts passing scale values to Csound on Start.")]
    public bool updateScaleOnStart;

    [Tooltip("Prints the object's relative scale on Update.")]
    public bool debugScaleAxis;

    [HideInInspector] public Vector3 startScale, relativeScale;
    [HideInInspector] public bool calculateRelativeScale;
    [HideInInspector] public bool updateScaleAxis = false;
}

[System.Serializable]
public class CsoundScaleMagnitude
{
    public enum ScaleMagnitudeVectorReference { Absolute, Relative, None };
    [Tooltip("Define if the magnitude of the object's scale is taken as an absolute value or relative to its starting scale.")]
    public ScaleMagnitudeVectorReference setScaleMagnitudeTo = ScaleMagnitudeVectorReference.Relative;

    [Tooltip("Csound channels that will be affected by the object's scale magnitude value.")]
    public CsoundChannelDataSO scaleMagnitudeChannels;

    [Tooltip("Maximum scale magnitude value used for scaling Csound channel values.")]
    public float scaleMagnitudeMax;

    [Tooltip("If true, uses local scale. If false, uses lossy scale to calculate the vector magnitude.")]
    public bool useLocalScaleMagnitude = true;

    [Tooltip("Starts calculating the scale magnitude and passing data into Csound on Start.")]
    public bool updateScaleMagnitudeOnStart = false;

    [Tooltip("Prints the object's scale magnitude on Update.")]
    public bool debugScaleMagnitude = false;

    [HideInInspector] public bool updateScaleMagnitude;
    [HideInInspector] public float scaleMagnitudeCurrent, scaleMagnitudeStart, scaleMagnitudeFinal;
}

[System.Serializable]
public class CsoundSpeed
{
    public enum SpeedSource { Rigidbody, Transform, None };
    [Tooltip("Component that will be used to calculate the speed values.")]
    public SpeedSource speedSource = SpeedSource.None;

    [Tooltip("Csound channels that will be affected by the object's speed.")]
    public CsoundChannelDataSO speedChannelData;

    [Tooltip("Maximum speed value used for scaling Csound channel values.")]
    public float maxSpeedValue;

    [Tooltip("Starts calculating speed and passing data into Csound on Start.")]
    public bool updateSpeedOnStart = false;

    [Tooltip("Prints the object's speed on Update.")]
    public bool debugSpeed = false;

    [HideInInspector] public float speed;
    [HideInInspector] public Vector3 previousPosSpeed;
    [HideInInspector] public bool updateSpeed = false;
}

[System.Serializable]
public class CsoundAngularSpeed
{
    public enum AngularSpeedSource { None, Rigidbody }
    [Tooltip("Component that will be used to calculate the angular speed values.")]
    public AngularSpeedSource angularSpeedSource = AngularSpeedSource.None;

    [Tooltip("Csound channels that will be affected by the object's angular speed.")]
    public CsoundChannelDataSO angularSpeedChannels;

    [Tooltip("Maximum angular speed value used for scaling Csound channel values.")]
    public float maxAngularSpeedValue;

    [Tooltip("Starts calculating angular speed and passing data into Csound on Start.")]
    public bool updateAngularSpeedOnStart;
    [Tooltip("Prints the object's angular speed on Update.")]
    public bool debugAngularSpeed = false;

    [HideInInspector] public float rotationSpeed;
    [HideInInspector] public bool updateAngularSpeed = false;
}