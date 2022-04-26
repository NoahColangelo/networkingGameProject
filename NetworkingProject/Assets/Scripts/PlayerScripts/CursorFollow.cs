using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorFollow : MonoBehaviour
{
    public GameObject CursorTarget;

    private GameObject _cursorTarget;
    private PlayerInput _playerInput;

    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();

        _cursorTarget = Instantiate(CursorTarget);
    }

    // Update is called once per frame
    void Update()
    {
        _cursorTarget.transform.position = new Vector3(_playerInput.GetMousePosition().x, 0.01f, _playerInput.GetMousePosition().z); ;
    }
}
