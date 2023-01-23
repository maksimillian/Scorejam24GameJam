
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class Usb : MonoBehaviour, IUsb
{
    [SerializeField] public float value = 0.2f;
    [SerializeField] public float timeToDownload = 10f;
    [SerializeField] public UsbEffect effect;
    private float _startDownloadingTime;
    private bool _used;

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

    public Tuple<UsbEffect, float> CompleteDownload()
    {
        _used = true;
        return Tuple.Create<UsbEffect, float>(effect, value);
    }

    public bool IsAlreadyDownloaded() => _used;

    public GameObject GetParent() => gameObject;
}