using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracker : MonoBehaviour
{
    ARTrackedImageManager imgtracker;

    public GameObject point;
    public Dictionary<string, GameObject> myPoints;

    public GameObject pacMan;
    private void Awake()
    {
        imgtracker = GetComponent<ARTrackedImageManager>();
        myPoints = new Dictionary<string, GameObject>();
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
        GameObject p;
        GameObject player;
        string key;
        if (img.trackingState == TrackingState.None)
        {
            return;
        }
        key = img.referenceImage.name;

        img.transform.Translate(0, 0, 0);

        if (!myPoints.ContainsKey(key))
        {
            p = Instantiate(point, img.transform.position, img.transform.rotation);
            p.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            p.transform.parent = img.transform;
            myPoints[key] = p;
        }

        if (key == "PacManSpawn")
        {
            player = Instantiate(pacMan, img.transform.position, img.transform.rotation);
            player.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
            player.transform.parent = img.transform;
        }

        Debug.Log("Found an Image: " + img.referenceImage.name + "(" + img.trackingState + ")");
    }
}
