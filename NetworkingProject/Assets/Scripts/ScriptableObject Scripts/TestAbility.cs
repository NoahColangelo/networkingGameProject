using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "TestAbility")]
public class TestAbility : Ability
{
    public override void Activate(GameObject gameObject)
    {
        Debug.Log(_name + " has been activated");
    }

    public override void Active(GameObject gameObject)
    {
        Debug.Log(_name + " is now active");
    }

    public override void Cooldown(GameObject gameObject)
    {
        Debug.Log(_name + " is now on cooldown");
    }
}
