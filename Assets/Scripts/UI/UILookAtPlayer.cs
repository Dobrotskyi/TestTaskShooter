using UnityEngine;

public class UILookAtPlayer : MonoBehaviour
{
    private Transform _player;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        Vector3 playerPos = _player.transform.position;
        playerPos.y = transform.position.y;
        transform.LookAt(playerPos);
    }
}
