using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Network
{
    private Action action;
    private int[,] genotype;
    private Dictionary<int, float> pMap;

    public Network() {
        action = new Action(12);
        genotype = new int[12, 2];
        for (int i=0; i<12; i++) {
            for (int j=0; j<2; j++) {
                genotype[i, j] = UnityEngine.Random.Range(0, 5);
            }
        }

        pMap = new Dictionary<int, float>();
        // 5 options for each parameter
        pMap.Add(0, 0f);
        pMap.Add(1, 0.25f);
        pMap.Add(2, 0.5f);
        pMap.Add(3, 0.75f);
        pMap.Add(4, 1f);
    }

    public Action GetAction(float time) {
        // calculate action for each joint and return value between 0-1
        for (int i=0; i<action.actions.Length; i++) {
            float val = (float)(genotype[i, 0] * Math.Tanh(
                4 * Mathf.Sin(2f * Mathf.PI * (time + genotype[i, 1]))
                ));
            action.actions[i] = (val + 1) / 2;
        }
        return action;
    }
}
