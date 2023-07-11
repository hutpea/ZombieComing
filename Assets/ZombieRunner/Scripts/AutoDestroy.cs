using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Runner;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifeTime;
    private float timer = 0;
    private void Start()
    {
        //StartCoroutine(AutoDestroyCoroutine(lifeTime));
    }

    private void Update()
    {
        if (PlayerController.Instance.isInMenu) return;
        timer += Time.deltaTime;
        if (timer > lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator AutoDestroyCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
