using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject blowFx;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Missle"))
        {
            var transform1 = transform;
            var blowFX = Instantiate(blowFx, transform1.position, Quaternion.identity, transform1);
            blowFX.transform.SetParent(transform.parent);
            gameObject.SetActive(false);
            other.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}