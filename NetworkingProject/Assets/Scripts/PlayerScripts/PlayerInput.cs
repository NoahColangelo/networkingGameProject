using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{

    private InputManager inputManager;
    private InputAction playerMovement;

    private bool _basicAttack = false;
    private bool _ability1 = false;
    private bool _ability2 = false;
    private bool _ability3 = false;
    private bool _ultimate = false;

    private bool[] _abilityToggle = new bool[4];

    private void Awake()
    {
        inputManager = new InputManager();
        playerMovement = inputManager.PlayerControls.Movement;

        for (int i = 0; i < _abilityToggle.Length; i++)//sets all the toggles to false
        {
            _abilityToggle[i] = false;
        }
    }

    #region Enable/Disable functions
    public void OnEnable()
    {
        playerMovement.Enable();

        inputManager.PlayerControls.BasicAttack.performed += BasicAttack;//subscribes the BasicAttack binding to an event
        inputManager.PlayerControls.BasicAttack.Enable();

        inputManager.PlayerControls.Ability1.performed += Ability1;//subscribes the Ability1 binding to an event
        inputManager.PlayerControls.Ability1.Enable();

        inputManager.PlayerControls.Ability2.performed += Ability2;//subscribes the Ability2 binding to an event
        inputManager.PlayerControls.Ability2.Enable();

        inputManager.PlayerControls.Ability3.performed += Ability3;//subscribes the Ability3 binding to an event
        inputManager.PlayerControls.Ability3.Enable();

        inputManager.PlayerControls.Ultimate.performed += Ultimate;//subscribes the Ultimate binding to an event
        inputManager.PlayerControls.Ultimate.Enable();
    }

    public void OnDisable()
    {
        playerMovement.Disable();

        inputManager.PlayerControls.BasicAttack.performed -= BasicAttack;//unsubscribes the BasicAttack binding from the event
        inputManager.PlayerControls.BasicAttack.Disable();

        inputManager.PlayerControls.Ability1.performed -= Ability1;//unsubscribes the Ability1 from the event
        inputManager.PlayerControls.Ability1.Disable();

        inputManager.PlayerControls.Ability2.performed -= Ability2;//unsubscribes the Ability2 binding from the event
        inputManager.PlayerControls.Ability2.Disable();

        inputManager.PlayerControls.Ability3.performed -= Ability3;//unsubscribes the Ability3 binding from the event
        inputManager.PlayerControls.Ability3.Disable();

        inputManager.PlayerControls.Ultimate.performed -= Ultimate;//unsubscribes the Ultimate binding from the event
        inputManager.PlayerControls.Ultimate.Disable();
    }
    #endregion

    #region Subscribed Event functions
    private void BasicAttack(InputAction.CallbackContext obj)//The subscribed BasicAttack function
    {
        if (inputManager.PlayerControls.BasicAttack.ReadValue<float>() > 0.0f)
            _basicAttack = true;
        else
            _basicAttack = false;
    }

    private void Ability1(InputAction.CallbackContext obj)//The subscribed Ability1 function
    {
        _abilityToggle[0] = true;
        _ability1 = true;
    }

    private void Ability2(InputAction.CallbackContext obj)//The subscribed Ability2 function
    {
        _abilityToggle[1] = true;
        _ability2 = true;
    }

    private void Ability3(InputAction.CallbackContext obj)//The subscribed Ability3 function
    {
        _abilityToggle[2] = true;
        _ability3 = true;
    }

    private void Ultimate(InputAction.CallbackContext obj)//The subscribed Ultimate function
    {
        _abilityToggle[3] = true;
        _ultimate = true;
    }

    #endregion

    #region get and set functions
    public InputAction GetPlayerMovement()// Used for getting the playerMovement input values for moving the character or for animation
    {
        return playerMovement;
    }

    public bool GetAbilityToggle(int index)
    {
        return _abilityToggle[index];
    }
    
    public void SetAbilityToggle(bool set, int index)
    {
        _abilityToggle[index] = set;
    }

    public bool GetBasicAttack()//Basic Attack get
    {
        return _basicAttack;
    }

    public bool GetAbility1()// Ability 1 get and set false
    {
        return _ability1;
    }

    public void SetAbility1False()
    {
        _ability1 = false;
    }

    public bool GetAbility2()//Ability 2 get and set false
    {
        return _ability2;
    }

    public void SetAbility2False()
    {
        _ability2 = false;
    }

    public bool GetAbility3()//Ability 3 get and set false
    {
        return _ability3;
    }

    public void SetAbility3False()
    {
        _ability3 = false;
    }

    public bool GetUltimate()//Ultimate get and set false
    {
        return _ultimate;
    }

    public void SetUltimateFalse()
    {
        _ultimate = false;
    }

    #endregion
}
