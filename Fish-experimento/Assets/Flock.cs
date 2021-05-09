using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockManager myManager;
    //velocidade
    float speed;
    // verifica se o peixe virou
    bool turning = false;

    // Start is called before the first frame update
    void Start()
    {
        //movimentação dos peixes
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        //limita a distancia que eles se movimentam sem rotacionar
        Bounds b = new Bounds(myManager.transform.position, myManager.swinLimits * 2);

        RaycastHit hit = new RaycastHit();
        Vector3 direction = myManager.transform.position - transform.position;

        //se passarem do limite
        if (!b.Contains(transform.position))
        {
            turning = true;
            direction = myManager.transform.position - transform.position;
        }
        else if(Physics.Raycast(transform.position, this.transform.forward * 50, out hit))
        {
            // evita a colisão
            turning = true;
            direction = Vector3.Reflect(this.transform.forward, hit.normal);
        }
        else
        {
            turning = false;
        }
        //se estiver virando
        if (turning)
        {
            //faz a rotação ser fluida, suave
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
        }
        else
        {
            //se estiver menor que 10 usa o maxSpeed e o minSpeed
            if (Random.Range(0, 100) < 10)
                speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
            //se estiver menor que 20 chama o metodo
            if(Random.Range(0, 100) < 20)
                ApplyRules();
        }
        // move o peixe
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    
    void ApplyRules()
    {
        //array
        GameObject[] gos;
        gos = myManager.allFish;
        // calculo do ponto medio
        Vector3 vcentre = Vector3.zero;
        //evita colisao
        Vector3 vavoid = Vector3.zero;
        // velocidade 
        float gSpeed = 0.01f;
        // distancia
        float nDistance;
        //quantos peixes formam o cardume
        int groupSize = 0;

        foreach (GameObject go in gos) {
            if (go != this.gameObject)
            {
                nDistance = Vector3.Distance(go.transform.position, this.transform.position); 
                if (nDistance <= myManager.neighbourDistance)
                {
                    //transforma o centro como posição do objeto
                    vcentre += go.transform.position;
                    //aumenta o tamanho do grupo
                    groupSize++;
                    //se a distancia for menor que 1
                    if(nDistance < 1.0)
                    {
                        //faz o calculo do objeto da lista
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }
                    //chama o script
                    Flock anotherFlock = go.GetComponent<Flock>();
                    //aumenta a velocidade
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }
        //se o groupSize for maior que zero
        if(groupSize > 0)
        {
            //faz um calculo do centro do peixe
            vcentre = vcentre / groupSize + (myManager.goalPos - this.transform.position);
            //ajeita a velocidade do grupo
            speed = gSpeed / groupSize;

            //calculo da direção
            Vector3 direction = (vcentre + vavoid) - transform.position;
            if(direction != Vector3.zero)
            {
                //rotaciona
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
            }
        }
    }
}