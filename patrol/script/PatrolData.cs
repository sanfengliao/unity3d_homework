using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Dir { down, right, up, left}
public class PatrolData : MonoBehaviour {

    // Use this for initialization
    public Vector3 startPostion; //巡逻的起始位置
    public Vector3 endPostion;//正方形对角线位置
    public int sideLength;
    public int dir = 0;
    public GameObject player; //玩家
    public int sign; //巡逻兵在哪一块区域
}
