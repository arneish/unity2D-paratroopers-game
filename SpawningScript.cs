using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningScript : MonoBehaviour {

    //public GameObject prefab;
    //GameObject clone;

    public GameObject enemies;
    public float spawnWait;
    public float spawnMostWait;
    public float spawnLeastWait;
    public int startWait;
    bool dumpVar;

    public static int parasOnLeft;
    public static int parasOnRight;
    public static bool reachedTurret;
    public static bool stopGame;
    public static bool destroyedTurret;
    public static bool reinitialised;
    public GameObject explosion;
    public GameObject temp_exp; // Temporary object for instantiating explosion
    public GameObject helicopter; // Temporary object for instantiating helicopter

    int spawnColumn;
    float columny;
    float helicopterRandomVar;

    void Start () {
        Debug.Log ("Initialising Spawning Script");
        startWait = 2;
        spawnLeastWait = 0.8f;
        spawnMostWait = 2f;
        parasOnLeft = 0;
        parasOnRight = 0;
        reachedTurret = false;
        dumpVar = false;
        destroyedTurret = false;
        stopGame = false;
        reinitialised = false;
        Debug.Log ("Right before Start Coroutine");
        StartCoroutine (Spawn ());
        Debug.Log ("Right after Start Coroutine");
    }

    void Update () {
        if (reinitialised) {
            StartCoroutine (Spawn ());
            reinitialised = false;
        }
        spawnWait = Random.Range (spawnLeastWait, spawnMostWait);
        if (reachedTurret & !dumpVar) {
            dumpVar = true;
            Invoke ("destroyTurret", 5f);
        }
    }

    IEnumerator Spawn () {
        yield return new WaitForSeconds (startWait);
        while ((parasOnLeft < 4) & (parasOnRight < 4)) {
            Debug.Log ("Inside recursively called Spawn function");
            spawnColumn = Random.Range (0, 3);
            if (spawnColumn == 0)
                columny = 4.45f;
            else if (spawnColumn == 1)
                columny = 3.45f;
            else
                columny = 2.45f;
            helicopter = (GameObject) Instantiate (enemies);
            helicopter.transform.rotation = Quaternion.identity;
            if (spawnColumn == 1) {
                helicopter.transform.position = new Vector3 (9.6f - 0.5f, columny, 0);
                helicopter.SendMessage ("assignDirection", false);
            } else {
                helicopter.transform.position = new Vector3 (-9.6f - 0.5f, columny, 0);
                helicopter.SendMessage ("assignDirection", true);
            }
            yield return new WaitForSeconds (spawnWait);
        }
    }

    public void destroyTurret () {
        StartCoroutine (DestroyTurretFunction ());
    }

    IEnumerator DestroyTurretFunction () {
        temp_exp = (GameObject) Instantiate (explosion, new Vector3 (0f, -3.93f, 0), Quaternion.identity);
        yield return new WaitForSeconds (1.5f);
        destroyedTurret = true;
        GameObject turret = GameObject.FindGameObjectWithTag ("playerWeapon");
        Destroy (turret);
        GameObject theend = GameObject.FindGameObjectWithTag ("Finish");
        yield return new WaitForSeconds (3.17f);
        Destroy (theend);
    }
}