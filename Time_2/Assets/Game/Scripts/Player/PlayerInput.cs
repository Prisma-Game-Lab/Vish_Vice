using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float speed;
    public Animator anim;
    public VariableJoystick variableJoystick;
    private CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;

        anim.SetFloat("Horizontal", variableJoystick.Horizontal);
        anim.SetFloat("Vertical", variableJoystick.Vertical);
        anim.SetFloat("Moving", direction.magnitude);
        controller.Move(direction * speed * Time.deltaTime);
    }

}
