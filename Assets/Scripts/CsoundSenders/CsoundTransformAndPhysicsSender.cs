using System.Collections;
using UnityEngine;

//TODO
    //PACKAGING
        //Write summaries/comments for all functions
        //Thorough commenting of code

    //BACKLOG
        //Angular speed: makes it so it can calculate the angular speed from the transform
        //Add VelocitySender: pass data based on each individual velocity vector axis 
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
                Debug.LogWarning("No Rigidbody component attached to " + referenceObject.name + ". This might lead to errors if you're using either the SpeedSender or AngularSpeedSender functions.");
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

    //Waits for Csound to initialize before executing functions on Start.
    private IEnumerator Initialization()
    {
        ///Waits for CsoundUnity to initialize
        while (!csoundUnity.IsInitialized)
        {
            Debug.Log("CSOUND NOT INITIALIZED");
            yield return null;
        }

        Debug.Log("CSOUND INITIALIZED");

        //Start updating values for each module on Start if the bool is toggled in the inspector
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
        //Calculates speed and send values to Csound if conditions are met.
        if (SpeedSender.updateSpeed && SpeedSender.speedSource != CsoundSpeed.SpeedSource.None)
            SendCsoundDataBasedOnSpeed();

        //Calculates angular speed and send values to Csound if conditions are met.
        if (AngularSpeedSender.updateAngularSpeed && AngularSpeedSender.angularSpeedSource != CsoundAngularSpeed.AngularSpeedSource.None)
            SendCsoundDataBasedOnAngularSpeed();
    }

    private void LateUpdate()
    {
        //Passes position values into Csound if conditions are met.
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

        //Passes rotation values into Csound if conditions are met.
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

        //Passes scale axis values into Csound if conditions are met.
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

        //Passes scale magnitude values into Csound.
        if (ScaleMagnitudeSender.updateScaleMagnitude && ScaleMagnitudeSender.setScaleMagnitudeTo != CsoundScaleMagnitude.ScaleMagnitudeVectorReference.None)
            SendCsoundDataBasedOnScaleMagnitude();
    }
    #endregion

    #region SPEED
    private void SendCsoundDataBasedOnSpeed()
    {
        //Checks if speed should be calculated from the objects Rigidbody or from the Transform as defined in the inspector.
        if (SpeedSender.speedSource == CsoundSpeed.SpeedSource.Rigidbody)
        {
            //Gets speed from the Rigidbody's velocity.
            SpeedSender.speed = rigidbody.velocity.magnitude;
        }
        else if (SpeedSender.speedSource == CsoundSpeed.SpeedSource.Transform)
        {
            //Calculates speed based on the transform position difference between each frame.
            SpeedSender.speed = (referenceObject.transform.position - SpeedSender.previousPosSpeed).magnitude / Time.deltaTime;
            SpeedSender.previousPosSpeed = referenceObject.transform.position;
        }

        //Assign values to Csound channels based on the object's speed.
        foreach (CsoundChannelRangeSO.CsoundChannelData channelData in SpeedSender.speedChannelData.channelData)
        {
            //Scales the value passed to Csound based on the minValue and maxValue defined for each channel between 0 (object is stopeed) and the max speed value defined in the inspector
            float scaledSpeedValue =
                Mathf.Clamp(ScaleFloat(0, SpeedSender.maxSpeedValue, channelData.minValue, channelData.maxValue, SpeedSender.speed), channelData.minValue, channelData.maxValue);
            //Passes values to Csound.
            csoundUnity.SetChannel(channelData.name, scaledSpeedValue);
        }

        //Prints speed value on the console.
        if (SpeedSender.debugSpeed)
            Debug.Log(SpeedSender.speed);

    }

    /// <summary>
    /// Starts calcualting the object speed and passing that value into Csound if the bool is true and stops it if false.
    /// </summary>
    /// <param name="update"></param>
    public void UpdateSpeed(bool update)
    {
        SpeedSender.updateSpeed = update;

        if (SpeedSender.debugSpeed)
            Debug.Log("CSOUND " + gameObject.name + " update speed = " + SpeedSender.updateSpeed);
    }

    /// <summary>
    /// Toggles the update speed bool between true and false to either start or stop calculating the object speed and passing it into Csound.
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
    /// <summary>
	/// Starts passing the object's transform position data into Csound if the bool is true and stops it if false.
	/// </summary>
	/// <param name="update"></param>
    public void UpdatePosition(bool update)
    {
        PositionSender.updatePosition = update;

        if (update)
            GetRelativeStartingPosition();

        //Calculates object's position relative to camera if any of the axes are set to RelativeToCamera.
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

    /// <summary>
	/// Toggles the update position bool to either start or stop passing data from the transform position to Csound.
	/// </summary>
    public void UpdatePositionToggle()
    {
        if (PositionSender.updatePosition)
            PositionSender.updatePosition = false;
        else
            PositionSender.updatePosition = true;

        UpdatePosition(PositionSender.updatePosition);
    }

    //Gets the object starting position to calculate its relative position.
    private void GetRelativeStartingPosition()
    {
        PositionSender.startPos = referenceObject.transform.position;

        if (PositionSender.calculateRelativeCameraPos)
            PositionSender.startPosCameraRelative = PositionSender.camera.transform.InverseTransformPoint(referenceObject.transform.position);
    }

    //Calculates position relative to the object's starting point.
    private void CaculateRelativePos()
    {
        PositionSender.relativePos = referenceObject.transform.position - PositionSender.startPos;

        if (PositionSender.debugPosition)
            Debug.Log("CSOUND " + referenceObject.name + " relative position: " + PositionSender.relativePos);
    }

    //Calculates position relative to the object's starting point, making it also relative to the camera's orientation.
    private void CalculateRelativeCameraPos()
    {
        Vector3 currentTransform = PositionSender.camera.transform.InverseTransformPoint(transform.position);

        PositionSender.relativeCameraPos = currentTransform - PositionSender.startPosCameraRelative;

        if (PositionSender.debugPosition)
            Debug.Log("CSOUND " + referenceObject.name + " relative position: " + PositionSender.relativeCameraPos);
    }

    //Passes the X position axis values to Csound
    private void SetCsoundValuesPosX()
    {
        if (PositionSender.setXPositionTo == CsoundPosition.PositionVectorReference.Absolute)
            SetCsoundChannelBasedOnAxis(PositionSender.csoundChannelsPosX, PositionSender.posVectorRangesMin.x, PositionSender.posVectorRangesMax.x, transform.position.x);
        else if (PositionSender.setXPositionTo == CsoundPosition.PositionVectorReference.RelativeToCamera)
            SetCsoundChannelBasedOnAxis(PositionSender.csoundChannelsPosX, PositionSender.posVectorRangesMin.x, PositionSender.posVectorRangesMax.x, PositionSender.relativeCameraPos.x);
        else
            SetCsoundChannelBasedOnAxis(PositionSender.csoundChannelsPosX, PositionSender.posVectorRangesMin.x, PositionSender.posVectorRangesMax.x, PositionSender.relativePos.x);
    }

    //Passes the Y position axis values to Csound
    private void SetCsoundValuesPosY()
    {
        if (PositionSender.setYPositionTo == CsoundPosition.PositionVectorReference.Absolute)
            SetCsoundChannelBasedOnAxis(PositionSender.csoundChannelsPosY, PositionSender.posVectorRangesMin.y, PositionSender.posVectorRangesMax.y, transform.position.y);
        else if (PositionSender.setXPositionTo == CsoundPosition.PositionVectorReference.RelativeToCamera)
            SetCsoundChannelBasedOnAxis(PositionSender.csoundChannelsPosY, PositionSender.posVectorRangesMin.y, PositionSender.posVectorRangesMax.y, PositionSender.relativeCameraPos.y);
        else
            SetCsoundChannelBasedOnAxis(PositionSender.csoundChannelsPosY, PositionSender.posVectorRangesMin.y, PositionSender.posVectorRangesMax.y, PositionSender.relativePos.y);
    }

    //Passes the Z position axis values to Csound
    private void SetCsoundValuesPosZ()
    {
        if (PositionSender.setZPositionTo == CsoundPosition.PositionVectorReference.Absolute)
            SetCsoundChannelBasedOnAxis(PositionSender.csoundChannelsPosZ, PositionSender.posVectorRangesMin.z, PositionSender.posVectorRangesMax.z, transform.position.z);
        else if (PositionSender.setZPositionTo == CsoundPosition.PositionVectorReference.RelativeToCamera)
            SetCsoundChannelBasedOnAxis(PositionSender.csoundChannelsPosZ, PositionSender.posVectorRangesMin.z, PositionSender.posVectorRangesMax.z, PositionSender.relativeCameraPos.z);
        else
            SetCsoundChannelBasedOnAxis(PositionSender.csoundChannelsPosZ, PositionSender.posVectorRangesMin.z, PositionSender.posVectorRangesMax.z, PositionSender.relativePos.z);
    }
    #endregion

    #region ANGULAR SPEED/TORQUE

    /// <summary>
    /// Starts passing the object's angular speed data into Csound if the bool is true and stops it if false.
    /// </summary>
    /// <param name="update"></param>
    public void UpdateAngularSpeed(bool update)
    {
        AngularSpeedSender.updateAngularSpeed = update;

        if (AngularSpeedSender.debugAngularSpeed)
            Debug.Log("CSOUND " + gameObject.name + " update andgular speed = " + AngularSpeedSender.updateAngularSpeed);
    }

    /// <summary>
	/// Toggles the update angular speed bool to either start or stop passing angular speed data to Csound.
	/// </summary>
    public void UpdateAngularSpeedToggle()
    {
        if (AngularSpeedSender.updateAngularSpeed)
            AngularSpeedSender.updateAngularSpeed = false;
        else
            AngularSpeedSender.updateAngularSpeed = true;

        if (AngularSpeedSender.debugAngularSpeed)
            Debug.Log("CSOUND " + referenceObject.name + " update andgular speed = " + AngularSpeedSender.updateAngularSpeed);
    }

    //Sends angular speed data into Csound
    private void SendCsoundDataBasedOnAngularSpeed()
    {
        //Gets the magnitude of the angular velocity vector.
        AngularSpeedSender.rotationSpeed = rigidbody.angularVelocity.magnitude;

        //Cycles through each channel defined in the angular speed channels and assign values.
        foreach (CsoundChannelRangeSO.CsoundChannelData data in AngularSpeedSender.angularSpeedChannels.channelData)
        {
            //Scales the value passed to Csound based on the minValue and maxValue defined for each channel between 0 (object is stopeed) and the max angular speed value defined in the inspector.
            float value =
                Mathf.Clamp(ScaleFloat(0, AngularSpeedSender.maxAngularSpeedValue, data.minValue, data.maxValue, rigidbody.angularVelocity.magnitude), data.minValue, data.maxValue);
            //Passes values into Csound.
            csoundUnity.SetChannel(data.name, value);
        }

        if (AngularSpeedSender.debugAngularSpeed)
            Debug.Log("CSOUND " + referenceObject.name + " angular speed: " + AngularSpeedSender.rotationSpeed);
    }
    #endregion

    #region ROTATION
    /// <summary>
    /// Starts passing the object's roation data into Csound if the bool is true and stops it if false.
	/// </summary>
	/// <param name="update"></param>
    public void UpdateRotation(bool update)
    {
        RotationSender.updateRotation = update;

        //Gets the object's current rotation in order to calculate its relative rotation.
        GetInitialRotation();

        if (RotationSender.debugRotation)
            Debug.Log("CSOUND " + gameObject.name + " update rotation  = " + RotationSender.updateRotation);
    }

    /// <summary>
	/// Toggles the update roation bool to either start or stop passing angular speed data to Csound.
	/// </summary>
    public void UpdateRotationToggle()
    {
        if (RotationSender.updateRotation)
            RotationSender.updateRotation = false;
        else
            RotationSender.updateRotation = true;

        UpdateRotation(RotationSender.updateRotation);
    }

    //Gets the object's initial rotation.
    private void GetInitialRotation()
    {
        if (!RotationSender.useLocalEulerAngles)
            RotationSender.rotationStart = transform.eulerAngles;
        else
            RotationSender.rotationStart = transform.localEulerAngles;
    }

    //Calculates the relative roation by subtracting the current rotation from the starting rotation.
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

    //Converts the value from a rotation axis to behave in a circular manner, wrapping around at 180 degrees.
    private float CircularAxisValue(float rotationAxis)
    {
        //Value that will be passed into Csound.
        float value;

        //If the rotation axis is greater than 180, mirrors the value back.
        if (rotationAxis >= 180)
            value = ((180 * 2) - rotationAxis) * 2;
        //If rotation axis is lesser than 180, increase the value.
        else
            value = rotationAxis * 2;
        //Returns the value with the wrap around logic.
        return value;
    }

    //Passes the X roation axis values to Csound
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

    //Passes the Y roation axis values to Csound
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

    //Passes the Z roation axis values to Csound
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

        foreach (CsoundChannelRangeSO.CsoundChannelData channelData in ScaleMagnitudeSender.scaleMagnitudeChannels.channelData)
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
    //Scales a float between a minimum and a maximum value.
    private float ScaleFloat(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {
        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }

    //Scales the ChannelRange minValue and maxValue and passes it into Csound, mapping them to a defined range.
    private void SetCsoundChannelBasedOnAxis(CsoundChannelRangeSO csoundChannels, float minVectorRange, float maxVectorRange, float vectorAxis)
    {
        //Cycles through every channel defined in the ChannelRange asset.
        foreach (CsoundChannelRangeSO.CsoundChannelData data in csoundChannels.channelData)
        {
            //Scales the defined minValue and maxValue variables to a range.
            float value =
                Mathf.Clamp(ScaleFloat(minVectorRange, maxVectorRange, data.minValue, data.maxValue, vectorAxis), data.minValue, data.maxValue);

            //Passes the vlaue to Csound
            if (!data.returnAbsoluteValue)
            {
                csoundUnity.SetChannel(data.name, value);
            }
            //Passes the absolute value to Csound if the bool is checked for the channel.
            else
            {
                csoundUnity.SetChannel(data.name, Mathf.Abs(value));
            }
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
    [Space]
    [Tooltip("Csound channels that will be affected by the position X axis.")]
    public CsoundChannelRangeSO csoundChannelsPosX;

    [Tooltip("Csound channels that will be affected by the position Y axis.")]
    public CsoundChannelRangeSO csoundChannelsPosY;

    [Tooltip("Csound channels that will be affected by the position Z axis.")]
    public CsoundChannelRangeSO csoundChannelsPosZ;

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
    public RotationVectorReference setXRotationTo = RotationVectorReference.None;

    [Tooltip("Define if the rotation Y axis is taken as an absolute value or relative to its starting rotation.")]
    public RotationVectorReference setYRotationTo = RotationVectorReference.None;

    [Tooltip("Define if the rotation Z axis is taken as an absolute value or relative to its starting rotation.")]
    public RotationVectorReference setZRotationTo = RotationVectorReference.None;
    [Space]
    [Tooltip("Csound channels that will be affected by the rotation X axis.")]
    public CsoundChannelRangeSO csoundChannelsRotationX;

    [Tooltip("Csound channels that will be affected by the rotation Y axis.")]
    public CsoundChannelRangeSO csoundChannelsRotationY;

    [Tooltip("Csound channels that will be affected by the rotation Z axis.")]
    public CsoundChannelRangeSO csoundChannelsRotationZ;

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
    public CsoundChannelRangeSO csoundChannelsScaleX;

    [Tooltip("Csound channels that will be affected by the scale Y axis.")]
    public CsoundChannelRangeSO csoundChannelsScaleY;

    [Tooltip("Csound channels that will be affected by the scale Z axis.")]
    public CsoundChannelRangeSO csoundChannelsScaleZ;

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
    public CsoundChannelRangeSO scaleMagnitudeChannels;

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
    public CsoundChannelRangeSO speedChannelData;

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
    public CsoundChannelRangeSO angularSpeedChannels;

    [Tooltip("Maximum angular speed value used for scaling Csound channel values.")]
    public float maxAngularSpeedValue;

    [Tooltip("Starts calculating angular speed and passing data into Csound on Start.")]
    public bool updateAngularSpeedOnStart;
    [Tooltip("Prints the object's angular speed on Update.")]
    public bool debugAngularSpeed = false;

    [HideInInspector] public float rotationSpeed;
    [HideInInspector] public bool updateAngularSpeed = false;
}