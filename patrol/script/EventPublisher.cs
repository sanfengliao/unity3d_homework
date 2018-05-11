using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPublisher : MonoBehaviour {
    //分数实践
    public delegate void ScoreEvent();
    public static event ScoreEvent ScoreAddEvent;

    public delegate void GameEvent();
    public static event GameEvent GameOverEvent;
	// Use this for initialization
    public void ScoreAdd()
    {
        if(ScoreAddEvent!=null)
        {
            ScoreAddEvent();
        }
    }

    public void GameOver()
    {
        if(GameOverEvent!=null)
        {
            GameOverEvent();
        }
    }
}
