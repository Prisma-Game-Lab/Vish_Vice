using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public float timer;
    private float ind;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        ind = 1;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        //Usando o ind como comparação para rotacionar a luz direcional
        //Pra quesito de teste, a cada 1 segundo, rotaciona -1 graus no eixo x
        //dando a impressão de anoitecer e amanhecer em um total de 2 minutos.
        if (timer < 61) {
            if (timer > ind) {
                transform.Rotate(-1, 0, 0);
                ind++;
            }
        } else if (timer < 121) {
            if (timer > ind) {
                transform.Rotate(1, 0, 0);
                ind++;
            }
        } else
        {
            timer = 0f;
            ind = 1;
        }
    }
}
