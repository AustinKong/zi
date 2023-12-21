using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlayerController : MonoBehaviour
{
   
    void Update()
    {
        Vector2Int movementDirection = Vector2Int.zero;

        if (Input.GetKeyDown(KeyCode.UpArrow)) movementDirection = Vector2Int.up;
        if (Input.GetKeyDown(KeyCode.DownArrow)) movementDirection = Vector2Int.down;
        if (Input.GetKeyDown(KeyCode.LeftArrow)) movementDirection = Vector2Int.left;
        if (Input.GetKeyDown(KeyCode.RightArrow)) movementDirection = Vector2Int.right;

        if (transform.position.x < -10) movementDirection = Vector2Int.right;
        if (transform.position.x > 10) movementDirection = Vector2Int.left;
        if (transform.position.y < -10) movementDirection = Vector2Int.up;
        if (transform.position.y > 10) movementDirection = Vector2Int.down;

        transform.Translate((Vector2)movementDirection);
    }
}
