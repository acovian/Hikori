using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    // Start is called before the first frame update
    private float length, startpos;
    private float height, startpos_y;
    public GameObject cam;
    public float parallaxEffect;

    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;

        startpos_y = transform.position.y;
        height = GetComponent<SpriteRenderer>().bounds.size.y;

    }

    // Update is called once per frame
    void Update()
    {
        float dist = (cam.transform.position.x * parallaxEffect);
        float dist_y = (cam.transform.position.y * parallaxEffect);
        transform.position = new Vector3(startpos + dist, startpos_y + dist_y, transform.position.z);
    }
}
