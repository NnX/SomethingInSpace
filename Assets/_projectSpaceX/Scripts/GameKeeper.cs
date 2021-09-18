using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameKeeper : MonoBehaviour
{
    private const int MaxEnemyWidth = 64;
    private const int MaxEnemyHeight = 44;
    private const int PaddingTop = 22;
    private const float StartMoveSpeed = 0.4f;
    private const float MoveSpeedDelta = 0.1f;
    private const float EnemyDownShift = 10f;
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
    private float _centerPositionX;
    private bool _isMoveRight = true;
    private float _moveSpeed = 0.4f;
    private float _screenWidth;
    private float _enemyRowWidth;

    private void Awake()
    {
        _enemyRowsList = new List<GameObject>();
        var rect = startTransform.rect;
        _topMaxPosition = rect.yMax;
        _maxLeftPosition = 0;
        _screenWidth = Screen.width;
    }

    private void Start()
    {
        var columnPosition = new Vector3(0, (_topMaxPosition * 2) - PaddingTop * 7, 0);
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
            _enemyRowWidth = columnPosition.x;
            var center = _screenWidth / 2 - _enemyRowWidth / 2;
            var centerPosition = new Vector3(center, rowLayoutPosition.y, 0);
            rowLayout.transform.position = centerPosition;
            _enemyRowsList.Add(rowLayout);
            columnPosition.y = columnPosition.y - (MaxEnemyHeight + spaceBetweenRows);
            columnPosition.x = 0;
        }
    }

    private void FixedUpdate()
    {
        DetectRowMoveDirectionChange();

        MoveEnemyRowsOnXAxis();
    }

    private void DetectRowMoveDirectionChange()
    {
        if (_enemyRowsList[0].transform.position.x - _moveSpeed < MaxEnemyWidth)
        {
            _isMoveRight = true;
            DownShiftEnemyRows();
        }
        else if (_enemyRowsList[0].transform.position.x + _moveSpeed > _screenWidth - _enemyRowWidth)
        {
            _isMoveRight = false;
            DownShiftEnemyRows();
        }
    }

    private void DownShiftEnemyRows()
    {
        foreach (var enemyRow in _enemyRowsList)
        {
            var position = enemyRow.transform.position;
            position.y -= EnemyDownShift;
            enemyRow.transform.position = position;
        }
        
        // TODO if last row has enemies and row position.y <= player position y = game over
    }

    private void MoveEnemyRowsOnXAxis()
    {
        foreach (var enemyRow in _enemyRowsList)
        {
            var direction = enemyRow.transform.position;
            if (_isMoveRight)
            {
                direction.x += _moveSpeed;
            }
            else
            {
                direction.x -= _moveSpeed;
            }

            enemyRow.transform.position = direction;
        }
    }
}