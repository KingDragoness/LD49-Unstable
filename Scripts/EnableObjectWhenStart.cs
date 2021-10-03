using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObjectWhenStart : MonoBehaviour
{

    public GameObject target;

    void Start()
    {
        target.SetActive(true);
    }

  
}
