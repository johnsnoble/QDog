using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archive
{
    private List<Network> archive;
    private int k, threshold;

    public Archive(int k, int threshold) {
        archive = new List<Network>();
        this.k = k;
        this.threshold = threshold;
    }

    private class NearnessComparer : IComparer<Network> {
        Network c;
        public NearnessComparer(Network c) {
            this.c = c;
        }
        public int Compare(Network x, Network y) {
            float xd = c.distance(x);
            float yd = c.distance(y);
            return xd.CompareTo(yd);
        }
    }

    public (float, Network) objectivesUpdate(Network n, List<Network> population) {
        PriorityQueue<Network> near = new PriorityQueue<Network>((IComparer<Network>) new NearnessComparer(n));
        foreach (Network network in archive) {
            near.Enqueue(network);
            while (near.Count > k) near.Dequeue();
        }

        foreach (Network network in population) {
            near.Enqueue(network);
            while (near.Count > k) near.Dequeue();
        }

        float novelty = 0;
        float count = near.Count;
        if (count == 0) return (0, null);

        Network closest = near.Peek();
        while (near.Count > 0) count += n.distance(near.Dequeue());
        novelty /= count;
        return (novelty, closest);
    }

    public void archiveManagement(float novelty, Network n, Network closest) {
        if (n.getQuality() > closest.getQuality()) {
            archive.Remove(closest);
            archive.Add(n);
        } else if (novelty > threshold) archive.Add(n);
    }
}
