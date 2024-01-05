using System.Collections;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;
using System.Threading;

public class Population : MonoBehaviour
{
    // Start is called before the first frame update
    public static Population instance;
    public static int episodeLength = 3;
    public static CountdownEvent cde;

    private ConcurrentQueue<Network> testQueue;
    private List<MainAgent.AgentScript> agents = new List<MainAgent.AgentScript>();
    private List<Network> population = new List<Network>();
    private Archive archive;
    private int initialPopulation = 100;

    private void Awake() {
        instance = this;
        testQueue = new ConcurrentQueue<Network>();
        cde = new CountdownEvent(initialPopulation);
        // TEMPORARY: Create 100 networks and add to queue
        for (int i = 0; i < initialPopulation; i++) {
            addToPopulation(new Network());
        }
        archive = new Archive(15, 10f);
    }

    private void Update() {
        if (cde.CurrentCount == 0) {
            foreach (Network n in population) {
                (float, Network) res = archive.objectivesUpdate(n, population);
                archive.archiveManagement(n, res.Item1, res.Item2);
            }
            print("archive size: " + archive.getArchiveSize());
            enabled = false;
            // TODO: Set next generation
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
