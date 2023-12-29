using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population : MonoBehaviour
{
    // Start is called before the first frame update
    public static Population instance;

    private List<MainAgent.AgentScript> agents = new List<MainAgent.AgentScript>();

    private void Awake() {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
// Debug.Log("there is " + agents.Count + " agents");
    }

    public void Add(MainAgent.AgentScript agent) {
        agents.Add(agent);
        Debug.Log("Added agent length of " + agents.Count);
    }
}
