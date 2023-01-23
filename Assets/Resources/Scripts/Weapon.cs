
using System;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon
{
    public BulletType type;
    public float damage = 10f;
    public void Fire(float damageMod)
    {
        Debug.Log($"piu {damage * damageMod}hp dealt");
    }

    public GameObject GetParent() => gameObject;
}