using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OilPortal : ConnectionPortal
{
    private NavMeshObstacle navMeshObstacle;

    private void Start()
    {
        navMeshObstacle = GetComponent<NavMeshObstacle>();
    }

    protected override void OnContactPet(Pet pet)
    {
        //pet.AgentEnabled(false);
        //pet.transform.position = LinePuzzleController.CurrentPiece.transform.position;
        //pet.AgentEnabled(true);

        //OilPet oilPet = pet as OilPet;
        //oilPet.SpreadOil();
    }

    public void ObstacleOff()
    {
        navMeshObstacle.enabled = false;
    }
}
