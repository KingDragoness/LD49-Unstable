using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableWhenStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Time.timeSinceLevelLoad < 1f)
            gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
