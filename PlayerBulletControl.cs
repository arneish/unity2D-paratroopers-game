using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletControl : MonoBehaviour {

    // Use this for initialization
    //public Transform Weapon;
    public float speedY, speedX;
    void Start () {
        //speedY = 6f;
        //transform.rotation = Weapon.transform.rotation;
    }

    // Update is called once per frame
    void Update () {
        //transform.rotation = Weapon.transform.rotation;
        float rotationCoordinate = transform.eulerAngles.z - 7f;
        rotationCoordinate += 45f;
        //Debug.Log("tranform.eulerAngles.z:" + rotationCoordinate);
        rotationCoordinate = rotationCoordinate * Mathf.Deg2Rad;
        float slope = Mathf.Tan (rotationCoordinate);
        float slopeZero = Mathf.Abs (slope);
        if (slopeZero < 0.1) {
            speedX = 0f;
        } else {
            speedX = speedY / slope;
        }
        //Debug.Log("slope:" + slope);
        Vector2 position = transform.position;
        position = new Vector2 (position.x + speedX * Time.deltaTime, position.y + speedY * Time.deltaTime);
        transform.position = position;
        transform.Rotate (0, 20 * Time.deltaTime, 0);
        Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2 (1, 1));
        if (transform.position.y > max.y) {
            Destroy (gameObject);
        }
    }
}