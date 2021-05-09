using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{

    public GameObject fishPrefab;
    //quantidade de peixe
    public int numFish = 20;
    public GameObject[] allFish;
    public Vector3 swinLimits = new Vector3(5, 5, 5);
    public Vector3 goalPos;

    //alterar a velocidade do cardume
    [Header("Configurações do Cardume")]
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    //distancia e rotação
    [Range(1.0f, 10.0f)]
    public float neighbourDistance;
    [Range(0.0f, 5.0f)]
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //coloca os peixes na cena em posição aleatoria
        allFish = new GameObject[numFish];
        for (int i = 0; i < numFish; i++)
        {
            //posiciona aleatoriamente
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x),
                                                                Random.Range(-swinLimits.y, swinLimits.y),
                                                                Random.Range(-swinLimits.z, swinLimits.z));
            //gera peixe na posição
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
            //chama o script Flock
            allFish[i].GetComponent<Flock>().myManager = this;
        }

        //Alvo
        goalPos = this.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        goalPos = this.transform.position;
        if (Random.Range(0, 100) < 10)
        {
            //define o destino em relação a posição do peixe 
            goalPos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x), Random.Range(-swinLimits.y, swinLimits.y), Random.Range(-swinLimits.z, swinLimits.z));
        }
        
    }
}