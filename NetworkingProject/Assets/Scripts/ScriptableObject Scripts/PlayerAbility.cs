using UnityEngine;

public class PlayerAbility : ScriptableObject
{
    public string _name;
    public float _cooldownTime;
    public float _activeTime;

    public virtual void Activate(GameObject gameObject) { }
}
