using UnityEngine;


public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform targetPosition;
    [SerializeField] private float speed = 3;

    private void Start()
    {
        if (!targetPosition)
        {
            Debug.LogError($"{name}: Target is null!");
            Debug.LogError($"Disabling component");
            enabled = false;
        }
    }

    private void Update()
    {
        if (targetPosition == null)
            Debug.LogError($"{name}: Target is null!");

        else
        {
            Vector3 currentPosition = transform.position;
            Vector3 nextPosition = targetPosition.position;

            Vector3 directionToNextPos = nextPosition - currentPosition;
            directionToNextPos.Normalize();

            Move(directionToNextPos);
        }
    }


    private void Move(Vector3 direction)
    {
        Vector3 horizontalDirection = new Vector3(direction.x, 0, direction.z);

        transform.rotation = Quaternion.LookRotation(horizontalDirection, Vector3.up);

        transform.position += horizontalDirection * speed * Time.deltaTime;
    }

}
