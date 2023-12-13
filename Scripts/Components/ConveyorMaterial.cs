using System.Collections.Generic;
using UnityEngine;

public class ConveyorMaterial : MonoBehaviour
{
    [SerializeField] private float speed, conveyorSpeed;
    [SerializeField] private List<GameObject> pots;
    [SerializeField] private Vector3 moveDirection;
    

    private Material targetMaterial;

    private void Start()
    {
        targetMaterial = GetComponent<MeshRenderer>().materials[2];
    }

    private void FixedUpdate()
    {
        if (targetMaterial.mainTextureOffset.x > 1)
        {
            targetMaterial.mainTextureOffset = new Vector2(0, 0.5f);
        }
        targetMaterial.mainTextureOffset += new Vector2(1, 0) * conveyorSpeed * Time.deltaTime;
    }
}
