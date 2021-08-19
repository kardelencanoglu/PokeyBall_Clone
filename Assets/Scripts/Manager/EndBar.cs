using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EndBar : MonoBehaviour
{
    [SerializeField] Transform Player;
    [SerializeField] Transform EndLine;
    [SerializeField] Slider slider;

    float maxDistance;

    // Start is called before the first frame update
    void Start()
    {
        maxDistance = getDistance();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.position.z <= maxDistance && Player.position.z <= EndLine.position.z)
        {
            float distance = 1 - (getDistance() / maxDistance);
            setProgress(distance);
            //Debug.Log(getDistance());
            //Debug.Log(EndLine.position.y);

        }
    }

    float getDistance()
    {
        return Vector3.Distance(Player.position, EndLine.position);
    }

    void setProgress(float p)
    {
        slider.value = p;
    }
}