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

            // generateOffspring();
            // TODO: delete half of new population based on quality
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

    private void generateOffspring(){
        // only mutation for now
        // parents selected in tournament style
        int i1;
        int i2;
        int oldPopCount = population.Count; 
        for (int c = oldPopCount; initialPopulation > 0; c -= 1){
            // pick 2 different indexes
            do{
                i1 = UnityEngine.Random.Range(0, oldPopCount - 1);
                i2 = UnityEngine.Random.Range(0, oldPopCount - 1);
            } while (i1 == i2);

            Network winner = (population[i1].getQuality() >= population[i2].getQuality()) ? population[i1] : population[i2];
            this.addToPopulation(winner.generateMutation());
        }
        
    }
}
