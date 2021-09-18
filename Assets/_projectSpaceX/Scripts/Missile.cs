using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;
    private float _speed = 180f;
    private float _maxPositionX;

    private void Awake()
    {
        _maxPositionX = Screen.height;
    }

    private void FixedUpdate()
    {
        rigidBody.velocity = transform.up * _speed;
        if (transform.position.y > _maxPositionX)
        {
            gameObject.SetActive(false);
        }
    }
}
