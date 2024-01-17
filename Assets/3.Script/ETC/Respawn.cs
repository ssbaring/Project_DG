using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Transform respawn;
    

    private void Start()
    {
        respawn = GetComponent<Transform>();
    }

    
}
