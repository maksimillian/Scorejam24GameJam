
using System;
using UnityEngine;
using UnityEngine.Serialization;

public class MainShip : MonoBehaviour
{
    public float HP;
    public float Speed;
    public float DamageModifier;

    [SerializeField] public GameObject Left;
    [SerializeField] public GameObject Right;

    private IWeaponSlot _leftWeapon;
    private IWeaponSlot _rightWeapon;

    public float stoppingDistance = 0.1f;
    public float maxSpeed = 10f;
    public float acceleration = 1f;
    private Rigidbody2D rb;

    public GameLifeScope gls;

    private void Awake()
    {
        _leftWeapon = Left.GetComponent<IWeaponSlot>();
        _rightWeapon = Right.GetComponent<IWeaponSlot>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckMouseInput();
        // Get the mouse position in world space
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the direction towards the mouse position
        Vector2 direction = (mousePos - (Vector2)transform.position).normalized;

        // Calculate the distance between the ship and the mouse position
        float distance = Vector2.Distance(transform.position, mousePos);

        // Calculate the currentSpeed based on the distance between the ship and the mouse position
        float currentSpeed = Mathf.Clamp(distance * acceleration, 0f, maxSpeed);
        
        // Check if ship is close to the target position
        if (distance < stoppingDistance)
        {
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0f, 2f * Time.deltaTime),Mathf.Lerp(rb.velocity.y, 0f, 2f * Time.deltaTime));
        }
        else
        {
            // Move the ship towards the mouse position
            rb.AddForce(direction * currentSpeed, ForceMode2D.Force);
        
            // Limit the velocity to maxSpeed
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        }

        // Rotate the ship to face the mouse position
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    private void CheckMouseInput()
    {
        if (Input.GetMouseButtonDown(0)) LeftMouseClicked();
        if (Input.GetMouseButtonDown(1)) RightMouseClicked();
        if (Input.GetMouseButtonDown(2)) MiddleMouseClicked();
        
        if (Input.GetKeyDown(KeyCode.S)) gls.SetScore(10);
    }

    private void LeftMouseClicked()
    {
        if (_leftWeapon.IsEmpty())
        {
            GetNearbyWeapon(_leftWeapon);
        }
        else
        {
            _leftWeapon.Fire();
        }
    }
    
    private void RightMouseClicked()
    {
        if (_rightWeapon.IsEmpty())
        {
            GetNearbyWeapon(_rightWeapon);
        }
        else
        {
            _rightWeapon.Fire();
        }
    }
    
    private void MiddleMouseClicked()
    {
        _leftWeapon.Drop();
        _rightWeapon.Drop();
    }

    private void GetNearbyWeapon( IWeaponSlot weaponSlot)
    {
        // Check for nearby weapons
        var firstOrNothing = weaponSlot.GetFirstOrNothing();
        if (firstOrNothing == null)
            return;

        weaponSlot.Mount(firstOrNothing);
    }
}

public interface IWeaponSlot
    {
        IWeapon GetFirstOrNothing();
        void Mount(IWeapon weapon);
        bool IsEmpty();
        void Fire();
        void Drop();
    }

    public interface IUsbSlot
    {
        bool IsEmpty();
        void Drop();
    }

    public interface IWeapon
    {
        void Fire();
        GameObject GetParent();
    }
    
    public interface IUsb
    {
        void StartDownload();
        float GetLoadingProgress();
        bool IsAlreadyDownloaded();
        void CompleteDownload();
        GameObject GetParent();
    }