using System;
using UnityEngine;

namespace _projectSpaceX.Scripts
{
    public class Enemy : MonoBehaviour
    {
        public event Action OnHit;
    
        [SerializeField] private GameObject blowFx;
        [SerializeField] private Transform missileSocket;
        [SerializeField] private GameObject missilePrefab;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!other.TryGetComponent<Missile>(out _))
            {
                return;
            } 
            
            var transform1 = transform;
            var blowFX = Instantiate(blowFx, transform1.position, Quaternion.identity, transform1);
            blowFX.transform.SetParent(transform.parent);
            OnHit?.Invoke();
            gameObject.SetActive(false);
            other.gameObject.SetActive(false);
            Destroy(gameObject);
        }

        public void Shoot()
        {
            var missileObject = Instantiate(missilePrefab, missileSocket.position, Quaternion.identity, missileSocket);
            if (missileObject.TryGetComponent<Missile>(out var missile))
            {
                missile.SetEnemyMissileParams();
            }
            missileObject.transform.SetParent(transform.parent.transform.parent);
        }
    }
}