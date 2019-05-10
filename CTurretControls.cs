using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTurretControls : MonoBehaviour {
    public float RotateSpeed = 2.5f;
    public Vector3 zAxis = new Vector3 (0, 0, 1);
    public static Vector3 point;
    public Vector3 POSITION;
    public Quaternion ROTATION;
    public float extremeLeft;
    public float extremeRight;
    //Bullets:
    public GameObject playerBullet01;
    public GameObject bulletPosition01;
    public GameObject bulletPosition02;
    public AudioSource fireBullet;
    public float recoilSpeed;
    public float maxRecoil_z = -.2f;
    public float recoil = 0;
    public int maxArsenalSize = 100;
    public int arsenalSize;
    //GUI:
    public GUISkin theSkin;
    public GUIStyle TextStyle;
    public GUIStyle TextStyleReload;
    public bool Reload;
    //Time hold;
    float lastStep;
    float timePressed;
    float timeBetweenSteps;
    //Audio for Reload:
    public AudioSource ReloadSound;

    private void Start () {
        point = new Vector3 (0, -4.52f, 0);
        arsenalSize = maxArsenalSize;
        extremeLeft = 87;
        extremeRight = 2;
        recoilSpeed = 5;
        arsenalSize = 100;
        Reload = false;
        /*time*/
        lastStep = Time.time;
        timeBetweenSteps = 0.15f;
    }

    void Update () {
        bool steerLeft = Input.GetKey (KeyCode.LeftArrow);
        bool steerRight = Input.GetKey (KeyCode.RightArrow);
        //this.transform.RotateAround(point, zAxis, RotateSpeed);
        if (steerLeft) {
            if (this.transform.eulerAngles.z < extremeLeft) {
                this.transform.RotateAround (point, zAxis, RotateSpeed);
            }
        } else if (steerRight) {
            if (this.transform.eulerAngles.z > extremeRight) {
                this.transform.RotateAround (point, zAxis, -RotateSpeed);
            }
        } else {
            this.transform.RotateAround (point, zAxis, 0);
        }
        //Bullet Fire:
        if (Input.GetKey ("space") && arsenalSize > 0) {
            if (!NewBehaviourScript.isPaused) {
                if (Time.time - lastStep > timeBetweenSteps) {
                    lastStep = Time.time;
                    recoil += 0.1f;
                    fireBullet = GetComponent<AudioSource> ();
                    fireBullet.Play (0);
                    //Debug.Log("Firing Bullet!");
                    GameObject bullet01 = (GameObject) Instantiate (playerBullet01);
                    bullet01.transform.position = bulletPosition01.transform.position;
                    Vector3 rotationCoordinates = new Vector3 (0, 0, 52);
                    bullet01.transform.eulerAngles = bulletPosition01.transform.eulerAngles + rotationCoordinates;
                    Vector3 permanentPosition = this.transform.position;
                    POSITION = permanentPosition;
                    ROTATION = this.transform.rotation;
                    recoiling ();
                    Invoke ("reset", 0.05f);
                    arsenalSize--;
                    if (this.gameObject.name == "Hellfire") {
                        GameObject bullet02 = (GameObject) Instantiate (playerBullet01);
                        bullet02.transform.position = bulletPosition02.transform.position;
                        Vector3 rotationCoordinates02 = new Vector3 (0, 0, 52);
                        bullet02.transform.eulerAngles = bulletPosition02.transform.eulerAngles + rotationCoordinates02;
                        arsenalSize--;
                    }
                }
            }
        } else if (Input.GetKeyDown ("r")) {
            if (NewBehaviourScript.finalScore - maxArsenalSize >= 0) {
                if (this.gameObject.name == "Railgun")
                    arsenalSize = maxArsenalSize / 2;
                else
                    arsenalSize = maxArsenalSize;
                ReloadSound.Play (0);
                NewBehaviourScript.finalScore -= maxArsenalSize;
            }
        }

    }

    void OnGUI () {
        GUI.skin = theSkin;
        if (arsenalSize > 10) {
            GUI.Label (new Rect (Screen.width * 1 / 50, Screen.height * 9 / 10, 100, 80), "" + arsenalSize, TextStyle);
        } else if (arsenalSize > 0) {
            GUI.Label (new Rect (Screen.width * 1 / 50, Screen.height * 9 / 10, 100, 80), "" + arsenalSize, TextStyleReload);
        } else if (arsenalSize == 0) {
            GUI.Label (new Rect (Screen.width / 2 - 105, Screen.height * 2 / 3, 200, 80), "RELOAD", TextStyleReload);
        }
    }

    void reset () {
        this.transform.position = POSITION;
        this.transform.rotation = ROTATION;
    }
    void recoiling () {
        Vector3 permanentPosition = this.transform.position;
        Vector3 maxRecoil = new Vector3 (0, -3f, 0);
        this.transform.position = Vector3.Slerp (permanentPosition, maxRecoil, Time.deltaTime * recoilSpeed);
    }

}