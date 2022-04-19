using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MultiCam : MonoBehaviour
{
    [HideInInspector]
    public string[] cameraPresetChoice = new string[3] {"First Person", "Third Person", "Top Down" };
    [HideInInspector]
    public MultiCamPresets[] cameraPresetValues = new MultiCamPresets[3] {new MultiCamPresets(), new MultiCamPresets(), new MultiCamPresets() };
    [HideInInspector]
    public int presetIndex;


    private Transform _cameraTransform;

    private Vector3 _cameraOffset = Vector3.zero;
    private Vector3 _velocity = Vector3.zero;

    private MultiCamPresets _currentPreset = new MultiCamPresets();

    [SerializeField]
    private bool _followOnStart = false;

    private bool lookAt;
    private bool _followingPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        // Start following the target if wanted.
        if (_followOnStart)
        {
            OnStartFollowing();
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // The transform target may not destroy on level load, 
        // so we need to cover corner cases where the Main Camera is different everytime we load a new scene, and reconnect when that happens
        //idea taken from photon tutorial camera work script
        if (_cameraTransform == null && _followingPlayer)
        {
            OnStartFollowing();
        }

        if (_followingPlayer)
        {
            //if (_currentPreset._smoothSpeed <= 0)
                //_currentPreset._smoothSpeed = 1.0f;

            //_cameraTransform.position = Vector3.Lerp(_cameraTransform.position, transform.position + transform.TransformVector(_cameraOffset), _smoothSpeed * Time.deltaTime);
            //_cameraTransform.position = Vector3.Slerp(_cameraTransform.position, transform.position + _cameraOffset, _currentPreset._smoothSpeed * Time.deltaTime);
            //_cameraTransform.position = Vector3.SmoothDamp(_cameraTransform.position, transform.position + _cameraOffset, ref _velocity , 0.3f);
            _cameraTransform.position = transform.position + _cameraOffset;


            if (lookAt)
                _cameraTransform.LookAt(transform.position + _currentPreset._newLookAtOffset);
            else
                _cameraTransform.rotation = transform.rotation;
        }
    }

    public void OnStartFollowing()
    {
        _cameraTransform = Camera.main.transform;
        _followingPlayer = true;
    }

    public void FirstPersonCamera()
    {
        MultiCamPresets HardSetCamValue = new MultiCamPresets();
        HardSetCamValue.FillInfo(0.0f, 2.0f, 0.0f, new Vector3(0, 1.3f, 1), 100.0f);//hard set values for a third person camera//hard set values for a first person camera

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
        HardSetCamValue.FillInfo(6.0f, 13.0f, 0.0f, new Vector3(0, 3, 0));//hard set values for a third person camera

        cameraPresetValues[2].CombineInfo(HardSetCamValue);

        _currentPreset = cameraPresetValues[2];
        _cameraOffset = new Vector3(_currentPreset._newPan, _currentPreset._newHeight, -_currentPreset._newDistance);

        lookAt = true;
    }

}
