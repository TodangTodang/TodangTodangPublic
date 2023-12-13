
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Customer/CustomerFaceSet", fileName = "CustomerFaceSet")]
public class CustomerFaceDecoSetSO : ScriptableObject
{
    [Header("Normal, Angry, Happy 순으로 배치")]
    [SerializeField] private Material[] faceSet;
    public Material GetFaceSet(Enums.FaceType type)
    {
        return faceSet[(int)type];
    }
}
