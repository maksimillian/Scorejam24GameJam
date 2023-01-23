using System;
using UnityEngine;

public class CookieShellBehavior : MonoBehaviour, IShell, IDamagable
{
    private SpriteRenderer image;
    
    private float health = 1;
    private float damage = 1;
    private float speed = 80;

    private float timeBeforeDestroy = 10f;
    private float timePassed = 0;
    
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        image = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > timeBeforeDestroy) SelfDestroy();
    }

    public void Init(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void Fire(Vector3 targetDirection)
    {
        rb.AddForce(targetDirection * speed, ForceMode2D.Impulse);
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
