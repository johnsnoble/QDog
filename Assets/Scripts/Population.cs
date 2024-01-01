using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;

public class Population : MonoBehaviour
{
    // Start is called before the first frame update
    public static Population instance;
    public static int episodeLength = 3;
    private ConcurrentQueue<Network> testQueue;

    private List<MainAgent.AgentScript> agents = new List<MainAgent.AgentScript>();
    private List<Network> population = new List<Network>();

    private void Awake() {
        instance = this;
        testQueue = new ConcurrentQueue<Network>();
        // TEMPORARY: Create 100 networks and add to queue
        for (int i = 0; i < 100; i++) {
            addToPopulation(new Network());
        }
    }

    public Network getNetwork() {
        if (testQueue.TryDequeue(out Network net)) {
            return net;
        }
        return null;
    }

    public void addAgent(MainAgent.AgentScript agent) {
        agents.Add(agent);
    }

    private void addToPopulation(Network n) {
        population.Add(n);
        testQueue.Enqueue(n);
    }
}
