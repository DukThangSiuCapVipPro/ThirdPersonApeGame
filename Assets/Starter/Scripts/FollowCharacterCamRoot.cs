using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCharacterCamRoot : MonoBehaviour
{
    public Transform characterTrans;

    private void Update()
    {
        transform.position = new Vector3(characterTrans.transform.position.x, 10, characterTrans.transform.position.z);
    }
}
