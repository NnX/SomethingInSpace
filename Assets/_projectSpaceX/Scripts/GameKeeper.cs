using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameKeeper : MonoBehaviour
{

    private const int MaxEnemyWidth = 64;
    private const int MaxEnemyHeight = 44;
    private const int PaddingTop = 22;
    [SerializeField] private int enemyColumnsAmount;
    [SerializeField] private int enemyRowsAmount;
    [SerializeField] private float spaceBetweenEnemies;
    [SerializeField] private float spaceBetweenRows;
    [SerializeField] private List<Enemy> enemies;
    [SerializeField] private GameObject enemyRowPrefab;
    [SerializeField] private RectTransform startTransform;

    private List<GameObject> _enemyRowsList;
    private float _topMaxPosition;
    private float _maxLeftPosition;
    private float _maxRightPosition;
    private float _centerPositionX;
    private bool _isMoveRight;

    private void Awake()
    {
        _enemyRowsList = new List<GameObject>();
        var rect = startTransform.rect;
        _topMaxPosition = rect.yMax;
        _maxLeftPosition = 0;
        _maxRightPosition = Screen.width;
    }

    private void Start()
    {
        var columnPosition = new Vector3(0,(_topMaxPosition * 2) - PaddingTop,0);
        var rowLayoutPosition = new Vector3(0, _topMaxPosition * 2, 0);
        for (int i = 0; i < enemyRowsAmount; i++)
        {
            var rowLayout = Instantiate(enemyRowPrefab, rowLayoutPosition, Quaternion.identity, startTransform);
            var randomEnemyIndex = Random.Range(0, enemies.Count);
            for (int j = 0; j < enemyColumnsAmount; j++)
            {
                var enemy = Instantiate(enemies[randomEnemyIndex], columnPosition, quaternion.identity, startTransform);
                enemy.gameObject.transform.SetParent(rowLayout.transform);
                columnPosition.x = columnPosition.x + ((RectTransform)enemy.transform).sizeDelta.x + spaceBetweenEnemies;
            }

            var center = Screen.width / 2 - columnPosition.x / 2;
            var centerPosition = new Vector3(center, rowLayoutPosition.y, 0);
            rowLayout.transform.position = centerPosition;
            _enemyRowsList.Add(rowLayout);
            columnPosition.y = columnPosition.y - (MaxEnemyHeight + spaceBetweenRows);
            columnPosition.x = 0;
        }
    }

    private void FixedUpdate()
    {

        foreach (var enemyRow in _enemyRowsList)
        {
            var direction = enemyRow.transform.position;
            if (_isMoveRight)
            {
                direction.x += 2;
            }
            else
            {
                direction.x -= 2;
                
            }
            enemyRow.transform.position = direction;
        }
    }
}
