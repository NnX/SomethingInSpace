using UnityEngine;

public class EnemyRow : MonoBehaviour
{
    private bool _isLastRow;
    private float _spawnMissileDelay = 3f;
    private float _spawnMissileTimer;
    private int _childCount;
    void Update()
    {
        _spawnMissileTimer += Time.deltaTime;
        if (_spawnMissileTimer > _spawnMissileDelay & _isLastRow)
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
        transform.GetChild(randomIndex).GetComponent<Enemy>()?.Shoot();
        _spawnMissileTimer = 0;
    }

    public void SetLastRow(bool isLastRow)
    {
        _isLastRow = isLastRow;
    }
}
