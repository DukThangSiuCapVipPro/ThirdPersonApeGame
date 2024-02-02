using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TruongNT;

public class Item : MonoBehaviour
{
    public int characterLayer;
    public ItemType type;
    public Rigidbody rigid;
    public Collider col;

    public Transform characterTrans;
    public bool isMoving = false;

    private void OnEnable()
    {
        characterTrans = null;
        isMoving = false;
        col.isTrigger = false;
        rigid.isKinematic = false;
        rigid.AddForce(new Vector3(Random.Range(-1, 1), 1, Random.Range(-1, 1)) * 3);
    }

    private void Update()
    {
        if (!isMoving || characterTrans == null)
            return;
        transform.LookAt(characterTrans);
        transform.Translate(transform.forward * Time.deltaTime * 7.5f, Space.World);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10)
        {
            rigid.isKinematic = true;
            col.isTrigger = true;
        }
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
                StartCoroutine(Tools.Delay(0.5f, delegate { isMoving = true; }));
            }
            else if (isMoving)
            {
                other.gameObject.SendMessage("ClaimItem", type);
                SimplePool.Despawn(gameObject);
            }
        }
    }
}

public enum ItemType
{
    None = 0,
    Exp = 1
}
