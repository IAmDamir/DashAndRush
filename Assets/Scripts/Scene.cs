using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour
{
    public Camera cam;
    float aspect;

    public float worldHeight;
    public float worldWidth;
    void Start()
    {
        aspect = (float)Screen.width / Screen.height;
        worldHeight = cam.orthographicSize * 2;
        worldWidth = worldHeight * aspect;
        worldHeight *= 2.16f;//Vector3(76.9,1,43.2) 20 35.55556
        worldWidth *= 2.16f;
        transform.localScale = new Vector3(worldWidth, 2, worldHeight);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 11 || other.gameObject.layer == 12)
        {
            Destroy(other.gameObject);
        }
    }
}
