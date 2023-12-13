using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
    [SerializeField] private float _fixedPos;
    private LayerMask _rail;
    private RaycastHit _currentRail;
    private Vector3 _direction;
    private float _maxSpeed;
    private float _speed;

    [SerializeField] private Movable _detectedObject;
    private float _minDistance = 0.3f;

    private void Start()
    {
        GameManager manager = GameManager.Instance;
        PlayerData playerData = manager.GetPlayerData();

        List<KitchenUtensilInfoData> utensilList = playerData.GetInventory<KitchenUtensilInfoData>();
        KitchenUtensilInfoData _infoData = utensilList.Find(x => x.DefaultData.name == "Rail");
        _maxSpeed = _infoData.DefaultData.SpeedUpgradeInfo[_infoData.Level] * 0.1f;
        _rail = LayerMask.GetMask("Conveyor");
    }

    private void FixedUpdate()
    {
        if (_detectedObject == null)
        {
            _speed = _maxSpeed;
        }
        else
        {
            float distance = Vector3.Distance(transform.position, _detectedObject.gameObject.transform.position);
            float t = Mathf.InverseLerp(0f, _minDistance, distance);
            _speed = Mathf.Lerp(_maxSpeed, 0.3f, t);
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 0.5f, _rail))
        {

            if (hit.collider != _currentRail.collider)
            {
                SetCurrentRail(hit);
            }
            float step = _speed * Time.fixedDeltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _direction, step);
        }
    }

    private void SetCurrentRail(RaycastHit hit)
    {
        BoxCollider collider = hit.collider as BoxCollider;

        float posX = collider.center.x;
        float posY = _fixedPos;
        float posZ = collider.center.z + -collider.size.z;

        _currentRail = hit;
        _direction = _currentRail.transform.position + _currentRail.transform.TransformDirection(posZ, posY, posX);
    }

    private void OnTriggerEnter(Collider other)
    {
        Movable movable = null;
        other.TryGetComponent<Movable>(out movable);

        if (movable == null) return;

        Vector3 thisToDirection = (_direction - this.transform.position).normalized;
        Vector3 thisToDetected = (movable.transform.position - this.transform.position).normalized;

        float dotProduct = Vector3.Dot(thisToDirection, thisToDetected);

        if (dotProduct > 0.8f)
        {
            _detectedObject = movable;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_detectedObject == null) return;

        if (other.gameObject ==  _detectedObject.gameObject)
        {
            _detectedObject = null;
        }
    }
}