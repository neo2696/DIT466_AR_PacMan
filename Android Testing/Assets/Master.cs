using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Master : MonoBehaviour
{
    public int score;
    public Text pointsText;

    // Update is called once per frame
    void Update()
    {
        pointsText.text = ("Points: " + score);
    }
}
