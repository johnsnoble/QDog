using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainAgent {
    public class AgentScript : MonoBehaviour {
        public Transform target;
        public Transform body;
        public Transform fr3100, fl3100, br3100, bl3100, fr3200, fl3200, br3200, bl3200;
        public Transform fr3300, fl3300, br3300, bl3300, fr3400, fl3400, br3400, bl3400;
        public Transform fr3500, fl3500, br3500, bl3500;
        private Vector3[] original_position;
        public HingeJoint[,] servo;
        private Transform[] parts;
        private Goal goal;
        private Network network;

        public float strength = 10000;
        public float speed = 100;
        private float time = 0f;

        public void Start() {
            network = new Network(12);
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

            original_position = new Vector3[21];
            for (int i=0; i<21; i++) {
                original_position[i] = parts[i].position;
            }

            goal = target.GetComponent<Goal>();

            OnEpisodeBegin();
        }

        public void OnEpisodeBegin() {
            Transform old = parts[0];
            for (int i=0; i < 21; i++) {
                parts[i].GetComponent<Rigidbody>().isKinematic = true;
            }

            for (int i = 0; i < 21; i++) {
                parts[i].position = original_position[i];
            }

            body.rotation = Quaternion.identity;
            fr3100.rotation = Quaternion.identity;
            fl3100.rotation = Quaternion.identity;
            br3100.rotation = Quaternion.identity;
            bl3100.rotation = Quaternion.identity;
            fr3200.rotation = Quaternion.identity;
            fl3200.rotation = Quaternion.identity;
            br3200.rotation = Quaternion.identity;
            bl3200.rotation = Quaternion.identity;
            fr3300.rotation = Quaternion.identity;
            fl3300.rotation = Quaternion.identity;
            br3300.rotation = Quaternion.identity;
            bl3300.rotation = Quaternion.identity;
            fr3400.rotation = Quaternion.identity;
            fl3400.rotation = Quaternion.identity;
            br3400.rotation = Quaternion.identity;
            bl3400.rotation = Quaternion.identity;
            fr3500.rotation = Quaternion.identity;
            fl3500.rotation = Quaternion.identity;
            br3500.rotation = Quaternion.identity;
            bl3500.rotation = Quaternion.identity;

            foreach (HingeJoint i in servo) {
                i.useLimits = true;
                i.useMotor = true;
                JointMotor joint = i.motor;
                joint.force = strength;
                joint.targetVelocity = 0;
                i.motor = joint;
            }

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

            for (int i = 0; i < 21; i++) {
                parts[i].GetComponent<Rigidbody>().isKinematic = false;
            }

            goal.respawn(old.position, old.rotation);
        }

        public void CollectObservations(Sensor sensor) {
            sensor.AddObservation(transform.localPosition);
            sensor.AddObservation(target.localPosition);

            Rigidbody rb = body.GetComponent<Rigidbody>();
            sensor.AddObservation(rb.angularVelocity);
            sensor.AddObservation(rb.rotation);
            sensor.AddObservation(rb.velocity);

            foreach (HingeJoint i in servo) {
                sensor.AddObservation(i.angle);
            }
        }

        public void OnActionReceived(Action actions) {
            int i = -1;
            moveMotor(servo[0, 0], actions.ContinuousActions[++i]);
            moveMotor(servo[0, 1], actions.ContinuousActions[++i]);
            moveMotor(servo[0, 2], actions.ContinuousActions[++i]);
            moveMotor(servo[0, 3], actions.ContinuousActions[++i]);
            moveMotor(servo[1, 0], actions.ContinuousActions[++i]);
            moveMotor(servo[1, 1], actions.ContinuousActions[++i]);
            moveMotor(servo[1, 2], actions.ContinuousActions[++i]);
            moveMotor(servo[1, 3], actions.ContinuousActions[++i]);
            moveMotor(servo[2, 0], actions.ContinuousActions[++i]);
            moveMotor(servo[2, 1], actions.ContinuousActions[++i]);
            moveMotor(servo[2, 2], actions.ContinuousActions[++i]);
            moveMotor(servo[2, 3], actions.ContinuousActions[++i]);
        }

        public void Update() {
            //AddReward(+0.01f);
            /*Vector3 facing = body.rotation * Vector3.forward;
            Vector3 toTarget = target.position - body.position;
            float angle = Mathf.Acos(Vector3.Dot(facing, toTarget) / facing.magnitude / toTarget.magnitude);
            float tol = Mathf.PI / 5;
            if (angle > Mathf.PI / 5) {
                AddReward(-0.01f * (angle - tol));
            }
            Debug.Log("height is "+parts[0].position.y);
            if (parts[0].position.y > 19) {
                AddReward(0.02f);
            } else {
                AddReward(-0.01f);
            }*/
            time += Time.deltaTime;
            network.Update();
            OnActionReceived(network.GetAction(time));
        }

        public float toFloat(bool boolean) {
            if (boolean) {
                return 1;
            }
            return 0;
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
                Debug.Log("new speed: " + t.motor.targetVelocity + " strength " + t.motor.force);
            } else {
                JointMotor joint = t.motor;
                joint.targetVelocity = 0;
                t.motor = joint;
            }
        }
    }
}
