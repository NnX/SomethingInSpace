using UnityEngine;

namespace _projectSpaceX.Scripts
{
    public class EnemyRow : MonoBehaviour
    {
        private const float SpawnMissileDelay = 3f;
        
        private bool _isLastRow;
        private float _spawnMissileTimer;
        private int _childCount;

        private void Update()
        {
            _spawnMissileTimer += Time.deltaTime;
            if (_spawnMissileTimer > SpawnMissileDelay & _isLastRow)
            {
                _childCount = transform.childCount;
                if (_childCount > 0)
                {
                    MakeRandomEnemyShot();    
                }
            }
        }

        private void MakeRandomEnemyShot()
        {
            var randomIndex = Random.Range(0, _childCount);
            if (transform.GetChild(randomIndex).TryGetComponent<Enemy>(out var enemy))
            {
                enemy.Shoot();
            }
            _spawnMissileTimer = 0;
        }

        public void SetLastRow(bool isLastRow)
        {
            _isLastRow = isLastRow;
        }
    }
}
