using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRecorder : MonoBehaviour {

    // Use this for initialization

    /** 
     * score是玩家得到的总分 
     */

    public int score;

    /** 
     * scoreTable是一个得分的规则表，每种飞碟的颜色对应着一个分数 
     */

   

    // Use this for initialization  
    void Start()
    {
        score = 0;
    }

    public void addRecord()
    {
        score += 1;
    }

    public void Reset()
    {
        score = 0;
    }
}
