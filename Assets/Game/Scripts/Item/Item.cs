using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Item : MonoBehaviour
{
    public int characterLayer;

    public Transform characterTrans;
    public bool isMoving = false;

    private void OnEnable()
    {
        characterTrans = null;
        isMoving = false;
    }

    private void Update()
    {
        if (!isMoving || characterTrans == null)
            return;
        transform.LookAt(characterTrans);
        transform.Translate(transform.forward * Time.deltaTime * 7.5f, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            if (characterTrans == null && !isMoving)
            {
                characterTrans = other.gameObject.transform;
                transform.DOJump(transform.position, 2, 1, 0.7f).OnComplete(delegate
                {
                    isMoving = true;
                });
            }
            else if (isMoving)
            {
                Destroy(gameObject);
            }
        }
    }
}
