using UnityEngine;

public class WrapPlayableArea : MonoBehaviour
{
    [SerializeField]
    private float margin = 0.01f;

    [SerializeField]
    protected bool destroyOnOutOfScreen = false;
    protected Vector3 viewportPos;
    protected Vector3 topLeft;
    protected Vector3 bottomRight;

    public virtual void Start()
    {
        topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight, -Camera.main.transform.position.z));
        bottomRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0, -Camera.main.transform.position.z));

        topLeft += Vector3.one * margin;
        bottomRight += Vector3.one * margin;
    }

    private void FixedUpdate()
    {
        if (transform.position.y > topLeft.y)
        {
            if (!destroyOnOutOfScreen)
                transform.position = new Vector2(transform.position.x, bottomRight.y - margin);
            else
                OnOutOfScreen();
        }
        else if (transform.position.y < bottomRight.y)
        {
            if (!destroyOnOutOfScreen)
                transform.position = new Vector2(transform.position.x, topLeft.y + margin);
            else
                OnOutOfScreen();
        }

        if (transform.position.x > bottomRight.x)
        {
            if (!destroyOnOutOfScreen)
                transform.position = new Vector2(topLeft.x - margin, transform.position.y);
            else
                OnOutOfScreen();
        }
        else if (transform.position.x < topLeft.x)
        {
            if (!destroyOnOutOfScreen)
                transform.position = new Vector2(bottomRight.x + margin, transform.position.y);
            else
                OnOutOfScreen();
        }
    }

    public virtual void OnOutOfScreen()
    {

    }

}
