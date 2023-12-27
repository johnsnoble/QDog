using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action
{
    public float[] ContinuousActions;

    public Action(int n) {
        ContinuousActions = new float[n];
        for (int i=0; i<n; i++) {
            ContinuousActions[i] = 0;
        }
    }
}
