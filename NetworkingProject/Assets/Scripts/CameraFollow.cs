using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraFollow : MonoBehaviour
{
    private Camera _camera;
    private Transform _target;
    public float _smoothTime = 0.3F;
    public Vector3 _cameraOffset;

    private Vector3 _velocity = Vector3.zero;

    private void Start()
    {
        _target = transform;
        _camera = Camera.main;
    }

    void FixedUpdate()
    {
        // Define a target position above and behind the target transform
        Vector3 targetPosition = _target.TransformPoint(_cameraOffset);

        // Smoothly move the camera towards that target position
        _camera.transform.position = Vector3.SmoothDamp(_camera.transform.position, targetPosition, ref _velocity, _smoothTime);

        _camera.transform.LookAt(_target.position);
    }
}
