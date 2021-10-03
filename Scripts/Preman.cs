using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaksoGame
{
    public class Preman : MonoBehaviour
    {

        public float speed = 100;
        Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            transform.LookAt(GerobakController.instance.transform);

            rb.AddForce(transform.forward * speed * Time.deltaTime);
            rb.AddForce(Vector3.up * rb.mass * 2 * Time.deltaTime);

        }

    }
}