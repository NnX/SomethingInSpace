using System.Collections.Generic;
using System.Globalization;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameKeeper : MonoBehaviour
{
    private const int MaxEnemyWidth = 64;
    private const int MaxEnemyHeight = 44;
    private const int PaddingTop = 154;
    private const float EnemyDownShift = 15f;
    private const float EnemyRowMoveStartSpeed = 0.4f;
    private const float EnemyRowIncreaseSpeed = 0.2f;
    [SerializeField] private int playerLiveAmount;
    [SerializeField] private int enemyColumnsAmount;
    [SerializeField] private int enemyRowsAmount;
    [SerializeField] private float spaceBetweenEnemies;
    [SerializeField] private float spaceBetweenRows;
    [SerializeField] private List<Enemy> enemies;
    [SerializeField] private GameObject enemyRowPrefab;
    [SerializeField] private RectTransform startTransform;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text hiScoreText;
    [SerializeField] private Text livesText;
    [SerializeField] private Player player;
    [SerializeField] private float playerMoveSpeed = 0.4f;
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private Button gameSettingsButton;
    
    private List<GameObject> _enemyRowsList;
    private float _centerPositionX;
    private bool _isMoveRight = true;
    private float _screenWidth;
    private float _screenHeight;
    private float _enemyRowWidth;
    private int _currentLivesAmount;
    private int _currentScore;
    private int _hiScoreAmount;
    private float _enemyRowMoveSpeed;
    private SaveKeeper _saveKeeper;
    
    private void Awake()
    {
        _enemyRowsList = new List<GameObject>();
        _screenWidth = Screen.width;
        _screenHeight = Screen.height;
        _saveKeeper = new SaveKeeper();
    }

    private void Start()
    {
        InitEnemies();
        _currentLivesAmount = playerLiveAmount;
        UpdateLivesCounter();
        player.OnDamageReceived += DealDamage;
        gameSettings.OnPlayerColorChanged += SavePlayerColor;
        hiScoreText.text = _saveKeeper.GetHiScore().ToString();
        player.GetComponent<Image>().color = _saveKeeper.GetPlayerColor();
        _currentScore = 0;
        _enemyRowMoveSpeed = EnemyRowMoveStartSpeed;
        gameSettingsButton.onClick.AddListener(()=> gameSettings.gameObject.SetActive(true));
    } 
    
    private void UpdateLivesCounter()
    {
        livesText.text = _currentLivesAmount.ToString(CultureInfo.InvariantCulture);
    }

    public void SavePlayerColor(Color color)
    {
        _saveKeeper.UpdatePlayerColor(color);    
    }
    
    private void DealDamage()
    {
        _currentLivesAmount--;
        UpdateLivesCounter();
        if (_currentLivesAmount == 0)
        {
            GameOver();
        }
    }

    private void InitEnemies()
    {
        var columnPosition = new Vector3(0, _screenHeight - PaddingTop, 0);
        var rowLayoutPosition = new Vector3(0, _screenHeight, 0);
        for (int i = 0; i < enemyRowsAmount; i++)
        {
            var rowLayout = Instantiate(enemyRowPrefab, rowLayoutPosition, Quaternion.identity, startTransform);
            var randomEnemyIndex = Random.Range(0, enemies.Count);
            for (int j = 0; j < enemyColumnsAmount; j++)
            {
                var enemy = Instantiate(enemies[randomEnemyIndex], columnPosition, quaternion.identity, startTransform);
                enemy.gameObject.transform.SetParent(rowLayout.transform);
                enemy.GetComponent<Enemy>().OnHit += () => {
                    _currentScore++;
                    scoreText.text = _currentScore.ToString(CultureInfo.InvariantCulture);
                };
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
        MoveEnemyRowsOnAxisX();
    }

    private void DetectRowMoveDirectionChange()
    {
        if (_enemyRowsList[0].transform.position.x - playerMoveSpeed < MaxEnemyWidth)
        {
            _isMoveRight = true;
            DownShiftEnemyRows();
        }
        else if (_enemyRowsList[0].transform.position.x + playerMoveSpeed > _screenWidth - _enemyRowWidth)
        {
            _isMoveRight = false;
            DownShiftEnemyRows();
        }
    }

    private void DownShiftEnemyRows()
    {
        var lowestY = _screenHeight;
        foreach (var enemyRow in _enemyRowsList)
        {
            var position = enemyRow.transform.position;
            position.y -= EnemyDownShift;
            enemyRow.transform.position = position;
            if (position.y - (_enemyRowsList.Count * (MaxEnemyHeight + spaceBetweenRows) + PaddingTop) < lowestY)
            {
                lowestY = position.y - (_enemyRowsList.Count * (MaxEnemyHeight + spaceBetweenRows) + PaddingTop);
            }
        }
        
        if (lowestY < 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        if (_currentScore > _hiScoreAmount)
        {
            _hiScoreAmount = _currentScore;
            _saveKeeper.UpdateHiScore(_hiScoreAmount);
            _saveKeeper.SaveParameters();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void MoveEnemyRowsOnAxisX()
    {
        foreach (var enemyRow in _enemyRowsList)
        {
            var direction = enemyRow.transform.position;
            if (_isMoveRight)
            {
                direction.x += _enemyRowMoveSpeed;
            }
            else
            {
                direction.x -= _enemyRowMoveSpeed;
            }

            enemyRow.transform.position = direction;
        }
        _enemyRowsList.RemoveAll((x) => x.transform.childCount == 0);
        
        if (_enemyRowsList.Count > 0)
        {
            _enemyRowsList[_enemyRowsList.Count  - 1].GetComponent<EnemyRow>().SetLastRow(true);    
        }
        else if (_enemyRowsList.Count == 0)
        {
            _currentLivesAmount++;
            _enemyRowMoveSpeed += EnemyRowIncreaseSpeed;
            UpdateLivesCounter();
            InitEnemies();
        }
    }
}