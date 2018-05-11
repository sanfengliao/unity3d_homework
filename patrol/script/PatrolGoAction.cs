using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolGoAction : SSAction {
    
    private Vector3 start; //巡逻兵的起始位置
    private Vector3 end;//巡逻兵起始位置的正方形对角线位置
    private Vector3 next;
    private int dir; //方向:0 x+, 1：z+, 2:x-,3:z-
    // Use this for initialization

   public static SSAction getPatrolGoAction()
    {
        PatrolGoAction action = ScriptableObject.CreateInstance<PatrolGoAction>();
        return action;
    }
    public override void Start () {
        PatrolData data = gameObject.GetComponent<PatrolData>();
        dir = 0;
        start = data.startPostion;
        end = data.endPostion;
       // transform.localEulerAngles =new  Vector3(0, 90, 0);
        next = new Vector3(end.x,0,start.z);
        transform.LookAt(next);
        this.enable = true;
        this.distroy = false;
	}
	
	// Update is called once per frame
	public override void Update () {
        //防止碰撞发生后的旋转
        if (transform.localEulerAngles.x != 0 || transform.localEulerAngles.z != 0)
        {
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        }
        if (transform.position.y != 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
        
        //移动
        transform.position = Vector3.MoveTowards(transform.position, next, Time.deltaTime * 2f);
        gameObject.GetComponent<Animator>().SetBool("walk", true);
        GameObject player = gameObject.GetComponent<PatrolData>().player;
        if (player !=null && player.GetComponent<PlayerData>().area == gameObject.GetComponent<PatrolData>().sign )
        {
            this.callback.SSActionEvent(this, 0, this.gameObject);
        }
        if (Vector3.Distance(transform.position, next) < 0.1)
        {
            dir = (dir + 1) % 4;
            if (dir == 1)
            {
                next = new Vector3(end.x, 0, end.z);
            }
            else if (dir == 2)
            {
                next = new Vector3(start.x, 0, end.z);
            }
            else if (dir == 3)
            {
                next = new Vector3(start.x, 0, start.z);
            }
            else
            {
                next = new Vector3(end.x, 0, start.z);
            }
            transform.LookAt(next);
        }
	}
}
