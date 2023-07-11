using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPointer : MonoBehaviour
{
    const float maxMultiplier = 5f;
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
        return Mathf.Max((100f - (Mathf.Abs(currentAngleValue) * 100 / maxZRotation)) * maxMultiplier / 100f, 1f);
    }
}
