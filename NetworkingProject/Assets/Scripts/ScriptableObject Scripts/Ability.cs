using UnityEngine;

public class Ability : ScriptableObject
{
    public string _name;
    public float _cooldownTime;
    public float _activeTime;

    [HideInInspector]
    public float _cooldownTimer = 0.0f;
    [HideInInspector]
    public float _activeTimer = 0.0f;

    public virtual void Activate(GameObject gameObject) { }
    public virtual void Active(GameObject gameObject) { }
    public virtual void Cooldown(GameObject gameObject) { }
}
