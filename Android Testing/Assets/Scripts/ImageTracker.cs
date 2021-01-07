using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracker : MonoBehaviour
{
    ARTrackedImageManager imgtracker;

    public GameObject parentPrefab;
    public GameObject pacMan;
    public Dictionary<string, GameObject> myParent;
    int countp ;
    private ARRaycastManager rays;
    public Camera myCamera;
    private ARAnchorManager anc;
    private ARPlaneManager plan;
    private static ILogger logger = Debug.unityLogger;
    bool isPacMan;

    private bool left, right, up, down;

    private Rigidbody rb;
    private Collider col;

    private void Awake()
    {
        imgtracker = GetComponent<ARTrackedImageManager>();
    }

    private void Start()
    {

        myCamera = this.gameObject.transform.Find("AR Camera").gameObject.GetComponent<Camera>();
        rays = this.gameObject.GetComponent<ARRaycastManager>();
        anc = this.gameObject.GetComponent<ARAnchorManager>();
        plan = this.gameObject.GetComponent<ARPlaneManager>();

    }

    private void Update()
    {
        if (left)
        {
            rb.AddForce(Vector3.left ,ForceMode.Force);
        }

        else if (right)
        {
            rb.AddForce(Vector3.right,ForceMode.Force);
        }

        else if (up)
        {
            rb.AddForce(Vector3.forward, ForceMode.Force);
        }

        else if (down)
        {
            rb.AddForce(Vector3.back, ForceMode.Force);
        }
        else if(Input.touchCount == 2 && !isPacMan)
        {
            doSpawnPacMan();
        }
    }

    private void OnEnable()
    {
        imgtracker.trackedImagesChanged += myEventHandler;
    }

    private void OnDisable()
    {
        imgtracker.trackedImagesChanged -= myEventHandler;
    }

    void myEventHandler(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage img in eventArgs.added)
        {
            handleTracking(img);
        }

        foreach (ARTrackedImage img in eventArgs.updated)
        {
            handleTracking(img);

        }
    }

    void handleTracking(ARTrackedImage img)
    {
        GameObject parentP;
        string key;
        
        if (img.trackingState == TrackingState.None)
        {
            return;
        }
        key = img.referenceImage.name;
       
        if ( key == "Board" && countp < 1)
        {
            img.transform.Translate(0.1f, 0, 0);
            parentP = Instantiate(parentPrefab, img.transform.position, img.transform.rotation);
            parentP.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
            parentP.transform.parent = img.transform;
            countp++;
        }
        Debug.Log("Found an Image: " + img.referenceImage.name + "(" + img.trackingState + ")");
    }

    public void doSpawnPacMan()
    {
        GameObject player;
        Vector3 screenCenter;
        bool hit;
        ARRaycastHit nearest;
        List<ARRaycastHit> myHits = new List<ARRaycastHit>();
        ARPlane plane;
        ARAnchor point;

        screenCenter = myCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

        hit = rays.Raycast(screenCenter,
            myHits,
            TrackableType.FeaturePoint | TrackableType.PlaneWithinPolygon);

        logger.Log("Hit: " + hit);

        if (hit == true)
        {
            nearest = myHits[0];
            player = Instantiate(pacMan, nearest.pose.position + nearest.pose.up * 0.1f, nearest.pose.rotation);

            player.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            player.tag = "Player";

            logger.Log("spawned at " + player.transform.position.x + ", " + player.transform.position.y + ", " + player.transform.position.z);

            plane = plan.GetPlane(nearest.trackableId);
            rb = player.GetComponent<Rigidbody>();
            if (plane != null)
            {
                point = anc.AttachAnchor(plane, nearest.pose);
                logger.Log("Added an anchor to a plane " + nearest);
            }
            else
            {
                point = anc.AddAnchor(nearest.pose);
                logger.Log("Added another anchor " + nearest);

            }
            isPacMan = true;
            player.transform.parent = point.transform;
        }
    }
    public void MoveLeft()
    {
        left = true;
    }

    public void StopMovingLeft()
    {
        left = false;
    }

    public void MoveRight()
    {
        right = true;
    }

    public void StopMovingRight()
    {
        right = false;
    }

    public void MoveUp()
    {
        up = true;
    }

    public void StopMovingUp()
    {
        up = false;
    }

    public void MoveDown()
    {
        down = true;
    }

    public void StopMovingDown()
    {
        down = false;
    }



}
