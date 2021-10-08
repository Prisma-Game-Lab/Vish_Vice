using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float speed;
    public Animator anim;
    public VariableJoystick variableJoystick;
    private CharacterController controller;
    private bool flipped = false;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        controller.enabled = false;
        if (Persistent.current.playerPosition != Vector3.zero)
            transform.position = Persistent.current.playerPosition;

        controller.enabled = true;
    }

    private void Update()
    {
        Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;

        if (variableJoystick.Horizontal < 0 && !flipped)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            flipped = true;
        }
        else if (variableJoystick.Horizontal > 0 && flipped)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            flipped = false;
        }
        anim.SetFloat("Horizontal", variableJoystick.Horizontal);
        anim.SetFloat("Vertical", variableJoystick.Vertical);
        anim.SetFloat("Moving", direction.magnitude);
        controller.SimpleMove(direction.normalized * speed);
    }

    public void playSteps()
    {
        int index = Random.Range(0, 2);
        if (index == 0)
            AudioManager.instance.Play("Step1");
        else
        {
            AudioManager.instance.Play("Step2");
        }
    }

    private void OnDestroy()
    {
        Persistent.current.playerPosition = transform.position;
    }
}
