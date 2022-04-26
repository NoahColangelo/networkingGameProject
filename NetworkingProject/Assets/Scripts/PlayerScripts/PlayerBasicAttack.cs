using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerBasicAttack : MonoBehaviour
{
    [SerializeField]
    private PhotonView _photonView;

    public GameObject Projectile;
    public int _poolCount = 3;

    public Transform ShootPoint;

    private float _fireRateTimer = 0.0f;

    private List<GameObject> _projectilePool = new List<GameObject>();
    private PlayerInput _playerInput;

    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();

        if (Projectile != null)
        {
            GameObject temp;
            for (int i = 0; i < _poolCount; i++)//fill the object pool
            {
                temp = Instantiate(Projectile, ShootPoint.position, ShootPoint.rotation);

                _projectilePool.Add(temp);
                _projectilePool[i].SetActive(false);
                DontDestroyOnLoad(_projectilePool[i]);
            }
        }
        else
            Debug.LogWarning("No Projectile Found!", this);
    }

    // Update is called once per frame
    void Update()
    {
        if (_photonView.IsMine)
        {
            //if the player is clicking the basic attack button and the rate of fire timer is ready
            if (_playerInput.GetBasicAttack() && _fireRateTimer >= Projectile.GetComponent<Projectile>().ProjectileInfo._fireRate)
            {
                _photonView.RPC("BasicAttack", RpcTarget.All);
                //BasicAttack();
                _fireRateTimer = 0.0f;
            }
            else
                _fireRateTimer += Time.deltaTime;
        }

        UpdateProjectilePool();
    }

    private void UpdateProjectilePool()//updates any projectiles to be inactive when they set there clean up bool to true
    {
        for (int i = 0; i < _projectilePool.Count; i++)
        {
            if(_projectilePool[i].GetComponent<Projectile>().GetDeactivate())
            {
                _projectilePool[i].gameObject.SetActive(false);
                _projectilePool[i].transform.position = ShootPoint.position;
                _projectilePool[i].transform.rotation = ShootPoint.rotation;
            }
        }
    }

    [PunRPC]
    private void BasicAttack()//runs through the pool to see if there is an avaliable projectile to use
    {
        for (int i = 0; i < _projectilePool.Count; i++)
        {
            if (!_projectilePool[i].activeInHierarchy)
            {
                _projectilePool[i].gameObject.SetActive(true);
                _projectilePool[i].transform.position = ShootPoint.position;
                _projectilePool[i].transform.rotation = ShootPoint.rotation;
                _projectilePool[i].GetComponent<Projectile>().Reactivate();
                break;
            }
        }
        Debug.Log("boop");
    }
}
