using UnityEngine;

public class RedEnemyAI : EnemyAI
{
    public float moveSpeed;

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

        // Generate a random direction to move in
        movementVector.x = Random.Range(1, -2);
        if (movementVector.x == 0)
        {
            movementVector.x = -1;
        }
        movementVector.y = Random.Range(0.2f, -0.2f);
    }

    protected void OnEnable()
    {
        timeStamp = Time.time;
    }

    void FixedUpdate()
    {
        Vector2 facingDir = new Vector2(
            MainCharControl.mainCharPos.x - transform.position.x,
            MainCharControl.mainCharPos.y - transform.position.y
        );
        transform.up = facingDir;

        transform.Translate(movementVector.normalized * moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Change movement when colliding with another enemy
    /// </summary>
    protected override void FriendlyTrigger()
    {
        movementVector.x = -movementVector.x;
        movementVector.y = Random.Range(0.2f, -0.2f);
    }
}
