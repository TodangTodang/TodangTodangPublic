using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Customer/CustomerDecoData", fileName = "CustomerDecoData")]
public class CustomerDecoDataSO : ScriptableObject
{
    [field: SerializeField] public Mesh BodyMesh { get; private set; }
    [field: SerializeField] public Material BodyMaterial { get; private set; }
    [field: SerializeField] public CustomerFaceDecoSetSO FaceSet { get; private set; }
    [field: SerializeField] public GameObject[] Accessories { get; private set; }
}
