using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
   #region Inspector Variables
   public Transform mTrans;
   public AudioSource source;
   #endregion

   #region Member Variables
   public int id { get; private set; }
   private AudioClip clip;
   private float duration;
   private bool isAttached = false;
   private GameObject parent;
   #endregion

   #region Unity Methods

   private void Awake()
   {
       if (isAttached)
       {
           if (parent == null || !parent.activeSelf)
           {
               isAttached = false;
           }
       }
   }

   #endregion

   #region Public Methods
   public void PlaySound(GameObject _parent, AudioClip _clip, bool _loop)
   {
       parent = _parent;
       mTrans.parent = parent.transform;
       mTrans.localPosition = Vector3.zero;
       id = parent.GetInstanceID();
       isAttached = true;
       clip = _clip;
       source.clip = clip;
       duration = clip.length;
       gameObject.SetActive(true);
       
       source.loop = _loop;
       source.Play();
   }
   #endregion

   #region Private Methods

   private void DetachSO()
   {
       parent = null;
       id = -1;
       if (SoundManager.Exist)
       {
           source.Stop();
           SoundManager.Instance.RecallSO(this);
       }
       else
       {
           DestroyImmediate(this.gameObject);
       }
   }
   #endregion
}
