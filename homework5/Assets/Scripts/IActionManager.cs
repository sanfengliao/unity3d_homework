using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActionManager {

    int getDiskNum();
    void StartThrow(Queue<GameObject> disks);
    void setDiskNum(int num);
}
