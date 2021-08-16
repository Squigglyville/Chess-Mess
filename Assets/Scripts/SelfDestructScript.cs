using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructScript : MonoBehaviour
{
    public float timeUntilDestroyed;
    float timePassed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if(timePassed >= timeUntilDestroyed)
        {
            Destroy(gameObject);
        }
    }
}
