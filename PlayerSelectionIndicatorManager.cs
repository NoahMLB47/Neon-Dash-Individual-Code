using UnityEngine;

public class PlayerSelectionIndicatorManager : MonoBehaviour
{
    [SerializeField] GameObject[] indicators = new GameObject[4];
    GameObject indicator;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // spawns selection indicator based on player number and character sselected
    public void spawnIndicator(int imageId, int playerId)
    {
        indicator = indicators[playerId];
        if (!indicator.activeSelf)
            indicator.SetActive(true);
        indicator.GetComponent<PlayerSelectionIndicator>().setImage(imageId);
    }

    // deletes the indicator of given player number
    public void deleteIndicator(int playerId)
    {
        indicator = indicators[playerId];
        if (indicator.activeSelf)
            indicator.SetActive(false);
    }
}
