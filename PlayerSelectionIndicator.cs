using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionIndicator : MonoBehaviour
{
    private Image image;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Awake()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // change the sprite based on the character that is selected
    public void setImage(int id)
    {
        if (id == 0)
            image.sprite = Resources.Load<Sprite>("Biker_idle");

        else if (id == 1)
            image.sprite = Resources.Load<Sprite>("Punk_idle");

        else if (id == 2)
            image.sprite = Resources.Load<Sprite>("Biker_idle 1");

        else if (id == 3)
            image.sprite = Resources.Load<Sprite>("Cyborg_idle");

        else if (id == 4)
            image.sprite = Resources.Load<Sprite>("Punk_idle 1");

        else if (id == 5)
            image.sprite = Resources.Load<Sprite>("Cyborg_idle 1");
    }
}
