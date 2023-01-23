using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionShellBehavior : MonoBehaviour, IShell
{
    private float damage = 10;

    public void Init(Sprite sprite)
    {
        CircleCollider2D[] cols = gameObject.GetComponents<CircleCollider2D>();
        foreach (CircleCollider2D col in cols)
        {
            Debug.Log($"collider - {col.gameObject}");
            if (!col.isTrigger) Destroy(col);
        }
    }

    public void Fire(Vector3 targetDirection)
    {
        //boom
        Debug.Log("boom");
        SelfDestroy();
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
}
