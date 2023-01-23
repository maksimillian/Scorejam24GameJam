
using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : MonoBehaviour, IWeaponSlot
{
    [SerializeField] public Vector2 dropShotSide;
    private Dictionary<IWeapon, float> _weaponsInRange;
    private IWeapon _weaponMounted;

    private void Awake()
    {
        _weaponsInRange = new Dictionary<IWeapon, float>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IWeapon weapon = other.GetComponent<IWeapon>();
        if (weapon != null && !_weaponsInRange.ContainsKey(weapon))
        {
            var distance = Vector3.Distance(transform.position, other.transform.position);
            _weaponsInRange.Add(weapon, distance);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IWeapon weapon = other.GetComponent<IWeapon>();
        if (weapon != null && _weaponsInRange.ContainsKey(weapon))
        {
            _weaponsInRange.Remove(weapon);
        }
    }

    private void FixedUpdate()
    {
        // update distances
        if (_weaponsInRange.Count != 0) UpdateDistances();
        
        // update weapon position
        if (_weaponMounted != null)
        {
            var weaponTransform = _weaponMounted.GetParent().transform;
            weaponTransform.position = transform.position;
            weaponTransform.eulerAngles = transform.eulerAngles;
        }
    }

    private void UpdateDistances()
    {
        var tempWeaponsInRange = new Dictionary<IWeapon, float>();
        foreach (var keyValuePair in _weaponsInRange)
        {
            var key = keyValuePair.Key;
            var distance = Vector3.Distance(transform.position, key.GetParent().transform.position);
            tempWeaponsInRange.Add(key, distance);
        }
        _weaponsInRange = tempWeaponsInRange;
    }

    public IWeapon GetFirstOrNothing()
    {
        if (_weaponsInRange.Count == 0) return null;
        
        // find the closest weapon
        var closestWeapon = new KeyValuePair<IWeapon, float>(null, float.MaxValue);
        foreach (var keyValuePair in _weaponsInRange)
        {
            closestWeapon = (keyValuePair.Value < closestWeapon.Value) ? keyValuePair : closestWeapon;
        }
        
        return closestWeapon.Key;
    }

    public void Mount(IWeapon weapon)
    {
        _weaponMounted = weapon;
    }

    public void Drop()
    {
        if (_weaponMounted == null) return;
            
        var weaponRB = _weaponMounted.GetParent().GetComponent<Rigidbody2D>();
        weaponRB.velocity = Vector2.zero;
        weaponRB.AddRelativeForce(dropShotSide * 50f, ForceMode2D.Impulse);
        _weaponMounted = null;
    }

    public bool IsEmpty() => _weaponMounted == null;

    public void Fire() => _weaponMounted?.Fire();
}