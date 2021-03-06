using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float speed;
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public bool stuck = false;
    public DayCycle dayCycle;
    public Animator anim;
    public VariableJoystick variableJoystick;
    private CharacterController controller;
    private bool flipped = false;

    private void Start()
    {
        Persistent persistentData = Persistent.current;
        controller = GetComponent<CharacterController>();

        controller.enabled = false;

        if (persistentData.fadeOn && persistentData.currentTime > 6)
        {
            StartCoroutine(dayCycle.FadeImage(false));
            transform.position = new Vector3(persistentData.playerStartX, persistentData.playerStartY, persistentData.playerStartZ);
            Debug.Log("111");
            persistentData.fadeOn = false;
        }
        else if (persistentData.fadeOn && persistentData.currentTime < 6)
        {
            Debug.Log("poss");
            StartCoroutine(dayCycle.FadeImage(false));
            transform.position = new Vector3(0.77f, 0.663f, 11.11f);
        }
        else if (persistentData.playerPosition != Vector3.zero)
        {
            transform.position = persistentData.playerPosition;
            print("olaa");
        }

        controller.enabled = true;
    }

    private void Update()
    {
        direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;

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
        string soundType1, soundType2;
        RaycastHit hit;
        Physics.Raycast(transform.position, -Vector3.up, out hit);

        if (hit.transform.CompareTag("Concrete"))
        {
            soundType1 = "ConcreteStep1";
            soundType2 = "ConcreteStep1";
        }
        else
        {
            soundType1 = "GrassStep1";
            soundType2 = "GrassStep2";
        }

        int index = Random.Range(0, 2);
        if (index == 0)
            AudioManager.instance.Play(soundType1);
        else
        {
            AudioManager.instance.Play(soundType2);
        }
    }

    private void OnDestroy()
    {
        Persistent.current.playerPosition = transform.position;
    }
}
