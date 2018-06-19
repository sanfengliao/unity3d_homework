using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour {
    public GameObject player;
    public GameObject enemy;
    public GameObject bullet;
    public ParticleSystem bulletPs;
    public ParticleSystem tankPs;
    // Use this for initialization
    private Queue<GameObject> enemys = new Queue<GameObject>();
    private Queue<GameObject> bullets = new Queue<GameObject>();
    private List<ParticleSystem> bulletPses = new List<ParticleSystem>();
    private List<ParticleSystem> tankPses = new List<ParticleSystem>();
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    public GameObject GetPlayer()
    {
        return Instantiate<GameObject>(player);
    }
    public GameObject GetEnemy()
    {
        GameObject e = null;
        Vector3 pos;
        if (enemys.Count == 0)
        {
            e = Instantiate<GameObject>(enemy);
             pos = new Vector3(Random.Range(-90, 90), 0, Random.Range(-90, 90));
            while (Vector3.Distance(Director.getInstance().currentSceneController.player.transform.position, pos) < 20)
            {
                pos = new Vector3(Random.Range(-90, 90), 0, Random.Range(-90, 90));
            }
            e.transform.position = pos;
            return e;
        }
        e = enemys.Dequeue();
        pos = new Vector3(Random.Range(-90, 90), 0, Random.Range(-90, 90));
        while (Vector3.Distance(Director.getInstance().currentSceneController.player.transform.position, pos) < 20)
        {
            pos = new Vector3(Random.Range(-90, 90), 0, Random.Range(-90, 90));
        }
        e.transform.position = pos;
        return e;
    }
    public GameObject GetBullet(int type)
    {
        GameObject b = null;
        if (bullets.Count == 0)
        {
            b = Instantiate<GameObject>(bullet);
            b.GetComponent<Bullet>().type = type;
            return b;
        }
        b = bullets.Dequeue();
        b.GetComponent<Bullet>().type = type;
        return b;
    }

    public void RecycleEnemy(GameObject e)
    {
        e.SetActive(false);
        enemys.Enqueue(e);
    }

    public void RecycleBullet(GameObject b)
    {
        b.SetActive(false);
        bullets.Enqueue(b);
    }

    public ParticleSystem GetBulletPs()
    {
        for (int i = 0; i < bulletPses.Count; ++i)
        {
            if (!bulletPses[i].isPlaying)
            {
                return bulletPses[i];
            }
        }
        ParticleSystem p = Instantiate<ParticleSystem>(bulletPs);
        bulletPses.Add(p);
        return p;
    }
    public ParticleSystem GetTankPs()
    {
        for (int i = 0; i < tankPses.Count; ++i)
        {
            if (!tankPses[i].isPlaying)
            {
                return tankPses[i];
            }
        }
        ParticleSystem p = Instantiate<ParticleSystem>(tankPs);
        tankPses.Add(p);
        return p;
    }
}
