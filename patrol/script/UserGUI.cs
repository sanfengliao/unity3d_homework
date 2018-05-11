using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    public FirstSceneController scene;
    public int status = 0;
    public bool start;
    GUIStyle style;
    GUIStyle buttonStyle;
    
    bool isSelect = false;
    void Start()
    {
        //scene = Director.getInstance().currentSceneController as FirstController;
        scene = Director.getInstance().currentSceneController as FirstSceneController;
        style = new GUIStyle("label");
        style.fontSize = 20;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.red;
        style.alignment = TextAnchor.MiddleCenter;
        buttonStyle = new GUIStyle("button");
        buttonStyle.fontSize = 25;
        

    }
    private void Update()
    {
        //scene.click();

    }
    void OnGUI()
    {

        int buttonX = Screen.width / 2 - 45;
        int buttonY = Screen.height / 2 - 45;
        int labelX = Screen.width / 2 - 45;
        int labelY = Screen.height;
        if (!start)
        {
            if (GUI.Button(new Rect(buttonX, buttonY, 90,90), "Start",buttonStyle))
            {
                start = true;
                scene.StartGame();
            }
        }
        else
        {
            if (scene.game_over)
            {
                GUI.Label(new Rect(Screen.width/2-90, buttonY, 180, 90), "Game Over!\n Your Score is " + scene.GetScore(), style);
            }
            else
            {
                scene.MovePlayer();
            }
            
        }
    }
}
