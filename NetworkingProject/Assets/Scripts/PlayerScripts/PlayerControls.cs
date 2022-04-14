using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerControls : MonoBehaviourPunCallbacks, IPunObservable
{
    #region Variables (All)

    PlayerInput _playerInput;

    private Animator animator;

    private Vector2 movementVector;

    [SerializeField]
    GameObject beam;

    public static GameObject LocalPlayerInstance;

    [SerializeField]
    public PlayerUI playerUIPrefab;

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

        if (photonView.IsMine)
            PlayerControls.LocalPlayerInstance = gameObject;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _playerInput = gameObject.GetComponent <PlayerInput>();
        MultiCam _multicam = gameObject.GetComponent<MultiCam>();

        if (_multicam)
        {
            if (photonView.IsMine)
            {
                _multicam.OnStartFollowing();
            }
        }
        else
            Debug.LogError("MultiCam is not found", this);

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
        //if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        //    return;

        if (!animator)
            return;

        if (photonView.IsMine && _playerInput.GetBasicAttack())
            isFiring = true;
        else
            isFiring = false;

        movementVector = _playerInput.GetPlayerMovement().ReadValue<Vector2>();//reads the values of the keys being pressed from the input manager and stores them in movement vector

        float h = movementVector.x;
        float v = movementVector.y;

        animator.SetFloat("Speed", h * h + v);
        animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);

        if (health <= 0.0f && photonView.IsMine)
            GameManager.Instance.LeaveRoom();

            beam.SetActive(isFiring);
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


#if UNITY_5_4_OR_NEWER

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
    {
        this.CalledOnLevelWasLoaded(scene.buildIndex);
    }

#endif

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
