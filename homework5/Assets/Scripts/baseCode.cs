using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.MyGame;
namespace Com.MyGame
{
    public enum GameState { ROUND_START,ROUND_FINISH, RUNNING, PAUSE,START}
    public class Director : System.Object
    {
        private static Director _instance;
        public SceneController currentSceneController { get; set; }
        public static Director getInstance()
        {
            if (_instance == null)
            {
                _instance = new Director();
            }
            return _instance;
        }

    }

    public interface SceneController
    {
        void loadResources();
    }

    public interface UserAction
    {
        
        GameState GetGameState();
        void SetGameState(GameState state);
        void GameOver();
        int getScore();
        void restart();
        void hit();
        int GetRound();
        void setMode(int i);
    }
    public class ScoreRecorder : MonoBehaviour
    {

        /** 
         * score是玩家得到的总分 
         */

        public int score;

        /** 
         * scoreTable是一个得分的规则表，每种飞碟的颜色对应着一个分数 
         */

        private Dictionary<Color, int> scoreTable = new Dictionary<Color, int>();

        // Use this for initialization  
        void Start()
        {
            score = 0;
            scoreTable.Add(Color.yellow, 1);
            scoreTable.Add(Color.blue, 2);
            scoreTable.Add(Color.red, 4);
        }

        public void Record(GameObject disk)
        {
            score += scoreTable[disk.GetComponent<Renderer>().material.color];
        }

        public void Reset()
        {
            score = 0;
        }
    }

}
