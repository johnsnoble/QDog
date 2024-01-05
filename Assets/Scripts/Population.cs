using System.Collections;
using System;
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
    private Archive archive;
    private float time = 0f;

    private void Awake() {
        instance = this;
        testQueue = new ConcurrentQueue<Network>();
        // TEMPORARY: Create 100 networks and add to queue
        for (int i = 0; i < 100; i++) {
            addToPopulation(new Network());
        }
        archive = new Archive(15, 10f);
    }

    private void Update() {
        time += Time.deltaTime;
        if (time > episodeLength * Math.Ceiling((double)population.Count / agents.Count)) {
            foreach (Network n in population) {
                (float, Network) res = archive.objectivesUpdate(n, population);
                archive.archiveManagement(n, res.Item1, res.Item2);
                enabled = false;
            }
            print("archive size: " + archive.getArchiveSize());
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

    //TODO: replace with future
    public void logResult(Network network) {
        (float, Network) res = archive.objectivesUpdate(network, population);
        archive.archiveManagement(network, res.Item1, res.Item2);
        print("archive size : " + archive.getArchiveSize());
    }

    private void addToPopulation(Network n) {
        population.Add(n);
        testQueue.Enqueue(n);
    }
}
