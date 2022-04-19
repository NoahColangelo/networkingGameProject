using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform _target;
    public float _smoothTime = 0.3F;
    public Vector3 _cameraOffset;

    private Vector3 _velocity = Vector3.zero;

    void FixedUpdate()
    {
        // Define a target position above and behind the target transform
        Vector3 targetPosition = _target.TransformPoint(_cameraOffset);

        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _smoothTime);

        transform.LookAt(_target.position);
    }
}
