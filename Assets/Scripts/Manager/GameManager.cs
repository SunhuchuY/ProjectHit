using Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Locomotion _player;
    [SerializeField] private UnityEvent<GameManager.GameState> _GameStateChangedEvent;

    public enum GameState
    {
        GamePlaying = 0,
        GameOver = 1,
        GameIsFinished = 2,
        GamePaused = 3,
    }

    private GameState p_currentState = GameState.GamePlaying;

    private GameState currentState 
    {
        get
        {
            return p_currentState;
        }

        set
        {
            p_currentState = value;
            _GameStateChangedEvent.Invoke(p_currentState);
        }
    }


    private void Update()
    {
        if(_player.currentState == Locomotion.State.Dead)
        {
            GameOver();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GamePaused();
        }
    }

    public void GameRestart()
    {
#if UNITY_EDITOR
        Debug.Log("GameRestart");
#endif
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GamePlaying();
    }

    public void GameMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
#if UNITY_EDITOR
        Debug.Log("GameMainMenu");
#endif
    }

    public void GameResume()
    {
        currentState = GameState.GamePlaying;
#if UNITY_EDITOR
        Debug.Log("GamePlaying");
#endif
    }

    public void GamePlaying()
    {
        currentState = GameState.GamePlaying;
#if UNITY_EDITOR
        Debug.Log("GamePlaying");
#endif
    }

    public void GameOver()
    {
        currentState = GameState.GameOver;
#if UNITY_EDITOR
        Debug.Log("GameOver");
#endif
    }

    public void GameIsFinished()
    {
        currentState = GameState.GameIsFinished;
#if UNITY_EDITOR
        Debug.Log("GameIsFinished");
#endif
    }

    public void GamePaused()
    {
        currentState = GameState.GamePaused;
#if UNITY_EDITOR
        Debug.Log("GamePaused");
#endif
    }
}
    
