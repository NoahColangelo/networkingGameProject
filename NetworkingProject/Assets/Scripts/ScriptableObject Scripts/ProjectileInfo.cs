using UnityEngine;

[CreateAssetMenu(fileName = "New ProjectileInfo" ,menuName = "ProjectileInfo")]
public class ProjectileInfo : ScriptableObject
{
    [field:SerializeField]
    public float _damage { get; private set; } = 1.0f;

    [field: SerializeField]
    public float _lifeTime { get; private set; } = 1.0f;

    [field: SerializeField]
    public float _speed { get; private set; } = 5.0f;

    [field: SerializeField]
    public float _fireRate { get; private set; } = 0.25f;
}
