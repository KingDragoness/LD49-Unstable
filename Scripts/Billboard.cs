using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    // Damn Unity should have integrate to the engine rather than having dev making this damn thing over 100 times every time a new project was made!
    void Update()
    {
        transform.LookAt(Camera.main.transform.position, -Vector3.up);
    }
}
