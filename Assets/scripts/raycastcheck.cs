using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycastcheck : MonoBehaviour
{
    public GameObject pointer;
    public RaycastHit sendhitinfo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitinfo, Mathf.Infinity))
        {
           // Debug.Log(hitinfo.transform.name);
            sendhitinfo = hitinfo;
        }
    }
}
