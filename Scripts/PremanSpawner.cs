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
        private float timer = 5f;

        public static PremanSpawner Instance;
        private int premanLimit = 0;

        private void Awake()
        {
            Instance = this;    
        }

        public void ClearAll()
        {
            allPremans.RemoveAll(x => x == null);

            foreach (var preman in allPremans)
            {
                Destroy(preman.gameObject);
            }

            allPremans.Clear();
            premanLimit = 0;

        }

        public void KillPreman(Preman preman)
        {
            Destroy(preman.gameObject);


            allPremans.RemoveAll(x => x == null);
        }

        private void Update()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;

            }
            else
            {
                Spawn();
                timer = 5f;
            }

            if (ConsoleBaksoMain.Instance.dayNightCycle.GetCurrentTime() > 15)
            {
                premanLimit = 2;

            }
            if (ConsoleBaksoMain.Instance.dayNightCycle.GetCurrentTime() > 16)
            {
                premanLimit = 4;

            }
        }

        public void Spawn()
        {
            if (allPremans.Count >= premanLimit)
            {
                return;
            }

            var NewPreman = Instantiate(premanPrefab, transform);

            NewPreman.transform.position = allSpawnLocations[Random.Range(0, allSpawnLocations.Count - 1)].position;
            allPremans.Add(NewPreman);
        }
    }
}