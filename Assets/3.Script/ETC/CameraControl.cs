using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject Player;
    private void FixedUpdate()
    {
        transform.position = new Vector3(Player.transform.position.x, transform.position.y, -10);
    }
}
