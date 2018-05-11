using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolFollowAction : SSAction {
    public GameObject player;
	// Use this for initialization
	public override void Start () {
        this.distroy = false;
        this.enable = true;
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
        transform.LookAt(player.transform.position);
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * 1.5f);
        gameObject.GetComponent<Animator>().SetBool("walk", true);
        player = gameObject.GetComponent<PatrolData>().player;
        if(player == null || player.GetComponent<PlayerData>().area != gameObject.GetComponent<PatrolData>().sign)
        {
            this.callback.SSActionEvent(this, 1, this.gameObject);
        }
    }
    public static SSAction GetPatrolFollowAction(GameObject player)
    {
        PatrolFollowAction action = ScriptableObject.CreateInstance<PatrolFollowAction>();
        action.player = player;
        return action;
    }
}
