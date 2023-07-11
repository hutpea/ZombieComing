using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HyperCasual.Runner;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class EndGameEvent : MonoBehaviour
{
    public GameObject gunManPrefab;
    public GameObject ladderPrefab;
    
    public Vector3 gunManRowOffset;
    public Vector3 ladderOffset;

    private Vector3 finishLinePos;
    
    public void Init()
    {
        //finishLinePos = GameManager.Instance.endLineObject.transform.position;
        GameObject finishLineObj = GameObject.FindGameObjectWithTag("Finish");
        if (finishLineObj != null)
        {
            finishLinePos = finishLineObj.transform.position;
        }
        PlayerController.Instance.ZombieFinishRun();
        StartCoroutine(EndGame());
    }

    private IEnumerator EndGame()
    {
        List<Gunman> gunmanList = new List<Gunman>();
        for (int i = 1; i <= 3; i++)
        {
            var gunManObj = Instantiate(gunManPrefab, finishLinePos + gunManRowOffset, Quaternion.Euler(0, 180f, 0));
            Gunman gunManComponent = gunManObj.GetComponent<Gunman>();
            gunmanList.Add(gunManComponent);

            switch (i)
            {
                case 1:
                {
                    break;
                }
                case 2:
                {
                    gunManObj.transform.DOMoveX(-2f, 1f);
                    break;
                }
                case 3:
                {
                    gunManObj.transform.DOMoveX(2f, 1f).OnComplete(delegate
                    {
                        foreach (var gunman in gunmanList)
                        {
                            gunman.isFire = true;
                        }
                    });
                    break;
                }
            }
        }

        var currentZombieList = PlayerController.Instance.zombieList;
        int numGunMan = gunmanList.Count - 1;
        foreach (var zombie in currentZombieList)
        {
            zombie.GetComponent<Rigidbody>().isKinematic = true;
            Vector3 targetPos = zombie.transform.position;
            if (numGunMan >= 0)
            {
                targetPos = gunmanList[numGunMan].transform.position;
                numGunMan--;
            }
            else
            {
                targetPos = new Vector3(Random.Range(-6.5f, 6f), 0, finishLinePos.z + gunManRowOffset.z);
            }

            zombie.transform.DOMove(targetPos, 1.5f).OnComplete(delegate
            {
            });
        }

        Vector3 targetPlayerPos = PlayerController.Instance.transform.position;
        targetPlayerPos += gunManRowOffset;
        PlayerController.Instance.transform.DOMoveZ(targetPlayerPos.z, 2f);

        for (int i = 0; i <= 4; i++)
        {
            GameObject ladderObj = Instantiate(ladderPrefab);
            ladderObj.transform.position = finishLinePos + gunManRowOffset + ladderOffset + new Vector3(0, 1f + i * 2, 0);
            TextMeshPro textMesh = ladderObj.transform.GetComponentInChildren<TextMeshPro>();
            textMesh.SetText("X" + (i + 2).ToString());
        }
        
        yield return new WaitForSeconds(2f);
        if (gunmanList[0] != null)
        {
            Destroy(gunmanList[0].gameObject);
        }
        if (gunmanList[1] != null)
        {
            Destroy(gunmanList[1].gameObject);
        }
        if (gunmanList[2] != null)
        {
            Destroy(gunmanList[2].gameObject);
        }
        foreach (var zombie in currentZombieList)
        {
            Vector3 targetPos = zombie.transform.position + ladderOffset + new Vector3(0, 0, -0.7f);
            zombie.transform.DOMove(targetPos, 1.5f).OnComplete(delegate
            {
                zombie.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            });
        }
        yield return new WaitForSeconds(2f);
        List<Zombie> successZoms = new List<Zombie>();
        for(int i = 0; i < currentZombieList.Count; i++)
        {
            int temp = i;
            int ladderLevel = Random.Range(1, 6);
            if (i == currentZombieList.Count - 1)
            {
                ladderLevel = 5;
            }
            Vector3 ladderHeight = new Vector3(0, ladderLevel * 2, 0);
            Vector3 targetPos = currentZombieList[i].transform.position + ladderHeight;
            currentZombieList[i].GetComponent<Animator>().Play("Climb");
            currentZombieList[i].transform.DOMove(targetPos, 1f * ladderLevel).OnComplete(delegate
            {
                if (ladderLevel != 5)
                {
                    currentZombieList[temp].GetComponent<Animator>().Play("Fall");
                    currentZombieList[temp].transform.DOMoveY(-0.5f, 1f);
                }
            });
            if (ladderLevel == 5)
            {
                successZoms.Add(currentZombieList[i]);
            }
        }

        if (successZoms.Count > 0)
        {
            GameData.LevelLadderLevel = 6;
        }
        
        yield return new WaitForSeconds(3.5f);
        targetPlayerPos = PlayerController.Instance.transform.position;
        targetPlayerPos += new Vector3(0, 0, 12f);
        PlayerController.Instance.transform.DOMoveZ(targetPlayerPos.z, 1f);
        CameraManager.Instance.SetCameraPositionAndOrientation(false);
        foreach (var zombie in successZoms)
        {
            zombie.GetComponent<Animator>().Play("Idle Walk Run Blend");
            Vector3 targetPos = zombie.transform.position + new Vector3(0, 0, 1f);
            zombie.transform.DOMoveZ(targetPos.z, 1f);
        }
        yield return new WaitForSeconds(1.15f);
        foreach (var zombie in successZoms)
        {
            Vector3 targetPos = zombie.transform.position;
            targetPos.y = -0.5f;
            zombie.transform.DOMoveY(targetPos.y, 1f);
        }
        yield return new WaitForSeconds(1f);
        foreach (var zombie in successZoms)
        {
            zombie.GetComponent<Animator>().Play("Idle Walk Run Blend");
            Vector3 normalXZomPos = zombie.transform.position;
            normalXZomPos.x = 0;
            Vector3 targetPos = normalXZomPos + new Vector3(0, 0.5f, 12f) + zombie.offsetPosToOriginal;
            zombie.transform.DOMove(targetPos, 2f).OnComplete(delegate
            {
                zombie.GetComponent<Animator>().Play("Dance");
            });
        }
        yield return new WaitForSeconds(2f);
        GameManager.Instance.m_WinEvent.Raise();
    }
}