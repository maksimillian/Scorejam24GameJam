using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Enemy", menuName = "Enemies")]
public class EnemyProfiler : ScriptableObject
{
    public Sprite enemySprite;
    public Sprite enemyBulletSprite;
    public float enemySize;
    public float enemyShellsSize;
    public float shootingRate;
    public float health;
    public float healthRegenSpeed;
    public float speed;
    public float requiredDistanceToFire;
    public float requiredDistanceToFeelComfortable;
    public float rotationDegree;
    public float spawnOffset;
    public WeaponType weapon;
}

[System.Serializable]
public enum WeaponType
{
    None,
    Cookies,
    Stars,
    Bombers,
    Explosion,
    Shield,
    Tesla,
    Flame, 
    Heal
}
