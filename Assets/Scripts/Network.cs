using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;


public class Network  //: IEquatable<Network>, IComparable<Network>
{
    private Action action;
    private int[,] genotype;
    private static Dictionary<int, float> pMap = new Dictionary<int, float> {
        { 0, 0f }, { 1, 0.25f }, { 2, 0.5f }, { 3, 0.75f }, { 4, 1f }
    };
    private float quality;
    private Vector3 endPos;
    private Vector3 endOrientation; // contains x, y, z  (.y is orientation on plane)
    private static float mutationProbability = 0.1f;

    private int mutationRange; 

    public Network() {
        action = new Action(12);
        genotype = new int[12, 2];
        for (int i=0; i<12; i++) {
            for (int j=0; j<2; j++) {
                genotype[i, j] = UnityEngine.Random.Range(0, 5);
            }
        }

        this.mutationRange = (int) Math.Floor(1/Network.mutationProbability);
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

    public float distance(Network n) {
        return Vector3.Distance(n.getEnd(), endPos);
    }

    public float getQuality() {
        return quality;
    }

    public Vector3 getEnd() {
        return endPos;
    }

    public void setQuality(float quality) {
        this.quality = quality;
    }

    public void setEndPos(Vector3 end) {
        this.endPos = end;
    }

    public void setEndOri(Vector3 end) {
        this.endOrientation = end;
    }

    public override String ToString() {
        if (endPos == null) return "null";
        return "q " + quality + " pos: " + endPos.x + ", " + endPos.y;
    }

    public float calculateQuality() {
        if (endPos == null || endOrientation == null) 
        throw new NullReferenceException("end values are not filled in before calculating quality");

        float desiredOrientation;

        float x = endPos.x;
        float y = endPos.y;
        double alpha = Math.Atan(y / x);

        double r = y / Math.Sin(Math.PI - 2*alpha);
        double m = (r-x)/y;
        double c = y-x*m;
        desiredOrientation = (float) Math.Atan(x/Math.Abs(y-c));

        if (y < 0) desiredOrientation = (float) (Math.PI - desiredOrientation);
        if (x < 0) desiredOrientation = (float) (2*Math.PI + desiredOrientation);

        //radians -> degrees
        desiredOrientation = (float) (180/Math.PI * desiredOrientation);


        this.quality = -Math.Abs(desiredOrientation - endOrientation.y);
        return desiredOrientation;
    }

    public Network generateMutation(){

        Network mutated = new Network();

        for (int i=0; i<12; i++) {
            for (int j=0; j<2; j++) {
                // mutate if randomly selected number from range is 1
                if (UnityEngine.Random.Range(1, this.mutationRange) == 1) {
                    int oldType = mutated.genotype[i, j];
                    // ensure that mutated version is different
                    do {
                        mutated.genotype[i, j] = UnityEngine.Random.Range(0, 5);
                    } while (oldType == mutated.genotype[i, j]);
                }
            }
        }

        return mutated;
    }
}
