using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor
{
    List<float> obs;

    public Sensor() {
        obs = new List<float>();
    }

    public void AddObservation(float x) {
        obs.Add(x);
    }

    public void AddObservation(UnityEngine.Vector3 v) {
        obs.Add(v.x);
        obs.Add(v.y);
        obs.Add(v.z);
    }

    public void AddObservation(UnityEngine.Quaternion q) {
        obs.Add(q.x);
        obs.Add(q.y);
        obs.Add(q.z);
        obs.Add(q.w);
    }
}
