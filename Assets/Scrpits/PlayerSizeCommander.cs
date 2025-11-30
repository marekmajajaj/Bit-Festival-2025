using UnityEngine;

public class PlayerSizeCommander : MonoBehaviour
{
    public int playerAge = 1; // 0 = small, 1 = medium, 2 = large
    public int prevPlayerAge;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerAge != prevPlayerAge)
        {
            prevPlayerAge = playerAge;

            switch (playerAge)
            {
                case 0:
                    transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    break;

                case 1:
                    transform.localScale = new Vector3(1f, 1f, 1f);
                    break;

                case 2:
                    transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                    break;

                default:
                    transform.localScale = new Vector3(1f, 1f, 1f);
                    break;
            }
        }
        
    }
}
