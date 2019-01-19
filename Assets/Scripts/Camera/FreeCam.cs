using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCam : MonoBehaviour 
{
    public float motionSpeed = 10f;
    public float rotationSpeed = 180f;
    Vector3 _motion;
    Vector2 _rotation;

    void Start()
    {
        _rotation.x = transform.rotation.eulerAngles.x;
        _rotation.y = transform.rotation.eulerAngles.y;
    }

	void Update()
    {
        _motion.x = Input.GetAxis("Horizontal");
        _motion.y = 0f;
        _motion.z = Input.GetAxis("Vertical");

        _motion.Normalize();

        transform.Translate(_motion * motionSpeed * Time.deltaTime);

        _rotation.y += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        _rotation.x += Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime * -1f;
        _rotation.y %= 360f;
        _rotation.x %= 360f;

        transform.rotation = Quaternion.Euler(_rotation);
    }
}
