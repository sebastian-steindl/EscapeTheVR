using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//For menu setup according to https://www.youtube.com/watch?v=__iTtJHZg6k and https://www.youtube.com/watch?v=0otP3ww-auE
public class Page : MonoBehaviour
{

    private Canvas canvas;
    // Start is called before the first frame update
    void Awake()
    {
        canvas = GetComponent<Canvas>();
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hide() {
        canvas.enabled= false;
    }

    public void Show() {
        canvas.enabled= true;
    }
}
