using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParatrooperScript : MonoBehaviour {

    private Rigidbody2D rigidBody;
    private Animator myAnimator;
    public AudioSource sound;
    bool killSound;
    bool landed;
    bool parachuteOpen;
    public GameObject parachutePrefab;
    GameObject instance; // Instance of parachute

    // Use this for initialization
    void Start () {
        killSound = false;
        rigidBody = GetComponent<Rigidbody2D> ();
        myAnimator = gameObject.GetComponent<Animator> ();
        myAnimator.SetBool ("isRunning", false);
        rigidBody.velocity = new Vector2 (0f, rigidBody.velocity.y);
        parachuteOpen = false;
        rigidBody.gravityScale = 0.3f;
    }

    // Update is called once per frame
    void Update () {
        if (rigidBody.transform.position.y < 1.5 && !landed) {
            rigidBody.gravityScale = -0.9f;
            if (rigidBody.velocity.y <= 0.5) {
                rigidBody.gravityScale = 0f;
                if (instance != null)
                    instance.GetComponent<Rigidbody2D> ().gravityScale = 0f;
            }
            if (!parachuteOpen) {
                instance = (GameObject) Instantiate (parachutePrefab);
                instance.transform.rotation = Quaternion.identity;
                float length = instance.GetComponent<SpriteRenderer> ().bounds.size.y;
                instance.transform.position = new Vector3 (rigidBody.transform.position.x, rigidBody.transform.position.y + (length / 2), 0);
                instance.GetComponent<Rigidbody2D> ().velocity = rigidBody.velocity;
                instance.GetComponent<Rigidbody2D> ().gravityScale = rigidBody.gravityScale;
                parachuteOpen = true;
            }
        }
        if (((SpawningScript.parasOnLeft == 4) || (SpawningScript.parasOnRight == 4)) & landed) {
            myAnimator.SetBool ("isRunning", true);
            float randomVal = Random.Range (0.05f, 0.1f);
            if (rigidBody.transform.position.x > 0)
                rigidBody.velocity = new Vector2 (-1.5f, 0);
            else
                rigidBody.velocity = new Vector2 (1.5f, 0);
            if (Mathf.Abs (rigidBody.velocity.x - 0f) < 1e-4) {
                if (rigidBody.transform.position.x > 0)
                    rigidBody.velocity = new Vector2 (-1.5f, randomVal);
                else
                    rigidBody.velocity = new Vector2 (1.5f, randomVal);

            }
            if (SpawningScript.reachedTurret & !SpawningScript.destroyedTurret) {
                myAnimator.SetBool ("isRunning", false);
                myAnimator.SetBool ("isAttacking", true);
                rigidBody.velocity = Vector2.zero;
            }
            if (SpawningScript.reachedTurret & SpawningScript.destroyedTurret) {
                myAnimator.SetBool ("isRunning", false);
                myAnimator.SetBool ("isAttacking", false);
                myAnimator.SetBool ("isJumping", true);
                rigidBody.velocity = Vector2.zero;
            }
        }
    }

    private void OnCollisionEnter2D (Collision2D collision) {
        if ((collision.gameObject.name == "Ground") && !landed) {
            Destroy (instance);
            if ((SpawningScript.parasOnLeft < 4) & (SpawningScript.parasOnRight < 4)) {
                if (rigidBody.transform.position.x < 0)
                    SpawningScript.parasOnLeft++;
                else
                    SpawningScript.parasOnRight++;

                if ((SpawningScript.parasOnLeft == 4) || (SpawningScript.parasOnRight == 4))
                    SpawningScript.stopGame = true;
            }
            rigidBody.transform.rotation = Quaternion.identity;
            rigidBody.angularVelocity = 0;
            rigidBody.velocity = Vector2.zero;
            rigidBody.gravityScale = 1f;
            landed = true;
            parachuteOpen = false;
        }
        if (collision.gameObject.tag == "anyBullet") {
            Destroy (instance);
            //parachuteOpen = false;
            NewBehaviourScript.finalScore += 15;
            //Destroy(instance);
            rigidBody.velocity = Vector2.zero;
            Destroy (collision.gameObject);
            myAnimator.SetBool ("isHurt", true);
            if (!killSound) {
                sound = GetComponent<AudioSource> ();
                sound.Play (0);
                killSound = true;
            }
            StartCoroutine (CollisionFunction ());
        }
        if (collision.gameObject.tag == "playerWeapon" && !SpawningScript.reachedTurret) {
            SpawningScript.reachedTurret = true;
            Debug.Log ("reached turret true");
        }
    }

    IEnumerator CollisionFunction () {
        yield return new WaitForSeconds (0.4f);
        Destroy (gameObject);
    }

    void OnBecameInvisible () {
        Destroy (gameObject);
    }
}