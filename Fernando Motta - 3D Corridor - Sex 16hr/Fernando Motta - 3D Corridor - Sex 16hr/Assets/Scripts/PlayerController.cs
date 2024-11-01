using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 8f;
    public float gravity = -9.81f;
    public bool chaveUm = false;

    Vector3 velocity;

    public Transform GroundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public int hp = 3;
    public float stunDuration = 0.2f;
    public float deathDuration = 5f;
    public GameObject hitParticle;

    public float rotationSpeed;

    private bool locked = false;
    private bool dead = false;

    bool IsGrounded;

    void Update()
    {
            IsGrounded = Physics.CheckSphere(GroundCheck.position, groundDistance, groundMask);

            if (IsGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");


        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        transform.eulerAngles  = new Vector3(0, transform.eulerAngles.y + rotationSpeed + Input.GetAxis("Mouse X"), 0);
    }

    public void GetHit(int damage, Vector3 particlePos)
    {
        if (!dead)
        {
            locked = true;
            hp-= damage;
            Instantiate(hitParticle, particlePos, Quaternion.identity);
            CancelInvoke("DealDamage");
            CancelInvoke ("Unlock");

            if (hp <= 0)
            {
                //rig.constraints = RigidbodyConstraints.None;
                //rig.AddTorque(new Vector3(Random.Range(-10,10), Random.Range(-10,10), Random.Range(-10,10)));
                dead = true;
                Invoke("Reload", deathDuration);
            }
            else
            {
                Invoke("Unlock", stunDuration);
            }
        }
    }
    void Unlock()
    {
        locked = false;
    }
    void Reload()
    {
        SceneManager.LoadScene(0);
    }
}
