using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainAgent;

public class Rough : MonoBehaviour
{
    public Transform agent;
    private AgentScript script;
    public void Start() {
        script = agent.GetComponent<AgentScript>();
    }

    /*private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Wall") {
            Debug.Log("Collision with " + gameObject.name);
        }
    }*/
}
