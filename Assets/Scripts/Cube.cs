using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Renderer))]

public class Cube : MonoBehaviour
{
    public event Action<GameObject> GotReleased;

    private Coroutine _releaseWaiter;
    private Rigidbody _rigidbody;
    private Renderer _renderer;

    private float _timeBeforeRelease;
    private float _maxReleaseTime;
    private float _minReleaseTime;

    private bool _isDeactivated;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();

        _maxReleaseTime = 5f;
        _minReleaseTime = 2f;

        _isDeactivated = false;      
    }

    private void OnEnable()
    {
        _isDeactivated = false;

        _rigidbody.velocity = Vector3.zero;
        _renderer.material.color = Color.white;
    }

    private void OnDisable()
    {
        if (_releaseWaiter != null)
        {
            StopCoroutine(_releaseWaiter);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Platform>() != null && _isDeactivated == false)
        {
            _isDeactivated = true;

            _renderer.material.color = UnityEngine.Random.ColorHSV();

            _timeBeforeRelease = UnityEngine.Random.Range(_minReleaseTime, _maxReleaseTime);

            RestartWaiting();
        }
    }

    private void RestartWaiting()
    {
        _releaseWaiter = StartCoroutine(WaitRelease());
    }

    private IEnumerator WaitRelease()
    {
        var wait = new WaitForSecondsRealtime(_timeBeforeRelease);

        while (true)
        {
            yield return wait;

            GotReleased?.Invoke(gameObject);
        }
    }
}
