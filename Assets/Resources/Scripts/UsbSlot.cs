
using System;
using System.Collections.Generic;
using UnityEngine;

public class UsbSlot : MonoBehaviour, IUsbSlot
{
    [SerializeField] public MainShip mainShip;
    [SerializeField] public Vector2 dropShotSide;
    public IUsb _connectedUsb;
    private Dictionary<IUsb, float> _usbsInRange;

    private void Awake()
    {
        _usbsInRange = new Dictionary<IUsb, float>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IUsb usb = other.GetComponent<IUsb>();
        if (usb != null && !_usbsInRange.ContainsKey(usb) && !usb.IsAlreadyDownloaded())
        {
            var distance = Vector3.Distance(transform.position, other.transform.position);
            _usbsInRange.Add(usb, distance);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IUsb usb = other.GetComponent<IUsb>();
        if (usb != null && _usbsInRange.ContainsKey(usb))
        {
            _usbsInRange.Remove(usb);
        }
    }

    private void UpdateDistances()
    {
        var tempUsbsInRange = new Dictionary<IUsb, float>();
        foreach (var keyValuePair in _usbsInRange)
        {
            var key = keyValuePair.Key;
            var distance = Vector3.Distance(transform.position, key.GetParent().transform.position);
            tempUsbsInRange.Add(key, distance);
        }
        _usbsInRange = tempUsbsInRange;
    }
    
    private IUsb TryConnect()
    {
        if (_usbsInRange.Count == 0) return null;
        
        // find the closest usb
        var closestUsb = new KeyValuePair<IUsb, float>(null, float.MaxValue);
        foreach (var keyValuePair in _usbsInRange)
        {
            closestUsb = (keyValuePair.Value < closestUsb.Value) ? keyValuePair : closestUsb;
        }
        
        return closestUsb.Key;
    }

    private void Update()
    {
        // update distances
        if (_usbsInRange.Count != 0) UpdateDistances();

        if (_connectedUsb == null)
        {
            _connectedUsb = TryConnect();
            if (_connectedUsb == null)
            {
                return;
            }
            _connectedUsb.StartDownload();
        }

        _connectedUsb.GetParent().transform.position = transform.position;
        _connectedUsb.GetParent().transform.rotation = transform.rotation;
        
        var loadingProgress = _connectedUsb.GetLoadingProgress();

        Debug.Log($"{loadingProgress * 100}%");

        if (Math.Abs(loadingProgress - 1f) < 0.003f)
        {
            var completeDownload = _connectedUsb.CompleteDownload();
            var effect = completeDownload.Item1;
            var value = completeDownload.Item2;
            mainShip.ApplyModifier(effect, value);
            Drop();
        }
    }

    public bool IsEmpty() => _connectedUsb != null;
    
    public void Drop()
    {
        if (_connectedUsb == null) return;
            
        var rb = _connectedUsb.GetParent().GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.AddRelativeForce(dropShotSide * 50f, ForceMode2D.Impulse);

        _usbsInRange.Remove(_connectedUsb);
        _connectedUsb = null;
    }
}