using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bot : MonoBehaviour
{
    public float speed;
    public float rotation;
    public LayerMask raycastMask;

    private float[] input = new float[5]; // Represents rays used to examine environment
    public NeuralNetwork network;

    public int position; // fitness score   
    public bool collided;

    Rigidbody _playerRb;

    void Start(){
        _playerRb = transform.Find("Sphere").GetComponent<Rigidbody>();
    }

    void FixedUpdate(){
        if (!collided)//stops the agent from communicating with the neural network if it has collided with an obstacle
        {
            for (int i = 0; i < 5; i++)
            {                                              
                Vector3 newVector = Quaternion.AngleAxis(i * 45 - 90, new Vector3(0, 1, 0)) * transform.right;
                RaycastHit hit;
                Ray Ray = new Ray(transform.position, newVector);

                if (Physics.Raycast(Ray, out hit, 10, raycastMask)) // Cast ray in 45 degree angles from ball
                {
                    if(i == 0){
                        Debug.DrawLine(transform.position, hit.point, Color.green); // make left ray of agent green for orientation
                    }
                    else{
                        Debug.DrawLine(transform.position, hit.point, Color.red);
                    }
                    input[i] = (10 - hit.distance) / 10;
                }
                else
                {
                    input[i] = 0;
                }
            }

            float[] output = network.FeedForward(input);
        
            transform.Rotate(0, output[0] * rotation, 0, Space.World);

            _playerRb.AddForce(this.transform.right * output[1] * speed);
        }
    }

    public void hasCollided(){
        collided = true;
    }

    public void incrementPosition(){
        position++;
    }

    public int getPosition(){
        return position;
    }

    public void UpdateFitness()
    {
        network.fitness = position;//updates fitness of network for sorting
    }
}
