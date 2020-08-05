using DG.Tweening;
using UnityEngine;

public class SimpleBoatController : MonoBehaviour
{
    #region Inspector

    [SerializeField]
    private Rigidbody physics = default;

    [SerializeField]
    [Min(1)]
    private float speed = default;

    [SerializeField]
    private ParticleSystem smokeParticles = default;

    public Vector3 worldPosition;
    #endregion


    #region Fields

    private float _force;

    private float _direction;

    #endregion


    #region MonoBehaviour

    private void Update ()
    {

        float desiredForce = 0;
        if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, 0);
            float distance;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out distance))
            {
                worldPosition = ray.GetPoint(distance);
            }
            transform.DOLookAt(worldPosition, 2);

            desiredForce += 1;
        }
        if (Input.GetMouseButton(1))
        {
            Plane plane = new Plane(Vector3.up, 0);
            float distance;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out distance))
            {
                worldPosition = ray.GetPoint(distance);
            }
            transform.DOLookAt(worldPosition, 2);

            desiredForce -= 1;
        }

        // Force input
        if ( Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) )
            desiredForce += 1;
        if ( Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) )
            desiredForce -= 1;

        // Direction input
        float desiredDirection = 0;
        if ( Input.GetKey(KeyCode.RightArrow)|| Input.GetKey(KeyCode.D) )
            desiredDirection += 1;
        if ( Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) )
            desiredDirection -= 1;

        // Acceleration
        _force = Mathf.SmoothStep(_force, desiredForce, Time.deltaTime * 10);
        _direction =
            Mathf.SmoothStep(_direction, desiredDirection, Time.deltaTime * 4);
    }

    private void FixedUpdate ()
    {
        // Velocity relative to local space
        Vector3 velocity = new Vector3(0, 0, _force * speed);
        physics.velocity = physics.transform.TransformDirection(velocity);

        // Torque
        Vector3 torque = new Vector3(0, _direction * speed, 0);
        physics.angularVelocity = torque;
    }

    private void OnTriggerEnter (Collider other)
    {
        if ( other.CompareTag("Bridge") )
            smokeParticles.Stop();
    }

    private void OnTriggerExit (Collider other)
    {
        if ( other.CompareTag("Bridge") )
            smokeParticles.Play();
    }

    #endregion
}
