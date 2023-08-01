using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TestZombieSpawnDeath : MonoBehaviour
{
    public GameObject zombiePrefab;
    
    [Button]
    public void Test()
    {
        Vector3 zOffsetNew = new Vector3(0, 0, 1f);
        var newZom = Instantiate(zombiePrefab, zOffsetNew, Quaternion.identity);
        newZom.tag = "Untagged";
        Quaternion newRot = Quaternion.Euler(0, 0, 0);
        newZom.transform.rotation = newRot;
        //newZom.GetComponent<CapsuleCollider>().enabled = false;

        Zombie newZomComponent = newZom.GetComponent<Zombie>();
        newZomComponent.isFinishRun = true;
        newZomComponent.animator = newZomComponent.GetComponent<Animator>();
        newZomComponent.animator.Play("Death");
        float pushForce = 250f;
        Rigidbody newZomRb = newZom.GetComponent<Rigidbody>();
        newZomRb.useGravity = true;
        newZomRb.mass = 3f;
        newZomRb.AddForce(new Vector3(Random.Range(-1f, 1f), 2f, 2f) * pushForce, ForceMode.Impulse);

        newZom.AddComponent<AutoDestroy>();
        newZom.GetComponent<AutoDestroy>().lifeTime = 2f;
    }
    
    [Button]
    public void TestCube()
    {
        float pushForce = 100f;
        Rigidbody newZomRb = transform.GetComponent<Rigidbody>();
        newZomRb.useGravity = true;
        newZomRb.AddForce(new Vector3(Random.Range(-1f, 1f), 1, 2f) * pushForce, ForceMode.Impulse);
    }
}
