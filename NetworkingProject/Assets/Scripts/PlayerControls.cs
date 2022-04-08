using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerControls : MonoBehaviourPunCallbacks, IPunObservable
{
    #region Variables (All)

    private InputManager inputManager;
    private InputAction playerMovement;

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
        inputManager = new InputManager();

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
        //CameraWork _cameraWork = gameObject.GetComponent<CameraWork>();
        //
        //if (_cameraWork)
        //{
        //    if (photonView.IsMine)
        //    {
        //        _cameraWork.OnStartFollowing();
        //    }
        //}
        //else
        //    Debug.LogError("CameraWork is not found", this);

#if UNITY_5_4_OR_NEWER
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
#endif

        if (playerUIPrefab)
        {
            PlayerUI _uiGo = Instantiate(playerUIPrefab);
            _uiGo.SetTarget(this);

        }
        else
            Debug.LogWarning("PlayerUI prefeb is missing", this);
    }

    #region Enable/Disable functions & subscribed functions
    public override void OnEnable()
    {
        playerMovement = inputManager.PlayerControls.Movement;
        playerMovement.Enable();

        inputManager.PlayerControls.Attack.performed += Attack;//subscribes the attack binding to an event
        inputManager.PlayerControls.Attack.Enable();
    }

#if UNITY_5_4_OR_NEWER
    public override void OnDisable()
    {
        playerMovement.Disable();

        inputManager.PlayerControls.Attack.performed -= Attack;//unsubscribes the attack binding
        inputManager.PlayerControls.Attack.Disable();

        base.OnDisable();
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }
#endif


    private void Attack(InputAction.CallbackContext obj)//The subscribed attack function
    {
        if (photonView.IsMine)
        {
            if (inputManager.PlayerControls.Attack.ReadValue<float>() > 0.0f)
            {
                //beam.SetActive(true);
                isFiring = true;
            }
            else
            {
                //beam.SetActive(false);
                isFiring = false;
            }
        }
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        //if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        //    return;

        if (!animator)
            return;

        movementVector = playerMovement.ReadValue<Vector2>();//reads the values of the keys being pressed from the input manager and stores them in movement vector

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

#if !UNITY_5_4_OR_NEWER

    private void OnLevelWasLoaded(int level)
    {
        this.CalledOnLevelWasLoaded(level);
    }
#endif

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

            Debug.Log(isFiring);
            Debug.Log(health);
        }
    }

    #endregion
}
