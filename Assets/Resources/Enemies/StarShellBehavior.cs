using UnityEngine;

public class StarShellBehavior : MonoBehaviour, IShell, IDamagable
{
    private SpriteRenderer image;
    
    private float health = 5;
    private float damage = 7;
    private float speed = 25;

    private float timeBeforeDestroy = 10f;
    private float timePassed = 0;

    private Vector2 targetDirection;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        image = GetComponent<SpriteRenderer>();
    }
    
    private void Update()
    {
        var playerPos = MainShip.Instance.transform.position;
        var distanceToPlayer = Vector3.Distance(playerPos, transform.position);
        Vector2 direction = ((Vector2)playerPos - (Vector2)transform.position).normalized;
        
        float currentSpeed = speed;
        rb.AddForce(direction * currentSpeed, ForceMode2D.Force);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, speed);
        timePassed += Time.deltaTime;
        if (timePassed > timeBeforeDestroy) SelfDestroy();
    }

    public void Init(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void Fire(Vector3 targetDirection)
    {
        this.targetDirection = targetDirection;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("OnCollisionEnter()");
        IDamagable damagable = col.gameObject.GetComponent<IDamagable>();
        if(damagable != null && col.gameObject.name == "Ship" || col.gameObject.name == "ShipShell")
        {
            damagable.TakeDamage(damage);
            SelfDestroy();
        }
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            SelfDestroy();
        }
    }
}
