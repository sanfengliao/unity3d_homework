using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour {

    // Use this for initialization
    GUIStyle style;
    SceneController scene;
	void Start () {
        style = new GUIStyle();
        style.normal.textColor = Color.red;
        style.fontSize = 20;
        scene = Director.getInstance().currentSceneController;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnGUI()
    {
        if(scene.GetGameOver())
        {
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50), "GameOver, Your Score is " + scene.Score.ToString(), style);
        } else
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, 10, 100, 50), Director.getInstance().currentSceneController.Score.ToString(), style);
        }
       
    }
}
