using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;  //REQUEST
    [SerializeField] private float offsetX = 0f;
    [SerializeField] private float offsetY = 3f;

    // Update is called once per frame
    private void Update()   // Camera follow player
    {
        // Set camera position to player position

        transform.position = new Vector3(player.position.x + offsetX, player.position.y + offsetY, transform.position.z);
    }
}
