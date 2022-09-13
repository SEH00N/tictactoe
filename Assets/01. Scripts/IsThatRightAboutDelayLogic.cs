using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsThatRightAboutDelayLogic : MonoBehaviour
{
    [SerializeField] float delay;
    private float rDelay = 0;

    private void Update()
    {
        delay += delay * Time.deltaTime;
        rDelay += Time.deltaTime;
        if(delay >= 100)
        {
            Debug.Log("Time.deltaTime : " + rDelay);
            Debug.Log("병신 : " + delay);

            delay = 0;
            rDelay = 0;
        }
    }
}
