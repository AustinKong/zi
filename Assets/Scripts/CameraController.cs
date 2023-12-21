using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera;
    Transform player;

    private void Start()
    {
        player = PlayerController.instance.transform;
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }

    private void LateUpdate()
    {
        if(Mathf.Abs(transform.position.x - player.position.x) > 6.5f)
        {
            transform.position += Vector3.right * (transform.position.x - player.position.x < 0 ? 1 : -1);
        }
        if (Mathf.Abs(transform.position.y - player.position.y) > 4.5f)
        {
            transform.position += Vector3.up * (transform.position.y - player.position.y < 0 ? 1 : -1);
        }
    }
}
