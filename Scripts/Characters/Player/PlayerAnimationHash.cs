using System;
using UnityEngine;

[Serializable]
public class PlayerAnimationHash
{

    [SerializeField] private string stayParameterName = "@Stay";
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string pickUpParameterName = "PickUp";
    [SerializeField] private string putDownParameterName = "PutDown";
    [SerializeField] private string holdParameterName = "Hold";
    [SerializeField] private string interactionParameterName = "Interaction";

    [SerializeField] private string moveParameterName = "@Move";
    [SerializeField] private string walkParameterName = "Walk";
    [SerializeField] private string holdAndWalkParameterName = "HoldAndWalk";

    public int StayParameterHash { get; private set; }
    public int IdleParameterHash { get; private set; }
    public int PickUpParameterHash { get; private set; }
    public int PutDownParameterHash { get; private set; }
    public int HoldParameterHash { get; private set; }
    public int InteractionParameterHash { get; private set; }

    public int MoveParameterHash { get; private set; }
    public int WalkParameterHash { get; private set; }
    public int HoldAndWalkParameterHash { get; private set; }

    public void Initialize()
    {
        StayParameterHash = Animator.StringToHash(stayParameterName);
        IdleParameterHash = Animator.StringToHash(idleParameterName);
        PickUpParameterHash = Animator.StringToHash(pickUpParameterName);
        PutDownParameterHash = Animator.StringToHash(putDownParameterName);
        HoldParameterHash = Animator.StringToHash(holdParameterName);
        InteractionParameterHash = Animator.StringToHash(interactionParameterName);


        MoveParameterHash = Animator.StringToHash(moveParameterName);
        WalkParameterHash = Animator.StringToHash(walkParameterName);
        HoldAndWalkParameterHash = Animator.StringToHash(holdAndWalkParameterName);
       
    }
}
