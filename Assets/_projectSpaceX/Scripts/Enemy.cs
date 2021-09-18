using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject blowFx;
    [SerializeField] private Transform missileSocket;
    [SerializeField] private GameObject missilePrefab;
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

    public void Shoot()
    {
        var missile = Instantiate(missilePrefab, missileSocket.position, Quaternion.identity, missileSocket);
        missile.GetComponent<Missile>().SetEnemyMissileParams();
        missile.transform.SetParent(transform.parent.transform.parent);
    }
}