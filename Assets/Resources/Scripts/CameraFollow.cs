using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject ship;
    public float smoothTime = 0.3f;
    public float zoomSpeed = 1;
    public float minZoom = 1;
    public float maxZoom = 5;
    public Vector2 followZoneSize = new Vector2(5, 5); // width and height of the zone that the camera will follow the target in
    public Vector2 followZoneOffset = new Vector2(0, 0); // offset of the zone from the target's position

    private Vector3 velocity = Vector3.zero;
    private Camera cam;
    private Vector2 followZoneMin;
    private Vector2 followZoneMax;
    private Rigidbody2D _rigidbody;
    private float targetZoom;

    private void Awake()
    {
        targetZoom = minZoom;
    }

    void Start()
    {
        _rigidbody = ship.GetComponent<Rigidbody2D>();
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        var position = transform.position;
        followZoneMin = (Vector2)position + followZoneOffset - followZoneSize * 0.5f;
        followZoneMax = (Vector2)position + followZoneOffset + followZoneSize * 0.5f;
        
        Vector3 targetPos = ship.transform.position;
        targetPos.z = position.z;
        
        // Check if the ship is within the follow zone
        if (ship.transform.position.x > followZoneMin.x && ship.transform.position.x < followZoneMax.x)
        {
            targetPos.x = position.x;
            // Ship is within the follow zone, do not move the camera
        }
        if (ship.transform.position.y > followZoneMin.y && ship.transform.position.y < followZoneMax.y)
        {
            targetPos.y = position.y;
            // Ship is within the follow zone, do not move the camera
        }

        var newPosition = Vector3.Lerp(position, targetPos, smoothTime * Time.deltaTime);
        transform.position = newPosition;

        // Calculate the new zoom level based on the ship's speed
        var magnitudePrc = _rigidbody.velocity.magnitude / 20;
        cam.orthographicSize = Mathf.Lerp( cam.orthographicSize, minZoom + (maxZoom-minZoom) * magnitudePrc, zoomSpeed * Time.deltaTime);
    }
}