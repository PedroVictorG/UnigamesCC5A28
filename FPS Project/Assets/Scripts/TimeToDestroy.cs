using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeToDestroy : MonoBehaviour
{
    public float time;
    void Update()
    {
        if (time < 0)
            Destroy(gameObject);
        else
            time -= 1;
    }
}
