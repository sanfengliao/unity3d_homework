using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Tank {

    // Use this for initialization
    private Vector3 target;
    private NavMeshAgent agent;
    private bool gameover;
    void Start () {
        Hp = 5;
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(shoot());
	}
	
	// Update is called once per frame
	void Update () {
        gameover = Director.getInstance().currentSceneController.GetGameOver();
        if (!gameover)
        {
            target = Director.getInstance().currentSceneController.player.transform.position;
            if (Hp <= 0)
            {
                ParticleSystem ps = Singleton<Factory>.Instance.GetTankPs();
                ps.transform.position = transform.position;
                ps.Play();
                Director.getInstance().currentSceneController.Score += 1;
                Singleton<Factory>.Instance.RecycleEnemy(this.gameObject);
            }
            else
            {
                    transform.LookAt(target);
                    agent.SetDestination(target);
            }
        }
        else
        {
            agent.velocity = Vector3.zero;
            agent.ResetPath();
        }
        
	}
    IEnumerator shoot()
    {//协程实现npc坦克每隔1s进行射击
        while (!gameover)
        {
            for (float i = 1; i > 0; i -= Time.deltaTime)
            {
                yield return 0;
            }
            if (Vector3.Distance(transform.position, target) < 20)
            {//和玩家坦克距离小于20，则射击
                Factory f = Singleton<Factory>.Instance;
                GameObject bullet =f.GetBullet(1);//获取子弹，传入的参数表示发射子弹的坦克类型
                bullet.transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z) +
                    transform.forward * 1.5f;//设置子弹
                bullet.transform.forward = transform.forward;//设置子弹方向
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                rb.AddForce(bullet.transform.forward * 20, ForceMode.Impulse);//发射子弹
            }
        }
    }

}
