using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChangeLocation : MonoBehaviour
{
    public Transform atrioLocation, lobbyLocation;
    public GameObject buttonAtrio, buttonLobby;
    public NavMeshAgent avatarNavMesh;
    private bool buttonsActivated;

    private void Start()
    {
        buttonsActivated = false;
    }

    public void ActivateButtons()
    {
        buttonsActivated = !buttonsActivated;

        if (buttonsActivated)
        {
            buttonAtrio.SetActive(true);
            buttonLobby.SetActive(true);
        }
        else
        {
            buttonAtrio.SetActive(false);
            buttonLobby.SetActive(false);
        }
    }

    public void gotAtrio()
    {
        avatarNavMesh.Warp(atrioLocation.position);
        avatarNavMesh.gameObject.transform.rotation = atrioLocation.rotation;
    }

    public void gotLobby()
    {
        avatarNavMesh.Warp(lobbyLocation.position);
        avatarNavMesh.gameObject.transform.rotation = lobbyLocation.rotation;
    }
}
