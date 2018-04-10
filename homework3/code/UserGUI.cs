using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.MyGame;

public class UserGUI : MonoBehaviour
{
    //public FirstController scene;
    public FirstController2 scene;
    public int status = 0;
    GUIStyle style;
    GUIStyle buttonStyle;
    float time = 60;
    float second = 0;
    void Start()
    {
        //scene = Director.getInstance().currentSceneController as FirstController;
        scene = Director.getInstance().currentSceneController as FirstController2;
        style = new GUIStyle();
        style.fontSize = 40;
        style.alignment = TextAnchor.MiddleCenter;

        buttonStyle = new GUIStyle("button");
        buttonStyle.fontSize = 30;
    }
    private void Update()
    {
        //scene.click();
        scene.check_game_over();
    }
    void OnGUI()
    {
        second += Time.deltaTime / 2;
        if (second >= 1)
        {
            time--;
            second = 0;
        }
        if (time > 0 && status == 0)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), time.ToString(), style);
            
        }
        if (status == 1 || time <= 0)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "You Fail!", style);
            if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 70), "Restart", buttonStyle))
            {
                status = 0;
                time = 60;
                second = 0;
                scene.restart();
            }
        }
        else if (status == 2)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "You win!", style);
            if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 70), "Restart", buttonStyle))
            {
                status = 0;
                time = 60;
                second = 0;
                scene.restart();
            }
        }
    }
}
