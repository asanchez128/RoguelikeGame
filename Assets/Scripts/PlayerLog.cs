 using UnityEngine;
 using System.Collections;
 using System.Collections.Generic;

//from http://answers.unity3d.com/questions/654880/how-do-i-create-a-text-box-to-display-scrollable-t.html

public class PlayerLog : MonoBehaviour
{

    public int maxLines = 8;
    private Queue<string> queue = new Queue<string>();
    private string Mytext = "";

    public void NewMessage(string message)
    {
        if (queue.Count >= maxLines)
            queue.Dequeue();

        queue.Enqueue(message);

        Mytext = "";
        foreach (string st in queue)
            Mytext = Mytext + st + "\n";
    }


    void OnGUI()
    {

        GUI.Label(new Rect(0, // x, left offset
        (0), // y, bottom offset
        300f, // width
        150f), Mytext, GUI.skin.textArea); // height, text, Skin features}
    }
}