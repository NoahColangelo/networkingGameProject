using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerControls : MonoBehaviourPunCallbacks, IPunObservable
{
    #region Variables (All)

    [SerializeField]
    private LayerMask _groundLayerMask;
    [SerializeField]
    private Transform _modelTransform;
    [SerializeField]
    GameObject beam;
    [SerializeField]
    public PlayerUI playerUIPrefab;


    private PlayerInput _playerInput;
    private Animator animator;
    private Vector2 _movementVector;
    private Rigidbody _rb;

    public float _movementSpeed = 10.0f;

    public static GameObject LocalPlayerInstance;

    //possibly temp
    public float directionDampTime = 0.25f;
    public bool isFiring = false;
    public float health = 1.0f;

    #endregion

    void Awake()
    {
        animator = GetComponent<Animator>();
        if(!animator)
            Debug.LogError("Missing Animator Component", this);

        if (!beam)
            Debug.LogError("missing gameobject Beam", this);
        else
            beam.SetActive(false);

        if (!_modelTransform)
            Debug.LogError("missing child transform", this);

        if (photonView.IsMine)
            PlayerControls.LocalPlayerInstance = gameObject;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _playerInput = gameObject.GetComponent <PlayerInput>();
        _rb = GetComponent<Rigidbody>();

        //if (_multicam)
        //{
        //    if (photonView.IsMine)
        //    {
        //        _multicam.OnStartFollowing();
        //    }
        //}
        //else
        //    Debug.LogError("MultiCam is not found", this);

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;

        if (playerUIPrefab)
        {
            PlayerUI _uiGo = Instantiate(playerUIPrefab);
            _uiGo.SetTarget(this);
        }
        else
            Debug.LogWarning("PlayerUI prefeb is missing", this);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            return;

        if (!animator)
            return;

        //if (photonView.IsMine && _playerInput.GetBasicAttack())
        //    isFiring = true;
        //else
        //    isFiring = false;

        if (health <= 0.0f && photonView.IsMine)
            GameManager.Instance.LeaveRoom();

        //reads the values of the keys being pressed from the input manager and stores them in movement vector
        _movementVector = _playerInput.GetPlayerMovement().ReadValue<Vector2>();

        LookAtMousePosition();
        UpdateAnimations();

        beam.SetActive(isFiring);
    }

    void FixedUpdate()
    {
        Vector3 _movementVector3D = new Vector3(_movementVector.x, 0.0f, _movementVector.y);
        _rb.MovePosition(_rb.position + _movementVector3D * (_movementSpeed * Time.fixedDeltaTime));
    }

    private void LookAtMousePosition()
    {
        //sets the Y to that of the players so only the Y will rotate
        Vector3 lookAtRotation = new Vector3(_playerInput.GetMousePosition().x, transform.position.y, _playerInput.GetMousePosition().z);

        //rotates the model instead of the parent gameobject to compenate for the camera stutter problem
        _modelTransform.LookAt(lookAtRotation);
    }

    private void UpdateAnimations()
    {
        float h = _movementVector.x;
        float v = _movementVector.y;

        animator.SetFloat("Speed", h * h + v);
        animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
    }

    #region Triggers and collisions

    private void OnTriggerEnter(Collider other)
    {
        if(!photonView.IsMine)
            return;

        if (!other.name.Contains("Beam"))
            return;

        health -= 0.1f;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!photonView.IsMine)
            return;

        if (!other.name.Contains("Beam"))
            return;

        health -= 0.1f * Time.deltaTime;
    }

    #endregion

    private void CalledOnLevelWasLoaded(int level)
    {
        if(!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0f, 5f, 0f);
        }

        PlayerUI _uiGo = Instantiate(playerUIPrefab);
        _uiGo.SetTarget(this);
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
    {
        this.CalledOnLevelWasLoaded(scene.buildIndex);
    }

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(isFiring);
            stream.SendNext(health);
        }
        else
        {
            // Network player, receive data
            isFiring = (bool)stream.ReceiveNext();
            health = (float)stream.ReceiveNext();
        }
    }

    #endregion
}
