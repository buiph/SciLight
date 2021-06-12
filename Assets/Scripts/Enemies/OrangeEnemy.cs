using UnityEngine;

public class OrangeEnemy : EnemyAI
{
    public float shotDelay;

    protected override float _fireRate
    {
        get
        {
            return shotDelay;
        }
    }

    internal void Awake()
    {
        // So that enemies will not be destroyed when colliding with each other
        Physics.IgnoreLayerCollision(8, 0, true);
    }

    protected void OnEnable()
    {
        timeStamp = Time.time;
    }

    /// <summary>
    /// Do nothing
    /// </summary>
    protected override void FriendlyTrigger() {}
}
