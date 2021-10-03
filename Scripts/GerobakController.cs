using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaksoGame
{

    [System.Serializable]
    public class AxleInfo
    {
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;
        public bool motor;
        public bool steering;
    }

    public class GerobakController : MonoBehaviour
    {

        public float uprightThreshold = 0.7f;
        public float hitThreshold = 100;

        [Space]
        [Header("Gerobak Objects")]
        public GameObject penompangKayu;

        [Space]
        [Header("Motor")]
        public List<AxleInfo> axleInfos;
        public float maxMotorTorque;
        public float maxSteeringAngle;

        public static GerobakController instance;
        private Rigidbody rb;
        private float timerUpside = 1.5f;

        private const float TIMER = 1.5f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            instance = this;
        }

        private void FixedUpdate()
        {
            if (ConsoleBaksoMain.Instance.IsDead)
            {
                return;
            }

            Thrust();
            Steering();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (ConsoleBaksoMain.Instance.isDayStarted == false)
            {
                return;
            }
            if (rb.velocity.magnitude > hitThreshold)
            {
                ConsoleBaksoMain.Instance.DamagePlayer(10);
            }
        }

        [ContextMenu("CheckUpside")]
        public bool CheckUpside()
        {
            if (transform.up.y > uprightThreshold)
            {
                return true;
            }

            return false;
        }

        private void Update()
        {

            if (ConsoleBaksoMain.Instance.IsDead | ConsoleBaksoMain.Instance.isDayStarted == false)
            {
                return;
            }

            if (timerUpside > 0)
            {
                timerUpside -= Time.deltaTime;
            }
            else
            {
                if (CheckUpside() == false)
                {
                    ConsoleBaksoMain.Instance.DamagePlayer(7);
                }

                timerUpside = TIMER;
            }

            if (Input.GetKeyUp(KeyCode.Return))
            {
                ResetGerobak();
            }

            if (ConsoleBaksoMain.Instance.IsServingMode)
            {
                rb.mass = 10000;
                rb.drag = 1f;
            }
            else
            {
                rb.mass = 500;
                rb.drag = 0.1f;
            }
        }

        public void RefreshGerobak()
        {
            penompangKayu.SetActive(ConsoleBaksoMain.Instance.IsServingMode);

        }

        private void Thrust()
        {
            float motor = maxMotorTorque * Input.GetAxis("Vertical");

            foreach (AxleInfo axleInfo in axleInfos)
            {
                if (axleInfo.motor)
                {
                    axleInfo.leftWheel.motorTorque = motor;
                    axleInfo.rightWheel.motorTorque = motor;
                }

                ApplyLocalPositionToVisuals(axleInfo.leftWheel);
                ApplyLocalPositionToVisuals(axleInfo.rightWheel);
            }

        }

        private void Steering()
        {
            float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

            foreach (AxleInfo axleInfo in axleInfos)
            {
                if (axleInfo.steering)
                {
                    axleInfo.leftWheel.steerAngle = steering;
                    axleInfo.rightWheel.steerAngle = steering;
                }

            }
        }

        // finds the corresponding visual wheel
        // correctly applies the transform
        public void ApplyLocalPositionToVisuals(WheelCollider collider)
        {
            if (collider.transform.childCount == 0)
            {
                return;
            }

            Transform visualWheel = collider.transform.GetChild(0);

            Vector3 position;
            Quaternion rotation;
            collider.GetWorldPose(out position, out rotation);

            visualWheel.transform.position = position;
            visualWheel.transform.rotation = rotation;
        }


        private void ResetGerobak()
        {
            Vector3 vehicleEuler = transform.localEulerAngles;
            vehicleEuler.x = 0;
            vehicleEuler.z = 0;
            transform.localEulerAngles = vehicleEuler;
        }
    }
}