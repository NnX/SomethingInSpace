using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event Action OnDamageReceived;
    private const int StartPoolSize = 6;

    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform missileSocket;
    private float _spawnMissileDelay = 1f;
    private float _spawnMissileTimer;

    private Touch _touch;
    private float _currentTouchX;
    private float _touchDelta;
    private Vector3 _playerPosition;

    private List<GameObject> _missilePool;
    private float _playerMoveSpeed = 15f;

    private void Awake()
    {
        _playerPosition = transform.position;
        _currentTouchX = _playerPosition.x;
        _missilePool = new List<GameObject>();
    }

    private void Start()
    {
        for (int i = 0; i < StartPoolSize; i++)
        {
            InitNewMissile(false);
        }
    }

    private void Update()
    {
        _spawnMissileTimer += Time.deltaTime;
        SpawnMissile();
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);
            if (_touch.phase == TouchPhase.Moved)
            {
                _currentTouchX = _touch.position.x;
            }
        }
        _playerPosition.x = Mathf.Lerp(_playerPosition.x,  _currentTouchX, Time.deltaTime * _playerMoveSpeed);
        transform.position = _playerPosition;
    }

    private void SpawnMissile()
    {
        if (_spawnMissileTimer >= _spawnMissileDelay)
        {
            for (var i = 0; i < _missilePool.Count; i++)
            {
                var o = _missilePool[i];
                if (!o.activeInHierarchy)
                {
                    o.SetActive(true);
                    o.transform.position = missileSocket.position;
                    _spawnMissileTimer = 0;
                    return;
                }
            }
            
            InitNewMissile(true);
            _spawnMissileTimer = 0;
        }
    }

    private void InitNewMissile(bool isActive)
    {
        var missile = Instantiate(missilePrefab, missileSocket.position, Quaternion.identity, missileSocket);
        missile.transform.SetParent(transform.parent);
        missile.gameObject.SetActive(isActive);
        _missilePool.Add(missile);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var missile = other.GetComponent<Missile>();
        if (missile != null)
        {
            Destroy(other.gameObject);
            OnDamageReceived?.Invoke();
        }
    }
}