using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private EnemyFsm _enemyFsm = null;
    [SerializeField]
    private GameObject _shellPrefab;
    private SpriteRenderer image;

    internal float scoreForElim;
    internal Sprite enemySprite;
    internal Sprite bulletSprite;
    internal float maxHealth;
    internal float health;
    internal float healthRegenSpeed;
    internal float speed;
    internal float requiredDistanceToFire;
    internal float requiredDistanceToFeelComfortable;
    internal float distanceToPlayer;
    private float rotationDegree;
    private float spawnOffset;
    private float enemySize;
    private float enemyShellsSize;
    private float shootingRate;
    internal WeaponType weapon;
    
    private float shootTimer = 0f;
    private IShell enemyShell;

    private Rigidbody2D rb;
    private Dictionary<WeaponType, EnemySettings> EnemyBulletsInfo;

    public void Init(EnemyProfiler enemyProfiler)
    {
        scoreForElim = enemyProfiler.scoreForElim;
        enemySprite = enemyProfiler.enemySprite;
        bulletSprite = enemyProfiler.enemyBulletSprite;
        enemySize = enemyProfiler.enemySize;
        enemyShellsSize = enemyProfiler.enemyShellsSize;
        shootingRate = enemyProfiler.shootingRate;
        maxHealth = enemyProfiler.health;
        health = maxHealth;
        healthRegenSpeed = enemyProfiler.healthRegenSpeed;
        speed = enemyProfiler.speed;
        requiredDistanceToFire = enemyProfiler.requiredDistanceToFire;
        requiredDistanceToFeelComfortable = enemyProfiler.requiredDistanceToFeelComfortable;
        rotationDegree = enemyProfiler.rotationDegree;
        spawnOffset = enemyProfiler.spawnOffset;
        weapon = enemyProfiler.weapon;
        
        image.sprite = enemySprite;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        image = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        EnemyBulletsInfo = new Dictionary<WeaponType, EnemySettings>
        {
            { 
                WeaponType.Cookies, new () 
                {
                    shellType = typeof(CookieShellBehavior), 
                    behaviorStates = new IFsmState[] {
                        new CookieFlyToPlayerState(this),
                        new CookieStopAndShootState(this),
                        new CookieFlyAwayState(this),
                        new CookieDestroyState(this)
                    }
                    
                }
            },
            {
                WeaponType.Stars, new () 
                {
                    shellType = typeof(StarShellBehavior), 
                    behaviorStates = new IFsmState[] {
                        new CookieFlyToPlayerState(this),
                        new CookieStopAndShootState(this),
                        new CookieFlyAwayState(this),
                        new CookieDestroyState(this) 
                    }
                }
            },
            {
                WeaponType.Bombers, new () 
                {
                    shellType = typeof(HorseShellBehavior), 
                    behaviorStates = new IFsmState[] {
                        new CookieFlyToPlayerState(this),
                        new CookieStopAndShootState(this),
                        new CookieFlyAwayState(this),
                        new CookieDestroyState(this) 
                    }
                }
            },
            {
                WeaponType.Explosion, new () 
                {
                    shellType = typeof(ExplosionShellBehavior), 
                    behaviorStates = new IFsmState[] {
                        new BomberFlyToPlayerState(this),
                        new BomberExplodeState(this),
                        new BomberDestroyState(this) 
                    }
                }
            },
        };
        
        IFsmState[] allStates = EnemyBulletsInfo[weapon].behaviorStates;
        // starts from play
        _enemyFsm = new EnemyFsm(allStates[0], allStates);
        _enemyFsm.Start();
    }

    private void Update()
    {
        CalculateDistance();
        _enemyFsm.UpdateTick();
    }

    private void CalculateDistance()
    {
        var playerPos = MainShip.Instance.transform.position;
        distanceToPlayer = Vector3.Distance(playerPos, transform.position);
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }

    public void MoveInTheDirection(Vector2 direction)
    {
        float currentSpeed = Mathf.Clamp(distanceToPlayer * 0.1f, 0f, speed);
        rb.AddForce(direction * currentSpeed, ForceMode2D.Force);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, speed);
    }

    public void RotateInTheDirectionOf(Vector2 direction)
    {
        // Rotate the ship to face the mouse position
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - rotationDegree);
    }
    
    public void ShootIfPossible()
    {
        if (distanceToPlayer > requiredDistanceToFire) return;
        
        shootTimer += Time.deltaTime; // Increase the spawn timer by the time since the last frame

        if (shootTimer >= shootingRate)
        {
            shootTimer = 0f; // Reset the spawn timer
            
            Shoot();
        }
    }

    public void Shoot()
    {
        var playerPos = MainShip.Instance.transform.position;
        var targetDirection = Vector3.Normalize(playerPos - transform.position);
            
        var bullet = Instantiate(_shellPrefab, transform.position + targetDirection * spawnOffset, transform.rotation);
        bullet.transform.localScale = new Vector3(enemyShellsSize, enemyShellsSize, enemyShellsSize);
        var shellComponent = EnemyBulletsInfo[weapon];

        var bulletComponent = bullet.AddComponent(shellComponent.shellType);
        IShell shell = bulletComponent as IShell;
        shell.Init(bulletSprite);
        shell.Fire(targetDirection);
    }
}

public class EnemySettings
{
    public Type shellType;
    public IFsmState[] behaviorStates;
}

