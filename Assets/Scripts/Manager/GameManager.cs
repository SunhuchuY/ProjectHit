using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Locomotion _player;

    [SerializeField] private float maxAngle = 3f;
    [SerializeField] private float maxRadius = 3f;

    public float radius = 5f;
    public int resolution = 100;
    public float hoverHeight = 5;
    public float hoverSpeed = 5;
    public float fieldOfViewAngle = 45f; // 시야각
    public float viewDistance = 10f; // 시야 거리


    public enum GameState
    {
        GamePlaying = 0,
        GameOver = 1,
        GameIsFinished = 2,
    }

    private GameState currentState;


    private void Awake()
    {
        currentState = GameState.GamePlaying;   
    }

    private void Update()
    {
        if(_player.currentState == Locomotion.State.Dead)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        currentState = GameState.GameOver;
        Debug.Log("GameOver");
    }

    public void GameIsFinished()
    {
        currentState = GameState.GameIsFinished;
        Debug.Log("GameIsFinished");
    }
}
    
