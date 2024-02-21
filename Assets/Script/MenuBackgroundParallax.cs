using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackgroundParallax : MonoBehaviour
{
    private float startpos, length;
    public GameObject cam;
    public float parallaxEffect;

    
    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float camAnimate = 0.0f;
        camAnimate += 0.001f;
        cam.transform.position = new Vector3(cam.transform.position.x + camAnimate, cam.transform.position.y, cam.transform.position.z);
    }

    void FixedUpdate()
    {
        float relcam = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);
        
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (relcam > startpos + length)
        {
            startpos += length;
        }

        else if (relcam < startpos - length)
        {
            startpos -= length;
        }
    }
}
