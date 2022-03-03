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

    //possibly temp
    public float directionDampTime = 0.25f;
    private bool isFiring = false;
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

    }

    void Start()
    {
        CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();

        if (!_cameraWork)
        {
            if (photonView.IsMine)
            {
                _cameraWork.OnStartFollowing();
            }
        }
        else
            Debug.LogError("CameraWork is not found", this);
    }

    #region Enable/Disable functions & subscribed functions
    private void OnEnable()
    {
        playerMovement = inputManager.PlayerControls.Movement;
        playerMovement.Enable();

        inputManager.PlayerControls.Attack.performed += Attack;//subscribes the attack binding to an event
        inputManager.PlayerControls.Attack.Enable();
    }

    private void OnDisable()
    {
        playerMovement.Disable();

        inputManager.PlayerControls.Attack.performed -= Attack;//unsubscribes the attack binding
        inputManager.PlayerControls.Attack.Disable();
    }

    private void Attack(InputAction.CallbackContext obj)//The subscribed attack function
    {
        if (photonView.IsMine)
        {
            if (inputManager.PlayerControls.Attack.ReadValue<float>() > 0.0f)
            {
                beam.SetActive(true);
                isFiring = true;
            }
            else
            {
                beam.SetActive(false);
                isFiring = false;
            }
        }
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            return;

        if (!animator)
            return;

        movementVector = playerMovement.ReadValue<Vector2>();//reads the values of the keys being pressed from the input manager and stores them in movement vector

        float h = movementVector.x;
        float v = movementVector.y;

        animator.SetFloat("Speed", h * h + v);
        animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);

        if (health <= 0.0f)
            GameManager.Instance.LeaveRoom();

    }

    #region Triggers and collisions

    private void OnTriggerEnter(Collider other)
    {
        if(!photonView.IsMine)
            return;

        if (other.name != "Beam")
            return;

        health -= 0.1f;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!photonView.IsMine)
            return;

        if (other.name != "Beam")
            return;

        health -= 0.1f * Time.deltaTime;
    }

    #endregion

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)//sending own data
        {
            stream.SendNext(isFiring);
            stream.SendNext(health);
        }
        else// receiving others data
        {
            this.isFiring = (bool)stream.ReceiveNext();
            this.health = (float)stream.ReceiveNext();
        }
    }

    #endregion
}
