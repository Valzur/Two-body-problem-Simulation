using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpaceObject : MonoBehaviour
{
    public float Mass;
    public Vector3 InitialVelocity;
    public Vector3 CurrentVelocity;
    // Start is called before the first frame update
    void Start()
    {
        CurrentVelocity = InitialVelocity;
        GameManager.Instance.Bodies.Add(this);
    }
    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
