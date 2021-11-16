using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInitialize : MonoBehaviour
{
    void Start()
    {
        InitializePlayerView();
    }

    private void InitializePlayerView()
    {
        var player = Pool.Instance.Get(PrefabRefs.Instanse.Player);
        //Instantiate(player, new Vector2(0, 0), Quaternion.identity); 
    }
}
