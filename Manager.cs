using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{

    public float timeframe;
    public int populationSize;//creates population size
    public GameObject prefab;//holds bot prefab

    public int[] layers = new int[3] { 5, 3, 2 };//initializing network to the right size

    [Range(0.0001f, 1f)] public float MutationChance = 0.01f;

    [Range(0f, 1f)] public float MutationStrength = 0.5f;

    [Range(0.1f, 10f)] public float Gamespeed = 1f;

    //public List<Bot> Bots;
    public List<NeuralNetwork> networks;
    private List<Bot> agents;

    void Start()// Start is called before the first frame update
    {
        if (populationSize % 2 != 0)
            populationSize = 50;// if population size is not even, sets it to fifty

        InitNetworks();
        InvokeRepeating("CreateBots", 0.1f, timeframe);// repeating function
    }

    public void InitNetworks()
    {
        networks = new List<NeuralNetwork>(); // creates neural networks for each agent in iteration
        for (int i = 0; i < populationSize; i++)
        {
            NeuralNetwork net = new NeuralNetwork(layers);
            net.Load("Assets/Pre-trained.txt"); // seems to not work, intended to load weights from file
            networks.Add(net);
        }
    }

    public void CreateBots()
    {
        Time.timeScale = Gamespeed;//sets gamespeed, which will increase to speed up training

        if (agents != null)
        {
            // print("Bots are not empty");
            for (int i = 0; i < agents.Count; i++)
            {
                GameObject.Destroy(agents[i].gameObject);//if there are Prefabs in the scene this will get rid of them
            }

            SortNetworks();//this sorts networks and mutates them
        }

        agents = new List<Bot>();
        print(agents.Count);
        for (int i = 0; i < populationSize; i++)
        {
            GameObject c = Instantiate(prefab, new Vector3(0, 1.6f, -16), new Quaternion(0, 0, 1, 0));
            Bot agent = c.GetComponent<Bot>(); // Creates an agent with a neural network
            agent.network = networks[i];
            agents.Add(agent);
        }
        
    }

    public void SortNetworks()
    {
        for (int i = 0; i < populationSize; i++)
        {
            agents[i].UpdateFitness();//sets agents fitness score to be the position
        }
        networks.Sort();
        networks[populationSize - 1].Save("Assets/Save.txt");//not working, intended to save weights to file
        for (int i = 0; i < populationSize / 2; i++)
        {
            networks[i] = networks[i + populationSize / 2].copy(new NeuralNetwork(layers));
            networks[i].Mutate((int)(1/MutationChance), MutationStrength);
        }
    }
}
