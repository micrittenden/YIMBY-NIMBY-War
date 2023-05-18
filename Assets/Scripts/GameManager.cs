using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Game Manager is Null");
            }

            return _instance;
        }
    }

    public bool _isGameOver;

    private void Awake()
    {
        _instance = this;
    }

    public void GameOver(bool flag)
    {
        _isGameOver = flag;
    }

    public bool IsGameOver()
    {
        return _isGameOver;
    }
}
