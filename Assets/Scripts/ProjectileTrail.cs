using UnityEngine;

public class ProjectileTrail : MonoBehaviour
{
    [SerializeField] private float _speed = 200f;
    private Vector3 _destination;

    public void Setup(Vector3 destination)
    {
        _destination = destination;
    }

    private void Update()
    {
        float distanceBefore = Vector3.Distance(transform.position, _destination);

        Vector3 moveDir = (_destination - transform.position).normalized;
        transform.position += moveDir * _speed * Time.deltaTime;

        float distanceAfter = Vector3.Distance(transform.position, _destination);

        if (distanceBefore < distanceAfter)
            Destroy(gameObject);
    }
}
