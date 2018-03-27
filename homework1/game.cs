using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game : MonoBehaviour {
    private int turn = 1;

    private int[,] state = new int[3, 3];

	// Use this for initialization
	void Start () {
        Reset();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnGUI()
    {
        int x = 200;
        int y = 0;
        int result = check();
        if (GUI.Button(new Rect(180 + x,50 + y,100,50),"Reset")) {
            Reset();
        }
        if (result == 1)
        {
            GUI.Label(new Rect(50 + x, 160 + y, 100, 50), "O WIN");
        }
        else if (result == 2)
        {
            GUI.Label(new Rect(50 + x, 160 + y, 100, 50), "X WIN");
        }
        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                if (state[i, j] == 1)
                {
                    GUI.Button(new Rect(i * 50 + x, j * 50 + y, 50, 50), "O");
                }
                else if (state[i, j] == 2)
                {
                    GUI.Button(new Rect(i * 50 + x, j * 50 + y, 50, 50), "X");
                }
                if (GUI.Button(new Rect(i * 50 + x, j * 50 + y, 50,50), ""))
                {
                    if (result == 0)
                    {
                        if (turn == 1)
                        {
                            state[i, j] = 1;
                        }
                        else
                        {
                            state[i, j] = 2;
                        }
                        turn = -turn;
                    }
                }
               
            }
        }
    }

    private void Reset()
    {
        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                state[i, j] = 0;
            }
        }
    }

    int check()
    {
        //横向
        for (int i = 0; i < 3; ++i)
        {
            if (state[i, 0] != 0 && state[i, 0] == state[i, 1] && state[i, 1] == state[i, 2])
                return state[i, 0];
        }
        //纵向
        for (int j = 0; j < 3; ++j)
        {
            if (state[0, j] != 0 && state[0,j] == state[1,j] && state[1,j] == state[2,j])
            {
                return state[0, j];
            }
        }
        //斜线
        if (state[1, 1] != 0 && 
            (state[1, 1] == state[2, 2] && state[1, 1] == state[0, 0] || 
             state[1, 1] == state[0, 2] && state[1, 1] == state[2, 0]))
            return state[1, 1];

        return 0;
    }
}
