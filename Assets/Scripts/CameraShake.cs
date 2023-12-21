using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private void Awake()
    {
        instance = this;
    }

    public bool shaking = false;

    public IEnumerator Shake(float duration, float magnitude) {
        /*you can set the originalPos to transform.localPosition of the camera in in that instance.*/

        shaking = true;
        Vector3 originalPos = transform.position;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float xOffset = Random.Range(-0.5f, 0.5f) * magnitude; 
            float yOffset = Random.Range(-0.5f, 0.5f) * magnitude; 
            transform.position =  originalPos +  new Vector3(xOffset, yOffset, 0); 
            elapsedTime += Time.deltaTime; //wait one frame
            yield return null; 
        }
        transform.position = originalPos;
        shaking = false;
    }
}
