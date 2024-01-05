using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archive
{
    private List<Network> archive;
    private int k;
    private float threshold;

    public Archive(int k, float threshold) {
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
        List<Network> near = new List<Network>();
        Network closest = null;
        float dist = float.MaxValue;

        foreach (Network network in archive) {
            near.Add(network);
            float d = n.distance(network);
            if (d < dist) {
                closest = network;
                dist = d;
            }
        }

        foreach (Network network in population) {
            near.Add(network);
        }

        near.Sort((x, y) => n.distance(x).CompareTo(n.distance(y)));
        near.RemoveRange(k, near.Count - k);
        float novelty = 0;
        float count = near.Count;
        if (count == 0) return (0, null);

        foreach (Network net in near) novelty += n.distance(net);
        novelty /= near.Count;
        return (novelty, closest);
    }

    public void archiveManagement(Network n, float novelty, Network closest) {
        if (closest != null && n.getQuality() >= closest.getQuality()) {
            archive.Remove(closest);
            archive.Add(n);
        } else {
            if (novelty > threshold || closest == null) archive.Add(n);
        }
    }

    public int getArchiveSize() {
        return archive.Count;
    }
}
