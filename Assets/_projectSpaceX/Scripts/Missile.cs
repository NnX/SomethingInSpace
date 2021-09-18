using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;
    private float _speed = 180f;

    private void FixedUpdate()
    {
        rigidBody.velocity = transform.up * _speed;
    }
}
