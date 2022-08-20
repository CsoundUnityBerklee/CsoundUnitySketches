using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AI : MonoBehaviour
{
    public float timer;

    public float newtarget;

    public float speed;

    public NavMeshAgent nav;

    public Vector3 Target;



    // Start is called before the first frame update
    void Start()
    {
        nav = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;


        if (timer >= newtarget)
        {
            newTarget();

            timer = 0;
        }
        
    }



    void newTarget()
    {
        float myX = gameObject.transform.position.x;

        float myZ = gameObject.transform.position.z;

        float xPos = myX + Random.Range(myX - 10, myX + 10);

        float zPos = myZ + Random.Range(myZ - 10, myZ + 10);



        Target = new Vector3(xPos, gameObject.transform.position.y, zPos);

        nav.SetDestination(Target);
    }
}
