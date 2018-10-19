﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDatabase : MonoBehaviour {

    public int playerCount;
    private List<GameObject>[] playerObjects;
	
	void Awake ()
    {
        playerObjects = new List<GameObject>[playerCount];
        for(int i = 0; i < playerCount; i++)
        {
            playerObjects[i] = new List<GameObject>();
        }
	}
	

    public void AddToPlayer(GameObject target, int player)
    {
        List<GameObject> playerList = playerObjects[player];

        if (playerList == null)
        {
            throw new System.Exception("Player not found");
        }
        else if(playerList.Find(x => x.Equals(target)) == null)
        {
            playerList.Add(target);
        }
    }

    public void RemoveFromPlayer(GameObject target, int player)
    {
        List<GameObject> playerList = playerObjects[player];

        if (playerList == null)
        {
            throw new System.Exception("Player not found");
        }
        else if (playerList.Find(x => x.Equals(target)) == null)
        {
            playerList.Remove(target);
        }
    }

    public bool IsFromPlayer(GameObject obj, int player)
    {
        List<GameObject> playerList = playerObjects[player];

        if(playerList == null)
        {
            throw new System.Exception("Player not found");
        }
        else
        {
            GameObject encontrado = playerList.Find(x => x.Equals(obj));
            return encontrado != null;
        }

    }

    public bool AreFromSamePlayer(GameObject obj_a, GameObject obj_b)
    {
        int obj_a_player = getObjectPlayer(obj_a);
        int obj_b_player = getObjectPlayer(obj_b);
        return obj_a_player == obj_b_player;
    }

    public int getObjectPlayer(GameObject obj)
    {
        for(int i = 0; i < playerObjects.Length; i++)
        {
            GameObject encontrado = playerObjects[i].Find(x => x.Equals(obj));
            if(encontrado != null)
            {
                return i;
            }
        }

        return -1;
    }
    

}