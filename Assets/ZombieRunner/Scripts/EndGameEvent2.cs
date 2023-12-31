using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HyperCasual.Runner;
using TMPro;
using UnityEngine;

public class EndGameEvent2 : MonoBehaviour
{
    public GameObject wallPrefab;
    public GameObject confettiPrefab;

    public Vector3 wallOffset;

    private Vector3 finishLinePos;

    private List<Zombie> zombieList;
    private EndGameWall endGameWall;

    public void StartGame()
    {
        //finishLinePos = GameManager.Instance.endLineObject.transform.position;
        GameObject finishLineObj = GameObject.FindGameObjectWithTag("Finish");
        if (finishLineObj != null)
        {
            finishLinePos = finishLineObj.transform.position;
        }

        var wallObj = Instantiate(wallPrefab, finishLinePos + wallOffset, Quaternion.identity);
        wallObj.transform.rotation = Quaternion.Euler(-90, 90, 0);
        endGameWall = wallObj.GetComponent<EndGameWall>();
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

        float timePerJump = .2f;
        float delayTime = 0f;
        int zombieIndex = 0;
        foreach (var zombie in currentZombieList)
        {
            zombie.wallIndex = zombieIndex;
            zombieIndex++;
        }

        foreach (var zombie in currentZombieList)
        {
            bool isOverWall = false;
            if (zombie.wallIndex >= 30)
            {
                isOverWall = true;
            }

            Vector3 initPos = zombie.transform.position;
            initPos += new Vector3(0, 0, -10f);
            zombie.transform.position = initPos;

            zombie.animator.SetFloat("Speed", 5f);
            zombie.animator.Play("Idle Walk Run Blend");

            zombie.transform.DOLookAt(endGameWall.wallPoints[0].position, 0.5f);

            zombie.transform.DOMove(endGameWall.wallPoints[0].position, 1f).SetDelay(delayTime).OnComplete(delegate
            {
                zombie.animator.SetFloat("Speed", 0f);
                zombie.animator.Play("Climb");
                zombie.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

                if (isOverWall)
                {
                    zombie.transform.DOMove(endGameWall.topPoint.position, timePerJump * 25).OnComplete(delegate
                    {
                        zombie.animator.SetFloat("Speed", 5f);
                        zombie.animator.Play("Idle Walk Run Blend");
                        Vector3 targetPos = endGameWall.dancePoint.position + zombie.offsetPosToOriginal;
                        zombie.transform.DOMove(targetPos, 1.5f).OnComplete(delegate
                        {
                            zombie.animator.SetFloat("Speed", 0f);
                            zombie.animator.Play("Dance");
                        });
                    });
                }
                else
                {
                    zombie.transform.DOMove(endGameWall.wallPoints[zombie.wallIndex].position,
                            timePerJump * zombie.wallIndex)
                        .OnComplete(
                            delegate
                            {
                                zombie.animator.SetFloat("Speed", 0);
                                zombie.animator.Play("Dance");
                                zombie.zombieRigidbody.isKinematic = true;
                                zombie.zombieRigidbody.constraints = RigidbodyConstraints.FreezePosition;

                                switch (zombie.wallIndex)
                                {
                                    case 4:
                                    {
                                        endGameWall.SetLight(1);
                                        break;
                                    }
                                    case 9:
                                    {
                                        endGameWall.SetLight(2);
                                        break;
                                    }
                                    case 14:
                                    {
                                        endGameWall.SetLight(3);
                                        break;
                                    }
                                    case 20:
                                    {
                                        endGameWall.SetLight(4);
                                        break;
                                    }
                                    case 29:
                                    {
                                        endGameWall.SetLight(5);
                                        break;
                                    }
                                    default: break;
                                }
                            });
                }
            });

            delayTime += 0.15f;
        }

        yield return new WaitForSeconds(0.25f);

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

        yield return new WaitForSeconds(delayTime + timePerJump * Mathf.Min(zombieList.Count, 30) - 0.25f);

        if (currentZombieList.Count > 30)
        {
            yield return new WaitForSeconds(1.5f);
        }

        yield return new WaitForSeconds(1.5f);
        var _tempPlayerPos = PlayerController.Instance.transform.position;
        _tempPlayerPos += new Vector3(0, -2, 2f);
        PlayerController.Instance.transform.DOMove(_tempPlayerPos, 1f);
        CameraManager.Instance.SetCameraPositionAndOrientation(false);

        foreach (var zom in zombieList)
        {
            int randN = UnityEngine.Random.Range(0, 100);
            if (randN > 50)
            {
                Instantiate(confettiPrefab, zom.transform.position + new Vector3(0, 0, -0.5f), Quaternion.identity);
                confettiPrefab.transform.localScale = new Vector3(4f, 4f, 4f);
            }
        }

        yield return new WaitForSeconds(1.5f);

        zombieList = null;

        GameManager.Instance.m_WinEvent.Raise();
    }

    private void Update()
    {
        if (zombieList != null)
        {
            if (zombieList.Count == 0) return;
            Vector3 _tempPlayerPos = PlayerController.Instance.transform.position;
            if (zombieList.Count > 30)
            {
                _tempPlayerPos.y = zombieList[30].transform.position.y + 5f;
            }
            else
            {
                _tempPlayerPos.y = zombieList[zombieList.Count - 1].transform.position.y + 5f;
            }
            PlayerController.Instance.transform.position = _tempPlayerPos;
            CameraManager.Instance.SetCameraPositionAndOrientation(false);
        }
    }
}