using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainAgent {
    public class AgentScript : MonoBehaviour {
        public Transform body;
        public Transform fr3100, fl3100, br3100, bl3100, fr3200, fl3200, br3200, bl3200;
        public Transform fr3300, fl3300, br3300, bl3300, fr3400, fl3400, br3400, bl3400;
        public Transform fr3500, fl3500, br3500, bl3500;
        public HingeJoint[,] servo;
        public float strength = 10000;
        public float speed = 100;
        public Transform plane;
        public Material awakeColour, asleepColour;

        private Vector3[] original_position;
        private Transform[] parts;
        private Network network;
        private bool awake;
        private float time = 0f;

        public void Start() {
            // store joints and bodies to a more usable format
            servo = new HingeJoint[3, 4];
            servo[0, 0] = fr3100.GetComponent<HingeJoint>();
            servo[0, 1] = fl3100.GetComponent<HingeJoint>();
            servo[0, 2] = br3100.GetComponent<HingeJoint>();
            servo[0, 3] = bl3100.GetComponent<HingeJoint>();
            servo[1, 0] = fr3200.GetComponent<HingeJoint>();
            servo[1, 1] = fl3200.GetComponent<HingeJoint>();
            servo[1, 2] = br3200.GetComponent<HingeJoint>();
            servo[1, 3] = bl3200.GetComponent<HingeJoint>();
            servo[2, 0] = fr3400.GetComponent<HingeJoint>();
            servo[2, 1] = fl3400.GetComponent<HingeJoint>();
            servo[2, 2] = br3400.GetComponent<HingeJoint>();
            servo[2, 3] = bl3400.GetComponent<HingeJoint>();

            parts = new Transform[21];
            parts[0] = body;
            parts[1] = fr3100;
            parts[2] = fl3100;
            parts[3] = br3100;
            parts[4] = bl3100;
            parts[5] = fr3200;
            parts[6] = fl3200;
            parts[7] = br3200;
            parts[8] = bl3200;
            parts[9] = fr3300;
            parts[10] = fl3300;
            parts[11] = br3300;
            parts[12] = bl3300;
            parts[13] = fr3400;
            parts[14] = fl3400;
            parts[15] = br3400;
            parts[16] = bl3400;
            parts[17] = fr3500;
            parts[18] = fl3500;
            parts[19] = br3500;
            parts[20] = bl3500;

            // save original position so we can reset later
            original_position = new Vector3[21];
            for (int i=0; i<21; i++) {
                original_position[i] = parts[i].position;
            }

            // give population manager a reference to this agent (executor)
            Population.instance.addAgent(this);
            // Try get a network (task) to execute
            network = Population.instance.getNetwork();
            awake = (network == null) ? false : true;
            updateMaterial();

            // Set initial state
            OnEpisodeBegin();
            
        }

        public void OnEpisodeBegin() {
            // Freeze body (probably not neccessary)
            for (int i = 0; i < 21; i++) {
                parts[i].GetComponent<Rigidbody>().isKinematic = true;
            }

            //reset position and rotation
            for (int i = 0; i < 21; i++) {
                parts[i].position = original_position[i];
                parts[i].rotation = Quaternion.identity;
            }

            // reset each motor to be static
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    servo[i, j].useLimits = true;
                    servo[i, j].useMotor = true;
                    var joint = servo[i,j].motor;
                    joint.force = strength;
                    joint.targetVelocity = 0;
                    servo[i,j].motor = joint;
                }
            }

            //unfreeze everything
            for (int i = 0; i < 21; i++) {
                parts[i].GetComponent<Rigidbody>().isKinematic = false;
            }

        }

        public void OnActionReceived(Action actions) {
            int k = 0;
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    moveMotor(servo[i, j], actions.actions[k++]);
                }
            }
        }

        public void Update() {
            // increment time and if awake get next action by updating network
            time += Time.deltaTime;
            if (awake) { OnActionReceived(network.GetAction(time)); }
            if (time >= Population.episodeLength) {
                // Once its time for the episode to stop we try get the next network and repeat
                if (network != null) {
                    network.setEndPos(body.position);
                    print("current orientation:" + this.transform.localRotation.eulerAngles);
                    network.setEndOri(this.transform.localRotation.eulerAngles);
                    network.setQuality(0);
                    Population.cde.Signal();
                }
                network = Population.instance.getNetwork();
                awake = (network == null) ? false : true;
                updateMaterial();
                OnEpisodeBegin();
                time = 0f;
            }
        }

        public void moveMotor(HingeJoint t, float value, float tol = 0.1f) {
            value = (1 - value) * t.limits.min + value*t.limits.max;
            if (Mathf.Abs(value - t.angle) > tol) {
                var joint = t.motor;
                if (value > t.angle) {
                    joint.targetVelocity = speed;
                } else {
                    joint.targetVelocity = -speed;
                }
                t.motor = joint;
            } else {
                JointMotor joint = t.motor;
                joint.targetVelocity = 0;
                t.motor = joint;
            }
        }

        private void updateMaterial() {
            plane.GetComponent<MeshRenderer>().material = awake ? awakeColour : asleepColour;
        }
    }
}
