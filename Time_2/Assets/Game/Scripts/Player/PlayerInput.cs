using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float speed;
    public VariableJoystick variableJoystick;
    public CharacterController controller;

    private void Update()
    {
        Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
        controller.Move(direction * speed * Time.deltaTime);
    }

}
