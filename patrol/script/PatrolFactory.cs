using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolFactory : MonoBehaviour {
    private List<GameObject> patrols = new List<GameObject>();
	// Use this for initialization
    public List<GameObject> GetPatrols()
    {
        int rectLength;
        int[] pos_x = { 5, -2, -9 };
        int[] pos_z = { 5, -2, -9 };
        int index = 1;
        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                Vector3 startPostion = new Vector3(pos_x[i], 0, pos_z[j]);
                rectLength = UnityEngine.Random.Range(3, 5);
                Vector3 endPostion = new Vector3(pos_x[i] + rectLength, 0, pos_z[j] + rectLength);
                GameObject monster =  Instantiate(Resources.Load<GameObject>("Prefabs/Monster"));
                monster.transform.position = startPostion;
                PatrolData data =  monster.AddComponent<PatrolData>();
                data.startPostion = startPostion;
                data.endPostion = endPostion;
                data.sideLength = rectLength;
                data.sign = index;
                patrols.Add(monster);
                ++index;
            }
        }
        return patrols;
    }
}
