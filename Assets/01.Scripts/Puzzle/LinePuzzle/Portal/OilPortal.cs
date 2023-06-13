using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OilPortal : ConnectionPortal
{
    private NavMeshObstacle navMeshObstacle;

    [SerializeField]
    private Material material;

    [SerializeField]
    private Renderer starRenderer;

    [SerializeField]
    private List<ParticleSystem> particles;

    [SerializeField]
    private float distance = 2f;

    private Transform targetTransform;

    [SerializeField]
    private GameObject portal;

    private void Start()
    {
        navMeshObstacle = GetComponent<NavMeshObstacle>();
        targetTransform = PetManager.Instance.GetMyPetByKind<OilPet>().transform;
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

    protected override void SetColor(Color color)
    {
        material.SetColor("_EmissionColor", color);
        pointLight.color = color;

        starRenderer.material.color = color;

        particles.ForEach(x =>
        {
            ParticleSystem.MainModule main = x.main;
            main.startColor = color;
        });
    }
}
