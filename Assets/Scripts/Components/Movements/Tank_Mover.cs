using UnityEngine;

public class Tank_Mover : Mover
{

    private Rigidbody body;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    //Overriding the Move Abtract method for the Tank Movement
    public override void Move(Vector3 direction, float speed)
    {
        Vector3 moveVector = direction.normalized * speed * Time.deltaTime; //Create a vector for the position needing to be added to the Tank

        body.MovePosition(body.position + moveVector); //Update the position of the tank with the gotten vector
    }
    //Overridding abstract Rotate Method
    public override void Rotate(float rotateSpeed)
    {
        body.transform.Rotate(0, rotateSpeed * Time.deltaTime, 0); //updates the rotation transform of the tank based on the rotation speed
    }
}
