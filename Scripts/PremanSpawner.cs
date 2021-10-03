using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaksoGame 
{

    public class PremanSpawner : MonoBehaviour
    {
        public Preman premanPrefab;
        public List<Transform> allSpawnLocations = new List<Transform>();

        private List<Preman> allPremans = new List<Preman>();
        private float timer = 1f;

        public static PremanSpawner Instance;

        private void Awake()
        {
            Instance = this;    
        }

        public void ClearAll()
        {
            foreach (var preman in allPremans)
            {
                Destroy(preman.gameObject);
            }

            allPremans.Clear();
        }

        private void Update()
        {
            if (ConsoleBaksoMain.Instance.dayNightCycle.GetCurrentTime() > 16)
            {

                if (timer > 0)
                {
                    timer -= Time.deltaTime;

                }
                else
                {
                    Spawn();
                    timer = 2f;
                }
            }
        }

        public void Spawn()
        {
            if (allPremans.Count > 10)
            {
                return;
            }

            var NewPreman = Instantiate(premanPrefab, transform);

            NewPreman.transform.position = allSpawnLocations[Random.Range(0, allSpawnLocations.Count - 1)].position;
            allPremans.Add(NewPreman);
        }
    }
}