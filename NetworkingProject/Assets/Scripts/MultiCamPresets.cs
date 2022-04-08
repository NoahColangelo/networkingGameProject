using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiCamPresets
{
    public float _distance = 0.0f;
    public float _height = 0.0f;
    public float _pan = 0.0f;
    public float _smoothSpeed = 0.0f;
    public Vector3 _lookAtOffset = Vector3.zero;

    public float _newDistance = 0.0f;
    public float _newHeight = 0.0f;
    public float _newPan = 0.0f;
    public Vector3 _newLookAtOffset = Vector3.zero;

    public void FillInfo(float distance, float height, float pan, Vector3 lookAtOffset, float smoothSpeed = 10.0f)
    {
        _distance = distance;
        _height = height;
        _pan = pan;
        _smoothSpeed = smoothSpeed;
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
