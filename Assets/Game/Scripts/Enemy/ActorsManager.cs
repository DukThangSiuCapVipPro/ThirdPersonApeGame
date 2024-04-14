using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorsManager : SingletonMonoBehaviour<ActorsManager>
{
    public List<Actor> Actors { get; private set; }
    public GameObject Player { get; private set; }

    public void SetPlayer(GameObject player) => Player = player;

    public override void Awake()
    {
        base.Awake();
        Actors = new List<Actor>();
    }
}
