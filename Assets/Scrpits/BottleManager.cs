using UnityEngine;

public class BottleManager : MonoBehaviour
{
    public Material matE;
    public Material matR;
    public Material matG;
    public Material matB;
    public string liquid;
    public string prevLiquid;
    private Renderer barrelRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (liquid != prevLiquid)
        {
            prevLiquid = liquid;

            foreach (Transform child in transform.GetComponentsInChildren<Transform>(true))
            {
                if (child.CompareTag("barrel"))
                {
                    barrelRenderer = child.GetComponent<Renderer>();
                    break;
                }
            }

            switch (liquid)
            {
            case "red":
                barrelRenderer.material = matR;
                break;

            case "green":
                barrelRenderer.material = matG;
                break;

            case "blue":
                barrelRenderer.material = matB;
                break;

            default:
                barrelRenderer.material = matE;
                break;
            }
        
        }
    }
}
