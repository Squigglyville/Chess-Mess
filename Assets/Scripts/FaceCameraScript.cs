using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCameraScript : MonoBehaviour
{
    public Transform textMeshTransform;

    private void Start()
    {
        textMeshTransform = GetComponent<Transform>();
    }
    void Update()
    {
        textMeshTransform.rotation = Quaternion.LookRotation(textMeshTransform.position - Camera.main.transform.position);
    }
}
