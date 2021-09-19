using System;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    public event Action<Color> OnPlayerColorChanged;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button selectColorButton;
    [SerializeField] private Button redColorButton;
    [SerializeField] private Button greenColorButton;
    [SerializeField] private Button blueColorButton;
    [SerializeField] private Image playerColor;
    [SerializeField] private GameObject chooseColorView;
    [SerializeField] private Image player;

    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    void Start()
    {
        playerColor.color = player.GetComponent<Image>().color;
        selectColorButton.onClick.AddListener(() =>
        {
            chooseColorView.SetActive(true);
        });
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        InitColorButtonListener(redColorButton, Color.red);
        InitColorButtonListener(greenColorButton, Color.green);
        InitColorButtonListener(blueColorButton, Color.blue);
    }

    private void InitColorButtonListener(Button button, Color color)
    {
        button.onClick.AddListener(() =>
        {
            playerColor.color = color;
            player.color = color;
            OnPlayerColorChanged?.Invoke(color);
            chooseColorView.SetActive(false);
        });
    }
    
}
