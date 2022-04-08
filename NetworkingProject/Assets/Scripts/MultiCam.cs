using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MultiCam : MonoBehaviour
{
    public class MultiCamPresets
    {
        public float _distance = 0.0f;
        public float _height = 0.0f;
        public float _pan = 0.0f;
        public Vector3 _lookAtOffset = Vector3.zero;

        public float _newDistance = 0.0f;
        public float _newHeight = 0.0f;
        public float _newPan = 0.0f;
        public Vector3 _newLookAtOffset = Vector3.zero;

        public void FillInfo(float distance, float height, float pan, Vector3 lookAtOffset)
        {
            _distance = distance;
            _height = height;
            _pan = pan;
            _lookAtOffset = lookAtOffset;
        }
        public void CombineInfo(MultiCamPresets multiCamPresets)
        {
            _newDistance = multiCamPresets._distance + _distance;
            _newHeight = multiCamPresets._height + _height;
            _newPan = multiCamPresets._pan + _pan;
            _newLookAtOffset = multiCamPresets._lookAtOffset + _lookAtOffset;
        }
    }

    [HideInInspector]
    public string[] cameraPresetChoice = new string[] {"First Person", "Third Person", "Top Down", "Custom" };
    [HideInInspector]
    public MultiCamPresets[] cameraPresetValues = new MultiCamPresets[3] {new MultiCamPresets(), new MultiCamPresets(), new MultiCamPresets() };
    [HideInInspector]
    public int presetIndex;


    private Transform _cameraTransform;

    private Vector3 _cameraOffset = Vector3.zero;

    private MultiCamPresets _currentPreset = new MultiCamPresets();

    [SerializeField]
    private float _smoothSpeed = 1.0f;
    [SerializeField]
    private bool _followPlayer = false;

    private bool lookAt;

    // Start is called before the first frame update
    void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (_smoothSpeed <= 0)
            _smoothSpeed = 1.0f;

        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, transform.position + transform.TransformVector(_cameraOffset), _smoothSpeed * Time.deltaTime);

        if (lookAt)
            _cameraTransform.LookAt(transform.position + _currentPreset._newLookAtOffset);
        else
            _cameraTransform.rotation = transform.rotation;
    }

    public void FirstPersonCamera()
    {
        MultiCamPresets HardSetCamValue = new MultiCamPresets();
        HardSetCamValue.FillInfo(0.0f, 2.0f, 0.0f, new Vector3(0, 1.3f, 1));//hard set values for a third person camera//hard set values for a first person camera

        cameraPresetValues[0].CombineInfo(HardSetCamValue);

        _currentPreset = cameraPresetValues[0];
        _cameraOffset = new Vector3(_currentPreset._newPan, _currentPreset._newHeight, -_currentPreset._newDistance);

        lookAt = false;
    }

    public void ThirdPersonCamera()
    {
        MultiCamPresets HardSetCamValue = new MultiCamPresets();
        HardSetCamValue.FillInfo(5.0f, 3.0f, 0.0f, new Vector3(0, 2.5f, 0));//hard set values for a third person camera

        cameraPresetValues[1].CombineInfo(HardSetCamValue);

        _currentPreset = cameraPresetValues[1];
        _cameraOffset = new Vector3(_currentPreset._newPan, _currentPreset._newHeight, -_currentPreset._newDistance);

        lookAt = true;
    }

    public void TopDownCamera()
    {
        MultiCamPresets HardSetCamValue = new MultiCamPresets();
        HardSetCamValue.FillInfo(3.0f, 10.0f, 0.0f, new Vector3(0, 0, 0));//hard set values for a third person camera

        cameraPresetValues[2].CombineInfo(HardSetCamValue);

        _currentPreset = cameraPresetValues[2];
        _cameraOffset = new Vector3(_currentPreset._newPan, _currentPreset._newHeight, -_currentPreset._newDistance);

        lookAt = true;
    }

}
