using UnityEngine;

/// <summary>
///     monobehavior to hide the cursor and position this gameobject where the mouse is
/// </summary>
public class Cheese : MonoBehaviour
{
    public void Start()
    {
        Cursor.visible = false;
    }

    public void Update()
    {
        this.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition).xy();
    }
}