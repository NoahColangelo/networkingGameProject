using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Projectile : MonoBehaviour
{
    public ProjectileInfo ProjectileInfo;

    private SphereCollider _sphereCollider;
    private bool _deactivate = false;//acts as a flag for the object pool to know when to take it back

    private float _lifeTimeTimer = 0.0f;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.isTrigger = true;
    }

    private void Update()
    {
        if(!_deactivate)
            LifeTimeTimer(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if(!_deactivate)
            transform.position = transform.position + transform.forward * (ProjectileInfo._speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            //lower enemy health by ProjectileInfo amount
        }

        _deactivate = true;
        _lifeTimeTimer = 0.0f;
    }

    private void LifeTimeTimer(float _dt)
    {
        _lifeTimeTimer += _dt;

        if (_lifeTimeTimer > ProjectileInfo._lifeTime)
        {
            _deactivate = true;
            _lifeTimeTimer = 0.0f;
        }
    }

    public bool GetDeactivate()
    {
        return _deactivate;
    }
    
    public void Reactivate()
    {
        _deactivate = false;
    }
}
