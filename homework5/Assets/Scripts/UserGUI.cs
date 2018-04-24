using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.MyGame;

public class UserGUI : MonoBehaviour
{
    public FirstController scene;
    public int status = 0;
    public bool isFirst = true;
    GUIStyle style;
    GUIStyle buttonStyle;
    float time = 60;
    float second = 0;
    bool isSelect = false;
    void Start()
    {
        //scene = Director.getInstance().currentSceneController as FirstController;
        scene = Director.getInstance().currentSceneController as FirstController;
        style = new GUIStyle();
        style.fontSize = 40;
        style.normal.textColor = Color.red;
        style.alignment = TextAnchor.MiddleCenter;

        buttonStyle = new GUIStyle("button");
        buttonStyle.fontSize = 25;
        
    }
    private void Update()
    {
        //scene.click();

    }
    void OnGUI() {
       
        int buttonX = Screen.width / 2 - 45;
        int buttonY = Screen.height / 2 - 45;
        int labelX = Screen.width / 2 - 45;
        int labelY = Screen.height;
        if (scene.GetRound() == 2 && scene.actionManger.getDiskNum() == 0)
        {
            //Debug.Log(2);
            GUI.Button(new Rect(buttonX, buttonY, 90, 90), "GameOver");
            return;
        }
        if (!isSelect)
        {
          
            
            if (GUI.Button(new Rect(buttonX- 45, buttonY, 90, 90), "PHYSICS"))
            {
                scene.setMode(1);
                isSelect = true;
            }
            if (GUI.Button(new Rect(buttonX + 90, buttonY, 90, 90), "CCFLY")) {
                scene.setMode(0);
                isSelect = true;
            }    
            return;
        }
        scene.hit();
        //GUI.Label(new Rect(buttonX, buttonY, 90, 90), "GameOver");
        GUI.Label(new Rect(1000, 0, 400, 400), scene.getScore().ToString(),style);
       
        if (scene.GetGameState() == GameState.RUNNING && GUI.Button(new Rect(10, 10,90,90 ), "Pause",buttonStyle) )
        {
            scene.SetGameState(GameState.PAUSE);
            Time.timeScale = 0;
        }
        if (scene.GetGameState() == GameState.PAUSE && GUI.Button(new Rect(10, 10, 90,90), "Run", buttonStyle))
        {
            scene.SetGameState(GameState.RUNNING);
            Time.timeScale = 1;
        }

        if (scene.GetRound() == -1 && GUI.Button(new Rect(buttonX, buttonY, 90, 90), "Start")) {
            scene.SetGameState(GameState.ROUND_START);
        }

        if (scene.GetGameState() == GameState.ROUND_FINISH  && scene.GetRound() < 2 && GUI.Button(new Rect(buttonX, buttonY, 90, 90), "Next Round"))
        {
        scene.SetGameState(GameState.ROUND_START);
        }
    }
}
