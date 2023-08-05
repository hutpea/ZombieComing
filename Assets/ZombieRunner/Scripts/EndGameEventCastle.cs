using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HyperCasual.Runner;
using TMPro;
using UnityEngine;

public class EndGameEventCastle : MonoBehaviour
{
    public List<GameObject> castlePrefabs;
    private CastleObject _castleObject;
    private Vector3 finishLinePos;

    public void StartGame()
    {
        GameObject finishLineObj = GameObject.FindGameObjectWithTag("Finish");
        if (finishLineObj != null)
        {
            finishLinePos = finishLineObj.transform.position;
        }

        var castleObj = Instantiate(castlePrefabs[Mathf.Min(GameData.CurrentCastleIndex + 1, 3)]);
        _castleObject = castleObj.GetComponent<CastleObject>();
        castleObj.transform.position = finishLinePos + new Vector3(0, 0, 10f);
    }

    public void Init()
    {
        PlayerController.Instance.ZombieFinishRun();
        StartCoroutine(EndGame());
    }

    private IEnumerator EndGame()
    {
        var currentZombieList = PlayerController.Instance.zombieList;

        float zombieHeight = 1.5f;

        float delayTime = 0f;

        int count;

        int requireZomsToBeat = 0;
        if (GameData.CurrentCastleIndex == -1)
        {
            requireZomsToBeat = 10;
        }
        else if (GameData.CurrentCastleIndex == 0)
        {
            requireZomsToBeat = 20;
        }
        else if (GameData.CurrentCastleIndex == 1)
        {
            requireZomsToBeat = 30;
        }
        else if (GameData.CurrentCastleIndex == 2)
        {
            requireZomsToBeat = 40;
        }

        if (currentZombieList.Count > requireZomsToBeat)
        {
            foreach (var zombie in currentZombieList)
            {
                zombie.animator.SetFloat("Speed", 12f);
                float xOffset = Random.Range(-2f, 2f);
                zombie.transform.DOMove(_castleObject.groundPoint.position + new Vector3(xOffset, 0, 0), 1f)
                    .SetDelay(delayTime).OnComplete(delegate
                    {
                        zombie.animator.Play("Climb");
                        zombie.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                        zombie.transform.DOMove(_castleObject.topWallPoint.position, 1f).OnComplete(delegate
                        {
                            zombie.animator.Play("Fall");
                            zombie.transform.DOMove(_castleObject.groundPoint.position + new Vector3(0, 0, 0.5f), 1f)
                                .OnComplete(delegate
                                {
                                    zombie.animator.SetFloat("Speed", 10f);
                                    zombie.animator.Play("Idle Walk Run Blend");
                                    zombie.transform.DOMove(_castleObject.dancePoint.position, 1.5f).OnComplete(delegate
                                    {
                                        zombie.animator.SetFloat("Speed", 0f);
                                        zombie.animator.Play("Dance");
                                        Vector3 normalXZomPos = zombie.transform.position;
                                        Vector3 targetPos = normalXZomPos + zombie.offsetPosToOriginal;
                                        zombie.transform.DOMove(targetPos, 1f);
                                    });
                                });
                        });
                    });
                delayTime += 0.1f;
            }

            yield return new WaitForSeconds(1f);
            Vector3 _tempPlayerPos = PlayerController.Instance.transform.position;
            _tempPlayerPos.z += 10f;
            PlayerController.Instance.transform.DOMoveZ(_tempPlayerPos.z, 1f);
            CameraManager.Instance.SetCameraPositionAndOrientation(false);

            yield return new WaitForSeconds(1.5f);
            _tempPlayerPos = PlayerController.Instance.transform.position;
            _tempPlayerPos.z += 10f;
            PlayerController.Instance.transform.DOMoveZ(_tempPlayerPos.z, 1f);
            CameraManager.Instance.SetCameraPositionAndOrientation(false);

            yield return new WaitForSeconds(4f + currentZombieList.Count * 0.1f);
            
            GameManager.Instance.m_WinEvent.Raise();

            GameData.CurrentCastleIndex++;
        }
        else
        {
            //Lose
            for (var index = 0; index < currentZombieList.Count; index++)
            {
                var zombie = currentZombieList[index];
                zombie.wallIndex = index;
            }

            int maxZom = currentZombieList.Count;
            float heightFactor = 1.5f;

            bool isLose = false;
            for (var index = 0; index < currentZombieList.Count; index++)
            {
                var zombie = currentZombieList[index];
                zombie.animator.SetFloat("Speed", 12f);
                float xOffset = Random.Range(-2f, 2f);
                zombie.transform.DOMove(_castleObject.groundPoint.position + new Vector3(xOffset, 0, 0), 1f)
                    .SetDelay(delayTime).OnComplete(delegate
                    {
                        zombie.animator.Play("Climb");
                        zombie.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                        zombie.transform.DOMoveY(_castleObject.groundPoint.position.y + zombie.wallIndex * heightFactor, 1f).OnComplete(delegate
                        {
                            zombie.animator.Play("Fall");
                            zombie.transform.DOMove(_castleObject.groundPoint.position + new Vector3(0, 0, -0.25f), 0.5f)
                                .OnComplete(delegate
                                {
                                    zombie.animator.Play("Death");
                                    if (zombie.wallIndex >= maxZom - 1)
                                    {
                                        if (!isLose)
                                        {
                                            isLose = true;
                                            GameManager.Instance.m_LoseEvent.Raise();
                                        }
                                    }
                                });
                        });
                    });
                delayTime += 0.1f;
            }
            
            yield return new WaitForSeconds(4f + currentZombieList.Count * 0.1f);
            if (!isLose)
            {
                isLose = true;
                GameManager.Instance.m_LoseEvent.Raise();
            }
        }
    }
}