
using System;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon
{
    public float damage = 10f;
    public void Fire()
    {
        Debug.Log($"piu {damage}hp dealt");
    }

    public GameObject GetParent() => gameObject;
}