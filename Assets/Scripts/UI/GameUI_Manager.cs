using Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI_Manager : MonoBehaviour
{
    [SerializeField] private Slider _playerHealthSlider;
    [SerializeField] private TMP_Text _playerCoinText;
    [SerializeField] private GameManager gameManager;

    // UI GameObjects
    [Header("UI GameObjects")]
    [SerializeField] private GameObject _UI_GameOver;
    [SerializeField] private GameObject _UI_GameIsFinished;
    [SerializeField] private GameObject _UI_GamePlaying;
    [SerializeField] private GameObject _UI_GamePaused;


    public void PlayerHealthChangedEvent(float playerHealthRatio)
    {
        Debug.Log("Test");
        _playerHealthSlider.value = playerHealthRatio;
    }

    public void PlayerCoinChangedEvent(int playerCoinValue)
    {
        _playerCoinText.text = $"{playerCoinValue}";
    }

    public void SwitchToState(GameManager.GameState state)
    {
        _UI_GameIsFinished.SetActive(false);
        _UI_GameOver.SetActive(false);
        _UI_GamePlaying.SetActive(false);
        _UI_GamePaused.SetActive(false);

        switch (state)
        {
            case GameManager.GameState.GamePlaying:
                _UI_GamePlaying.SetActive(true);
                Time.timeScale = 1f;
                break;

            case GameManager.GameState.GameOver:
                _UI_GameOver.SetActive(true);
                break;

            case GameManager.GameState.GameIsFinished:
                _UI_GameIsFinished.SetActive(true);
                break;

            case GameManager.GameState.GamePaused:
                _UI_GamePaused.SetActive(true);
                Time.timeScale = 0f;
                break;
        }
    }

    public void Button_MainMenu()
    {
        gameManager.GameMainMenu();
    }

    public void Button_Resume()
    {
        gameManager.GameResume();
    }
    
    public void Button_ReStart()
    {
        gameManager.GameRestart();
    }
}
