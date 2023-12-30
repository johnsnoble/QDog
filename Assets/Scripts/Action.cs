using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action
{
    public float[] actions;

    public Action(int n) {
        actions = new float[n];
        for (int i=0; i<n; i++) {
            actions[i] = 0;
        }
    }
}
