using System;
using System.Collections.Generic;
using UnityEngine;

namespace _projectSpaceX.Scripts
{
    public class Player : MonoBehaviour
    {
        private const float SpawnMissileDelay = 1f;
        private const int StartPoolSize = 6;
        private const float PlayerMoveSpeed = 15f;
        public event Action OnDamageReceived;

        [SerializeField] private GameObject missilePrefab;
        [SerializeField] private Transform missileSocket;

        private float _spawnMissileTimer;
        private Touch _touch;
        private float _currentTouchX;
        private float _touchDelta;
        private Vector3 _playerPosition;

        private List<GameObject> _missilePool;

        private void Awake()
        {
            _playerPosition = transform.position;
            _currentTouchX = _playerPosition.x;
            _missilePool = new List<GameObject>();
        }

        private void Start()
        {
            for (var i = 0; i < StartPoolSize; i++)
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
            _playerPosition.x = Mathf.Lerp(_playerPosition.x,  _currentTouchX, Time.deltaTime * PlayerMoveSpeed);
            transform.position = _playerPosition;
        }

        private void SpawnMissile()
        {
            if (_spawnMissileTimer < SpawnMissileDelay)
            {
                return;
            }
            foreach (var o in _missilePool)
            {
                if (o.activeInHierarchy)
                {
                    continue;
                }
                o.SetActive(true);
                o.transform.position = missileSocket.position;
                _spawnMissileTimer = 0;
                return;
            }
            
            InitNewMissile(true);
            _spawnMissileTimer = 0;
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
            if(other.TryGetComponent<Missile>(out _))
            {
                Destroy(other.gameObject);
                OnDamageReceived?.Invoke();
            } 
        }
    }
}