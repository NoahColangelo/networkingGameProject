using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private Text playerNameText;

    [SerializeField]
    private Slider playerHealthSlider;

    private PlayerControls target;

    [SerializeField]
    private Vector3 screenOffset = new Vector3(0f, 30f, 0f);

    private float characterControllerHeight = 0f;
    private Transform targetTransform;
    private Renderer targetRenderer;
    private CanvasGroup _canvasGroup;
    private Vector3 targetPosition;

    public int yeet;

    #endregion

    #region MonoBehaviour CallBacks

    void Awake()
    {
        transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);

        _canvasGroup = this.GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if(!target)
        {
            Destroy(this.gameObject);
            Debug.Log("destroyed");
            return;
        }

        if (playerHealthSlider)
            playerHealthSlider.value = target.health;
    }

    void LateUpdate()
    {
        if (targetRenderer)
            _canvasGroup.alpha = targetRenderer.isVisible ? 1f : 0f;

        if(targetTransform)
        {
            targetPosition = targetTransform.position;
            targetPosition.y += characterControllerHeight;
            transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
        }
    }

    #endregion

    #region Public Methods

    public void SetTarget(PlayerControls _target)
    {
        if(!_target)
        {
            Debug.LogError("Player target is missing", this);
            return;
        }

        target = _target;

        targetTransform = target.GetComponent<Transform>();
        targetRenderer = target.GetComponent<Renderer>();

        CharacterController characterController = _target.GetComponent<CharacterController>();

        if (characterController)
            characterControllerHeight = characterController.height;

        if (playerNameText)
            playerNameText.text = target.photonView.Owner.NickName;
    }

    #endregion

}
