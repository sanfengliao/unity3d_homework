using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFactory : MonoBehaviour {


    private Queue<GameObject> diskFactory = new Queue<GameObject>();
    public GameObject GetDisk(int round)
    {
        GameObject newDisk = null;
        if (diskFactory.Count > 0)
        {
            newDisk = diskFactory.Dequeue();
        }
        else
        {
            newDisk = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("prefab/disk"), Vector3.zero, Quaternion.identity);
            newDisk.AddComponent<DiskData>();
            newDisk.SetActive(false);
            
        }
        round = Random.Range(0, round + 1);
        switch (round)
        {
            case 0:
                
                newDisk.GetComponent<Renderer>().material.color = Color.yellow;
                newDisk.GetComponent<DiskData>().speed = 4.0f;
                int diraction = UnityEngine.Random.Range(-1f, 1f) > 0 ? -1 : 1;
                newDisk.GetComponent<DiskData>().flyDiraction = diraction;
                break;
            case 1:
                newDisk.GetComponent<Renderer>().material.color = Color.blue;
                newDisk.GetComponent<DiskData>().speed = 8.0f;
                int diraction1 = UnityEngine.Random.Range(-1f, 1f) > 0 ? -1 : 1;
                newDisk.GetComponent<DiskData>().flyDiraction = diraction1;
                break;
            case 2:
                newDisk.GetComponent<Renderer>().material.color = Color.red;
                newDisk.GetComponent<DiskData>().speed = 10.0f;
                int diraction2 = UnityEngine.Random.Range(-1f, 1f) > 0 ? -1 : 1;
                newDisk.GetComponent<DiskData>().flyDiraction = diraction2;
                break;
        }
        return newDisk;
    }
    public void FreeDisk(GameObject disk)
    {
        disk.SetActive(false);
        diskFactory.Enqueue(disk);
    }
	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
       
	}
}
