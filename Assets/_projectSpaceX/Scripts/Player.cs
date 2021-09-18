using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform missileSocket;
    private float _spawnMissileDelay = 1f;
    private float _spawnMissileTimer;
    private float _missileSpeed = 5f;

    private Touch _touch;
    private float _currentTouchX;
    private float _touchDelta;
    private Vector3 _playerPosition;


    private void Awake()
    {
        _playerPosition = transform.position;
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
            if (_touch.phase == TouchPhase.Began)
            {
                _currentTouchX = _touch.position.x;
            } 
            if (_touch.phase == TouchPhase.Moved)
            {
                _touchDelta  = _touch.position.x - _currentTouchX;
            }
            if (_touch.phase == TouchPhase.Ended || _touch.phase == TouchPhase.Canceled)
            {
                _touchDelta = 0;
            }
        }
        _playerPosition.x = Mathf.Lerp(_playerPosition.x, _playerPosition.x + _touchDelta, Time.deltaTime);
        transform.position = _playerPosition;
    }

    private void SpawnMissile()
    {
        if (_spawnMissileTimer >= _spawnMissileDelay)
        {
            var missile = Instantiate(missilePrefab, missileSocket.position, Quaternion.identity, missileSocket);
            missile.transform.SetParent(transform.parent);
            _spawnMissileTimer = 0;
        }
    }
}