using UnityEngine;

[ExecuteInEditMode]
public class MessageController : MonoBehaviour
{
    public bool DestroyAfterClose = false;

    public void Show()
    {
        this.gameObject.SetActive(true);

        this.transform.SetAsLastSibling();
    }

    public void Close()
    {
        this.gameObject.SetActive(false);

        if (DestroyAfterClose) Destroy(this.gameObject, 0.5f);
    }

    public void tolink(string url)
    {
        // on click, OpenUrl
        Application.OpenURL(url);
    }
}
