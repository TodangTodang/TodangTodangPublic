using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class CustomerWear: Poolable
{
    [SerializeField] private SkinnedMeshRenderer BodyRenderer;
    [SerializeField] private SkinnedMeshRenderer FaceRenderer;

    [Header("Accessory")]
    [SerializeField] private Transform Head;
    [SerializeField] private Transform HeadWrap;
    [SerializeField] private Transform BodyUp;
    [SerializeField] private Transform BodyDown;
    [SerializeField] private Transform GlassesUp;
    [SerializeField] private Transform GlassesDown;
    [SerializeField] private Transform WeaponL;
    [SerializeField] private Transform WeaponR;

    private CustomerFaceDecoSetSO FaceSet;

    private List<GameObject> Accessaries;

    private ResourceManager _resourceManager;
    
    private void Awake()
    {
        Accessaries = new List<GameObject>();
    }

    public void SetBodyMesh(Mesh bodyMesh,Material bodyMaterial)
    {
        BodyRenderer.sharedMesh = bodyMesh;
        BodyRenderer.sharedMaterial = bodyMaterial;
    }

    public void SetFaceMesh(CustomerFaceDecoSetSO faceSet)
    {
        FaceSet = faceSet;
        FaceRenderer.sharedMaterial = FaceSet.GetFaceSet(Enums.FaceType.Normal);
    }

    public void SetFace(Enums.FaceType type)
    {
        FaceRenderer.sharedMaterial = FaceSet.GetFaceSet(type);
    }

    public void SetAccessory(GameObject prefab)
    {
        string prefabName = prefab.name;
        string prefabPath = Path.Combine("Customer/CustomerAccessories", prefabName);
        Transform targetParent = null;
        if (prefabName.StartsWith("A_BU"))
        {
            targetParent = BodyUp;
        }else if (prefabName.StartsWith("A_BD"))
        {
            targetParent = BodyDown;
        }
        else if (prefabName.StartsWith("A_HW"))
        {
            targetParent = HeadWrap;
        }
        else if (prefabName.StartsWith("A_H"))
        {
            targetParent = Head;
        }
        else if (prefabName.StartsWith("A_G"))
        {
            int choice = Random.Range(0, 2);
            if (choice == 1)
            {
                targetParent = GlassesUp;
            }
            else
            {
                targetParent = GlassesDown;
            }
        }
        else if (prefabName.StartsWith("A_W"))
        {
            int choice = Random.Range(0, 2);
            if (choice == 1)
            {
                targetParent = WeaponL;
            }
            else
            {
                targetParent = WeaponR;
            }
        }

        if (targetParent)
        {
            if (!_resourceManager)
            {
                _resourceManager = ResourceManager.Instance;
                Debug.Assert(_resourceManager,$"_resourceManager {Strings.DebugLog.INIT_PROBLEM}");
            }
            
            //GameObject obj = _resourceManager.Instantiate(prefabPath);
            GameObject obj = Instantiate(prefab);
            obj.transform.SetParent(targetParent);
            obj.transform.SetLocalPositionAndRotation(Vector3.zero,Quaternion.identity);
            obj.transform.localScale = Vector3.one;
            if (Accessaries == null)
                Accessaries = new List<GameObject>();
            Accessaries.Add(obj);
        }
            
    }

    public void OffAccessary()
    {
        foreach (var accessary in Accessaries)
        {
            Destroy(accessary);
            //_resourceManager.Destroy(accessary);
        }   
        Accessaries.Clear();
    }
}
