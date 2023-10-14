
using UnityEngine;

public class BotController : MonoBehaviour
{
    public int Index;
    public int botIndex = 0;
    public Vector2 direction = Vector2.zero;
    private float inputX = 0;
    private float inputY = 0;

    private Compass compass_ { get; set; } = new Compass();
    public Compass compass
    {
        get { return compass_; }
        set
        {
            compass_ = value;
            if (compass_ == Compass.North)
                direction = Vector2.up;
            else if (compass_ == Compass.South)
                direction = Vector2.down;
            else if (compass_ == Compass.West)
                direction = Vector2.left;
            else if (compass_ == Compass.East)
                direction = Vector2.right;
        }
    }

    private void Start()
    {
        switch (new System.Random().Next(0, 4))
        {
            case 0:
                compass = Compass.North;
                break;
            case 1:
                compass = Compass.South;
                break;
            case 2:
                compass = Compass.West;
                break;
            case 3:
                compass = Compass.East;
                break;
        }
    }

    private void Update()
    {
        if (Manager.Game.General.IsPaused)
            return;

        inputX = direction.x;
        inputY = direction.y;

        float gposX = gameObject.transform.position.x;
        float gposY = gameObject.transform.position.y;
        float gposZ = gameObject.transform.position.z;

        float movementSpeed = Manager.Game.General.BotSpeed;
        if (inputX > 0)
        { gameObject.transform.position = new Vector3(gposX + (Time.deltaTime * movementSpeed), gposY, gposZ); }
        else if (inputX < 0)
        { gameObject.transform.position = new Vector3(gposX - (Time.deltaTime * movementSpeed), gposY, gposZ); }
        else if (inputY > 0)
        { gameObject.transform.position = new Vector3(gposX, gposY, gposZ + (Time.deltaTime * movementSpeed)); }
        else if (inputY < 0)
        { gameObject.transform.position = new Vector3(gposX, gposY, gposZ - (Time.deltaTime * movementSpeed)); }
    }


    private void OnCollisionStay(Collision other)
    {
        if (other.transform.CompareTag("Player") || other.transform.CompareTag("Bot") || other.transform.CompareTag("Wall"))
            RandomReverseCompass();
    }

    private void ReverseCompass()
    {
        switch (compass)
        {
            case Compass.North:
                compass = Compass.South;
                break;
            case Compass.West:
                compass = Compass.East;
                break;
            case Compass.East:
                compass = Compass.West;
                break;
            case Compass.South:
                compass = Compass.North;
                break;
        }
    }

    private void RandomReverseCompass()
    {
        System.Random rnd = new System.Random();
        Compass myNewWay;
        do
            myNewWay = (Compass)rnd.Next(0, 4);
        while (myNewWay == compass);
        compass = myNewWay;
    }
}
