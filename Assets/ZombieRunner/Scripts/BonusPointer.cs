using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BonusPointer : MonoBehaviour
{
    const float maxMultiplier = 3f;
    const float maxZRotation = 0.135f;
    public float currentAngleValue;
    public Transform centerPoint;
    
    [SerializeField] private float direction = 1;
    
    void Update()
    {
        currentAngleValue = transform.rotation.z;
        if (transform.rotation.z > maxZRotation)
        {
            //Debug.Log("revert -1");
            direction = -1;
        }
        if (transform.rotation.z < -maxZRotation)
        {
            //Debug.Log("revert 1");
            direction = 1;
        }
        //Debug.Log("BonusPointer Update" + Time.deltaTime);

        transform.RotateAround(centerPoint.position, Vector3.forward, 50 * Time.deltaTime * direction);
    }

    public float GetMultiplierFromAngle()
    {
        return 100f - (Mathf.Abs(currentAngleValue) * 100 / maxZRotation);
    }

    public float GetLevelMultiplier()
    {
        float realValue = GetMultiplierFromAngle();
        if(realValue >= 0 && realValue < 20)
        {
            return 1.5f;
        }
        else if (realValue >= 20 && realValue < 45)
        {
            return 2f;
        }
        else if (realValue >= 45 && realValue < 80)
        {
            return 2.5f;
        }
        else
        {
            return 3f;
        }
    }
}
