using UnityEngine;

public class Missile : MonoBehaviour
{
    private const float PlayerMissileSpeed = 180f;
    private const float EnemyMissileSpeed = 120f;
    [SerializeField] private Rigidbody2D rigidBody;
    private float _speed = 180f;
    private float _maxPositionX;

    private Vector2 _moveDirection;
    private bool _isEnemyMissile;
    
    public void SetEnemyMissileParams()
    {
        _moveDirection = Vector2.down;;
        _speed = EnemyMissileSpeed;
        _isEnemyMissile = true;
    }

    private void SetPlayerMissileParams()
    {
        _maxPositionX = Screen.height;
        _moveDirection = transform.up;
        _speed = PlayerMissileSpeed;
    }
    
    private void Awake()
    {
        SetPlayerMissileParams();
    }
    
    private void FixedUpdate()
    {
        rigidBody.velocity = _moveDirection * _speed;
        if (transform.position.y > _maxPositionX || transform.position.y < 0)
        {
            if (_isEnemyMissile)
            {
                Destroy(gameObject);
            }
            
            gameObject.SetActive(false);
        }
    }
}
