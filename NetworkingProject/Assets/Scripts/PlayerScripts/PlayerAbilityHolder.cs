using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityHolder : MonoBehaviour
{
    [Tooltip ("DO NOT CHANGE THE ARRAY SIZE FROM 4 YOU WILL CAUSE PROBLEMS!!!!")]
    public Ability[] _abilities = new Ability[4];

    private PlayerInput _playerInput;

    enum AbilityState
    {
        ready,
        active,
        cooldown
    }
    private AbilityState[] _abilityStates = new AbilityState[4];

    void Start()
    {
        _playerInput = gameObject.GetComponent<PlayerInput>();

        for (int i = 0; i < _abilityStates.Length; i++)
        {
            AbilityState temp = new AbilityState();
            _abilityStates[i] = temp;
            _abilityStates[i] = AbilityState.ready;
        }
    }

    void Update()
    {
        CheckAbilities(Time.deltaTime);
    }

    void CheckAbilities(float _dt)
    {
        for (int i = 0; i < _abilities.Length; i++)
        {
            switch (_abilityStates[i])
            {
                case AbilityState.ready:

                    if(_playerInput.GetAbilityToggle(i))
                    {
                        //_abilities[i].Activate(gameObject);
                        _abilityStates[i] = AbilityState.active;
                    }

                    break;
                case AbilityState.active:

                    if (_abilities[i]._activeTimer < _abilities[i]._activeTime)
                    {
                        _abilities[i]._activeTimer += _dt;
                        //_abilities[i].Active(gameObject);
                    }
                    else
                    {
                        _abilityStates[i] = AbilityState.cooldown;
                        _abilities[i]._activeTimer = 0.0f;
                    }

                    break;
                case AbilityState.cooldown:

                    if (_abilities[i]._cooldownTimer < _abilities[i]._cooldownTime)
                    {
                        _abilities[i]._cooldownTimer += _dt;
                        //_abilities[i].Cooldown(gameObject);
                    }
                    else
                    {
                        _abilityStates[i] = AbilityState.ready;
                        _abilities[i]._cooldownTimer = 0.0f;
                        _playerInput.SetAbilityToggle(false, i);//prevents any ability from immediately activating on ready state after cooldown
                    }

                    break;
            }

            
        }
    }
}
