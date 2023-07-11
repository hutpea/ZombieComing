using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HyperCasual.Runner;
using TMPro;
using UnityEngine;

public class EndGameEvent2 : MonoBehaviour
{
    public GameObject ladderPrefab;
    public GameObject confettiPrefab;
    
    public Vector3 ladderOffset;

    private Vector3 finishLinePos;

    public List<GameObject> ladderTmpList;

    private List<Zombie> zombieList;

    public void StartGame()
    {
        //finishLinePos = GameManager.Instance.endLineObject.transform.position;
        GameObject finishLineObj = GameObject.FindGameObjectWithTag("Finish");
        if (finishLineObj != null)
        {
            finishLinePos = finishLineObj.transform.position;
        }

        float valueLadder = 1.0f;
        for (int i = 0; i <= 30; i++)
        {
            GameObject ladderObj = Instantiate(ladderPrefab);
            ladderObj.transform.position = finishLinePos + ladderOffset + new Vector3(0, 1f + i * 1.5f, 0);
            TextMeshPro textMesh = ladderObj.transform.GetComponentInChildren<TextMeshPro>();
            float ladderVal = valueLadder + 0.2f * i;
            textMesh.SetText("X" + ladderVal.ToString());
            ladderTmpList.Add(textMesh.gameObject);
            textMesh.gameObject.SetActive(false);
        }
    }

    public void Init()
    {
        PlayerController.Instance.ZombieFinishRun();
        StartCoroutine(EndGame());
    }

    private IEnumerator EndGame()
    {
        var currentZombieList = PlayerController.Instance.zombieList;
        zombieList = new List<Zombie>();
        zombieList = currentZombieList;
        //1.5f run to wall
        /*foreach (var zombie in currentZombieList)
        {
            zombie.GetComponent<Rigidbody>().isKinematic = true;
            Vector3 targetPos = zombie.transform.position;
            float xPos = Random.Range(-6.5f, 6.6f);
            targetPos = new Vector3(xPos, 0, finishLinePos.z + ladderOffset.z - 4f - (6 - Mathf.Abs(xPos)) / 2f - (6 - Mathf.Abs(xPos)) / 6f);
            
            zombie.transform.DOMove(targetPos, 1f).OnComplete(delegate
            {
                zombie.animator.SetFloat("Speed", 0f);
                zombie.animator.Play("Idle Walk Run Blend");
            });
        }*/


        yield return new WaitForSeconds(0.25f);

        int confettiCount = 3;
        float timePerJump = .2f;
        float zombieHeight = 1.5f;
        float height = 0f;
        //float timePerZom = 0.8f;
        float verticalOffset = 0f;
        float delayTime = 0f;
        int zombieClimbCount = 0;
        foreach (var zombie in currentZombieList)
        {
            zombieClimbCount++;
            bool isOverWall = false;
            if (zombieClimbCount > 30)
            {
                isOverWall = true;
            }

            zombie.animator.SetFloat("Speed", 0f);
            zombie.animator.Play(null);
            Vector3 groundPos = finishLinePos + ladderOffset + new Vector3(0, 0.6f, -1f);
            Vector3 targetPos = groundPos + new Vector3(0, height * zombieHeight, 0);
            if (isOverWall)
            {
                targetPos = groundPos + new Vector3(0, 30 * zombieHeight + 1.35f, 0);
            }
            
            Vector3 targetGrPosToRot = new Vector3(groundPos.x, zombie.transform.position.y, groundPos.z);
            zombie.transform.DOLookAt(targetGrPosToRot, 0.5f);

            zombie.transform.DOMove(groundPos + new Vector3(0, 0, verticalOffset), 1f).SetDelay(delayTime).OnStart(
                delegate
                {
                    zombie.animator.SetFloat("Speed", 5f);
                    zombie.animator.Play("Idle Walk Run Blend");
                }).OnComplete(delegate
            {
                zombie.animator.SetFloat("Speed", 0f);
                zombie.animator.Play("Climb");
                zombie.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                zombie.transform.DOMove(targetPos, timePerJump * height).OnComplete(delegate
                {
                    zombie.animator.SetFloat("Speed", 0);
                    zombie.animator.Play(null);
                    if (isOverWall)
                    {
                        zombie.animator.SetFloat("Speed", 0f);
                        zombie.animator.Play("Dance");
                        var dancePoint = zombie.transform.position + new Vector3(0, 0, 5f);
                        zombie.transform.DOMove(dancePoint, 1.5f).OnComplete(delegate
                        {
                            zombie.animator.Play("Dance");
                            Vector3 normalXZomPos = zombie.transform.position;
                            Vector3 targetPos = normalXZomPos + zombie.offsetPosToOriginal;
                            zombie.transform.DOMove(targetPos, 1f);

                            if (confettiCount > 0)
                            {
                                confettiCount--;
                                Instantiate(confettiPrefab, targetPos, Quaternion.identity);
                                confettiPrefab.transform.localScale = new Vector3(4f, 4f, 4f);
                            }
                        });
                    }
                    else
                    {
                        zombie.animator.SetFloat("Speed", 0);
                        zombie.animator.Play("Dance");
                    }
                });
            });
            height += 1;
            //timePerZom -= 0.03f;
            //verticalOffset -= 0.1f;
            delayTime += 0.15f;

            //yield return new WaitForSeconds(timePerZom);
        }

        yield return new WaitForSeconds(0.25f);
        Vector3 _tempPlayerPos = PlayerController.Instance.transform.position;
        if (currentZombieList.Count <= ladderTmpList.Count)
        {
            _tempPlayerPos.y += ladderTmpList[currentZombieList.Count - 1].transform.position.y;
        }
        else
        {
            _tempPlayerPos.y += ladderTmpList[ladderTmpList.Count - 1].transform.position.y;
        }

        /*Vector3 _tempPlayerPos = PlayerController.Instance.transform.position;
        _tempPlayerPos.y = zombieList[zombieList.Count - 1].transform.position.y + 5f;
        PlayerController.Instance.transform.DOMoveY(_tempPlayerPos.y, timePerJump * height).OnComplete(
            delegate
            {
                if (zombieList.Count <= ladderTmpList.Count)
                {
                    ladderTmpList[zombieList.Count - 1].SetActive(true);
                }
                else
                {
                    ladderTmpList[ladderTmpList.Count - 1].SetActive(true);
                }
            });
        CameraManager.Instance.SetCameraPositionAndOrientation(false);*/

        /*_tempPlayerPos = PlayerController.Instance.transform.position;
        if (currentZombieList.Count > 30)
        {
            _tempPlayerPos.z += 5f;
            PlayerController.Instance.transform.DOMoveZ(_tempPlayerPos.z, 1f);
        }*/

        yield return new WaitForSeconds(delayTime + timePerJump * height - 0.25f);

        if (zombieList.Count <= ladderTmpList.Count)
        {
            ladderTmpList[zombieList.Count - 1].SetActive(true);
        }
        else
        {
            ladderTmpList[ladderTmpList.Count - 1].SetActive(true);
        }

        float totalHeight = (height - 1) * zombieHeight;
        int levelMultiplier = (int)(totalHeight / 2f);
        GameData.LevelLadderLevel = levelMultiplier;

        if (currentZombieList.Count > 30)
        {
            yield return new WaitForSeconds(1.5f);
        }

        yield return new WaitForSeconds(3f);
        _tempPlayerPos = PlayerController.Instance.transform.position;
        _tempPlayerPos += new Vector3(0, -2, 2f);
        PlayerController.Instance.transform.DOMove(_tempPlayerPos, 1f);
        CameraManager.Instance.SetCameraPositionAndOrientation(false);
        yield return new WaitForSeconds(2f);

        zombieList = null;
        
        GameManager.Instance.m_WinEvent.Raise();
    }

    private void Update()
    {
        if (zombieList != null)
        {
            if (zombieList.Count == 0) return;
            Vector3 _tempPlayerPos = PlayerController.Instance.transform.position;
            _tempPlayerPos.y = zombieList[zombieList.Count - 1].transform.position.y + 5f;
            PlayerController.Instance.transform.position = _tempPlayerPos;
            CameraManager.Instance.SetCameraPositionAndOrientation(false);
        }
    }
}