using System.Collections;
using UnityEngine;

public class Bunny00Controller : MonoBehaviour
{
    private Animator _animator;
    private Enums.Bunny00State _currentState = Enums.Bunny00State.WalkingToFridge;

    public Transform fridgeLocation;
    public Transform dishesLocation;

    private float _moveSpeed = 3f;
    private float _rotationSpeed = 90f;

    private int _isWalking;
    private int _searchFridge;
    private int _washDishes;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        _isWalking = Animator.StringToHash("isWalking");
        _searchFridge = Animator.StringToHash("SearchFridge");
        _washDishes = Animator.StringToHash("WashDishes");
    }

    private void Update()
    {
        switch (_currentState)
        {
            case Enums.Bunny00State.WalkingToFridge:
                MoveToLocation(fridgeLocation, Enums.Bunny00State.TurningFromFridge, _isWalking);
                break;

            case Enums.Bunny00State.TurningFromFridge:
                SetYRotation(Enums.Bunny00State.SearchingFridge, _isWalking, _searchFridge);
                break;

            case Enums.Bunny00State.SearchingFridge:
                SearchLocation(Enums.Bunny00State.WalkingToDishes, _searchFridge, _isWalking);
                break;

            case Enums.Bunny00State.WalkingToDishes:
                MoveToLocation(dishesLocation, Enums.Bunny00State.TurningFromDishes, _isWalking);
                break;

            case Enums.Bunny00State.TurningFromDishes:
                SetYRotation(Enums.Bunny00State.WashingDishes, _isWalking, _washDishes);
                break;

            case Enums.Bunny00State.WashingDishes:
                SearchLocation(Enums.Bunny00State.WalkingToFridge, _washDishes, _isWalking);
                break;
        }
    }

    private void MoveToLocation(Transform targetLocation, Enums.Bunny00State nextState, int param)
    {
        RotateTowards(targetLocation.position);

        Vector3 nextPosition = Vector3.MoveTowards(transform.position, targetLocation.position, _moveSpeed * Time.deltaTime);

        transform.position = nextPosition;

        if (Vector3.Distance(transform.position, targetLocation.position) < 0.1f)
        {
            _animator.SetBool(param, false);
            _currentState = nextState;
        }
        else
        {
            _animator.SetBool(param, true);
        }
    }

    private void SearchLocation(Enums.Bunny00State nextState, int param, int changeParam)
    {
        StartCoroutine(CompleteSearchAnimation(nextState, param, changeParam));
    }

    private IEnumerator CompleteSearchAnimation(Enums.Bunny00State nextState, int param, int changeParam)
    {
        yield return new WaitForSeconds(5f);

        _animator.SetBool(param, false);
        _animator.SetBool(changeParam, true);
        _currentState = nextState;
    }

    private void SetYRotation(Enums.Bunny00State nextState, int param, int changeParam)
    {
        Quaternion targetRotation = Quaternion.Euler(0f, 0, 0f);
        SmoothRotate(targetRotation, param, changeParam);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
        {
            _currentState = nextState;
            _animator.SetBool(param, false);
            _animator.SetBool(changeParam, true);
        }
    }

    private void RotateTowards(Vector3 targetPosition)
    {
        Vector3 targetDirection = (targetPosition - transform.position).normalized;

        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    private void SmoothRotate(Quaternion targetRotation, int param, int changeParam)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
        {
            _animator.SetBool(param, false);
            _animator.SetBool(changeParam, true);
        }
    }
}
