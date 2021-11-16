using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabRefs : MonoBehaviour
{
    public GameObject Player;
    public static PrefabRefs Instanse { get; private set; }

    private void Awake()
    {
        Instanse = this;
    }
}
