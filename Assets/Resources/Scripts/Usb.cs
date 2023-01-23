
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class Usb : MonoBehaviour, IUsb
{
    [SerializeField] public float timeToDownload;
    [SerializeField] public UsbEffect effect;
    private float _startDownloadingTime;
    private bool _used;


    public enum UsbEffect
    {
        Speed,
        Damage,
        Health
    }
    
    public void StartDownload()
    {
        if (_startDownloadingTime != 0) return;
        _startDownloadingTime = Time.realtimeSinceStartup;
    }

    public float GetLoadingProgress()
    {
        if (_startDownloadingTime == 0) return 0;
        return Mathf.Clamp((Time.realtimeSinceStartup - _startDownloadingTime) / timeToDownload, 0f, 1f);
    }

    public void CompleteDownload()
    {
        _used = true;
        Debug.Log($"You have increased {effect}");
    }

    public bool IsAlreadyDownloaded() => _used;

    public GameObject GetParent() => gameObject;
}