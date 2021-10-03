using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaksoGame
{
    public class NPCSpawner : MonoBehaviour
    {

        //AI
        public float detectionRange = 10;

        [Space]
        public Customer[] npcPrefabs;
        public List<Transform> spawnRegions = new List<Transform>();
        public int spawnAttempts = 20;
        public LayerMask spawnMask;
        public bool displaySpawnRange = false;

        private List<Customer> allSpawnedCustomers = new List<Customer>();

        public static NPCSpawner instance;

        private void Awake()
        {
            instance = this;
        }

        private void OnDrawGizmos()
        {

            if (displaySpawnRange == false)
            {
                return;
            }

            Gizmos.color = Color.green;

            foreach (var t in spawnRegions)
            {
                if (t == null)
                {
                    continue;
                }

                Gizmos.DrawWireCube(t.position, t.localScale);
            }
        }

        public void KillEmAll()
        {
            foreach(var customer in allSpawnedCustomers)
            {
                if (customer == null)
                {
                    continue;
                }

                Destroy(customer.gameObject);
            }

            allSpawnedCustomers.Clear();
        }

        private void Update()
        {
            Vector3 playerPos = GerobakController.instance.transform.position;

            foreach(var customer in allSpawnedCustomers)
            {
                if (customer == null)
                {
                    continue;
                }

                if (customer.hasBeenServed == true)
                {
                    continue;
                }

                float range = Vector3.Distance(playerPos, customer.transform.position);

                if (range > detectionRange)
                {
                    customer.ToggleExclamation(false);
                    continue;
                }

                customer.ToggleExclamation(true);
            }
        }

        public void SpawnNewLevel()
        {
            foreach(var customer in allSpawnedCustomers)
            {
                if (customer == null)
                {
                    continue;
                }
                Destroy(customer.gameObject);
            }

            allSpawnedCustomers.Clear();

            SpawnNPC();
        }

        private void SpawnNPC()
        {
            for (int x = 0; x < spawnAttempts; x++)
            {
                try
                {
                    Transform spawnLocation = RandomPickLocations(spawnRegions);
                    Vector3 checkPosition = spawnLocation.position;
                    Vector3 finalPositionSpawn = spawnLocation.position;
                    checkPosition.x += Random.Range(-spawnLocation.localScale.x / 2, spawnLocation.localScale.x / 2);
                    checkPosition.y += spawnLocation.localScale.y / 2;
                    checkPosition.z += Random.Range(-spawnLocation.localScale.z / 2, spawnLocation.localScale.z / 2);

                    finalPositionSpawn.x = checkPosition.x;
                    finalPositionSpawn.y = checkPosition.y;
                    finalPositionSpawn.z = checkPosition.z;

                    RaycastHit hit;

                    if (Physics.Raycast(checkPosition, spawnLocation.TransformDirection(Vector3.down), out hit, Mathf.Infinity, spawnMask))
                    {
                        finalPositionSpawn = hit.point;
                    }

                    var instantiatedScript = Instantiate(PickRandomCustomerVariant(), this.transform);
                    Vector3 eulerAngle = instantiatedScript.transform.eulerAngles;


                    instantiatedScript.transform.position = finalPositionSpawn;
                    instantiatedScript.transform.eulerAngles = eulerAngle;

                    allSpawnedCustomers.Add(instantiatedScript);
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        private Customer PickRandomCustomerVariant()
        {
            return npcPrefabs[Random.Range(0, npcPrefabs.Length)];
        }

        private Transform RandomPickLocations(List<Transform> all_Locations)
        {
            return all_Locations[Random.Range(0, all_Locations.Count - 1)];
        }
    }

}