using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : MonoBehaviour
{
    public abstract void Move(Vector3 direction, float speed); //Abstract method used to move with a velocity

    public abstract void Rotate(float rotateSpeed); //Abtract method used to rotate the gameObject
}
