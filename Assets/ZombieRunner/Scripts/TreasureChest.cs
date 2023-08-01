using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Runner;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    public ParticleSystem confettiPrefab;
    private bool isActive = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isActive)
        {
            isActive = true;
            StartCoroutine(WinCoroutine());
        }
    }

    private IEnumerator WinCoroutine()
    {
        PlayerController.Instance.ZombieFinishRun();
        var currentZombieList = PlayerController.Instance.zombieList;
        foreach (var zombie in currentZombieList)
        {
            zombie.animator.SetFloat("Speed", 0);
            zombie.animator.Play("Dance");
            
            int randN = UnityEngine.Random.Range(0, 100);
            if (randN > 50)
            {
                Instantiate(confettiPrefab, zombie.transform.position + new Vector3(0, 0, -0.5f), Quaternion.identity);
                confettiPrefab.transform.localScale = new Vector3(4f, 4f, 4f);
            }
        }

        yield return new WaitForSeconds(1f);
        GameManager.Instance.m_WinEvent.Raise();
    }
}
