using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainAgent;

public class Goal : MonoBehaviour
{
    public Transform agent;
    public Transform agentBody;
    private AgentScript script;
    private float spawnDistance = 50;
    private Transform target;
    public void Start() {
        script = agent.GetComponent<AgentScript>();
        target = gameObject.transform;
        respawn(agentBody.position, agentBody.rotation);
    }
    private void OnTriggerEnter(Collider col) {
        if (col.tag != "Wall" && col.tag != "Ground") {
            Debug.Log("Goal reached!");
            respawn(agentBody.position, agentBody.rotation);
        }
    }
    public void respawn(Vector3 centre, Quaternion direction) {
        float x = Random.Range(-spawnDistance, spawnDistance);
        float y = Random.Range(-spawnDistance, spawnDistance);
        Vector3 newPos = new Vector3(x, 5, y);
        Debug.Log("new position is " + newPos);
        target.localPosition = newPos;
    }

    public void updateDifficulty(float value) {
        spawnDistance = value;
    }
}
