using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    public Transform player;
    
    public TextMeshProUGUI numberZombiesTMP;

    public void Setup(Transform playerTrans)
    {
        player = playerTrans;
    }
    // Update is called once per frame
    void Update()
    {
        var temp = transform.position;
        temp.x = player.transform.position.x;
        temp.z = player.transform.position.z;
        transform.position = temp;
    }
}
