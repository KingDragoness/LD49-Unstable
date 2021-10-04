using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaksoGame
{
    [RequireComponent(typeof(Customer))]
    public class Preman : MonoBehaviour
    {

        public float speed = 100;
        public Customer customer;
        Rigidbody rb;

        private float timer = 1;


        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (customer.hasBeenServed == true && customer.isPissedOff == false)
            {
                return;
            }
            if (customer.HasStartedOrder && customer.hasBeenServed == false)
            {
                return;
            }

            transform.LookAt(GerobakController.instance.transform);

            rb.AddForce(transform.forward * speed * Time.deltaTime);
            rb.AddForce(Vector3.up * rb.mass * 2 * Time.deltaTime);

            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                float dieChance = Random.Range(0, 1f);

                if (dieChance > 0.9f)
                {
                    KillTHis();
                }

                timer = 1;
            }
        }

        private void KillTHis()
        {
            float dist = Vector3.Distance(GerobakController.instance.transform.position, transform.position);

            if (dist > 35)
            {
                PremanSpawner.Instance.KillPreman(this);
            }
        }

    }
}