using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GoogleARCore;

#if UNITY_EDITOR
// Set up touch input propagation while using Instant Preview in the editor.
using Input = GoogleARCore.InstantPreviewInput;
#endif

public class skeletonarplacer : MonoBehaviour
{

    public GameObject currentgameobject, instgam,currentlyselected, planevisualizer;
    bool isinstantiated = false;

    public GameObject[] bones,ui;
    public GameObject uipluck;

    RaycastHit hitinfo;
    raycastcheck raycastcheck;
    public Text debtext;
    public Material shader;
    Transform boneplacer;
    // Start is called before the first frame update
    void Start()
    {
        raycastcheck = FindObjectOfType<raycastcheck>();
    }

    // Update is called once per frame
    void Update()
    {
        Touch touch;
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            Debug.Log("Touch detected");
        }
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }

        TrackableHit hit;
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
            TrackableHitFlags.FeaturePointWithSurfaceNormal;

        if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
        {
            if (hit.Trackable is DetectedPlane && !isinstantiated)
            {
                instgam = Instantiate(currentgameobject, hit.Pose.position, hit.Pose.rotation);
                var anchor = hit.Trackable.CreateAnchor(hit.Pose);
                instgam.transform.parent = anchor.transform;
                isinstantiated = true;
                planevisualizer.SetActive(false);
                boneplacer = instgam.transform.GetChild(instgam.transform.childCount - 1);
                uipluck = instgam.transform.GetChild(instgam.transform.childCount - 2).gameObject;
            }
        }

        Debug.Log(raycastcheck.sendhitinfo.transform.name);

        foreach (GameObject bone in bones)
        {
            if (raycastcheck.sendhitinfo.transform.name == bone.name)
            {
                if (currentlyselected != null)
                {
                    Destroy(currentlyselected);
                    currentlyselected = Instantiate(bone, boneplacer.position, Quaternion.identity);
                    
                    foreach (var uielement in ui)
                    {
                        if (bone.name == uielement.name)
                        {
                            uipluck.GetComponent<SpriteRenderer>().sprite = uielement.GetComponent<SpriteRenderer>().sprite;
                        }
                    }
                }
            }           
        }        
    }

    public void reloadscene()
    {
        SceneManager.LoadScene(0);
    }

    public void exitapp()
    {
        Application.Quit();
    }


}
