using UnityEngine;

public class AlwaysFaceObj : MonoBehaviour
{
    public Transform target;
    // Update is called once per frame
    void Update()
    {
        Vector3 direction = transform.position - target.position;
        direction.y = 0; // Keep the y-axis rotation fixed
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
}
