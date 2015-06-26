using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(TextMesh))]
public class SmartTextMesh : MonoBehaviour
{
    TextMesh TheMesh;
    public string UnwrappedText;
    public float MaxWidth;
    public bool NeedsLayout = true;
    public bool ConvertNewLines = false;

    //! \brief Awake is called first when the script is enabled.
    //! \return void
    void Awake() 
    {
        TheMesh = GetComponent<TextMesh>();
    }

    //! \brief Start is called on the frame when a script is enabled.
    //! Initialize the variables.
    //! \return void
    void Start()
    {
        if (ConvertNewLines)
            UnwrappedText = UnwrappedText.Replace("\\n", System.Environment.NewLine);
    }

    //! \brief BreakPartIfNeeded cuts a string in multiple lines.
    //! This function cuts a string in multiple lines to fit an answer
    //! in a cloud.
    //! \return void
    string BreakPartIfNeeded(string part)
    {
        string saveText = TheMesh.text;
        TheMesh.text = part;

        if (TheMesh.GetComponent<Renderer>().bounds.extents.x > MaxWidth)
        {
            string remaining = part;
            part = "";
            while (true)
            {
                int len;
                for (len = 2; len <= remaining.Length; len++)
                {
                    TheMesh.text = remaining.Substring(0, len);
                    if (TheMesh.GetComponent<Renderer>().bounds.extents.x > MaxWidth)
                    {
                        len--;
                        break;
                    }
                }
                if (len >= remaining.Length)
                {
                    part += remaining;
                    break;
                }
                part += remaining.Substring(0, len) + System.Environment.NewLine;
                remaining = remaining.Substring(len);
            }

            part = part.TrimEnd();
        }

        TheMesh.text = saveText;

        return part;
    }

    //! \brief UpdateTextLayOut updates the text.
    //! Format the string to fit in a cloud.
    //! \return void
    public void UpdateTextLayOut()
    {

        if (!NeedsLayout)
        {
            return;
        }

        if (MaxWidth == 0)
        {
            TheMesh.text = UnwrappedText;
            return;
        }
        string builder = "";
        string text = TheMesh.text;

        TheMesh.text = "";
        //.text = "";
        string[] parts = text.Split(' ');
        for (int i = 0; i < parts.Length; i++)
        {
            string part = BreakPartIfNeeded(parts[i]);
            TheMesh.text += part + " ";
            if (TheMesh.GetComponent<Renderer>().bounds.extents.x > MaxWidth)
            {
                TheMesh.text = builder.TrimEnd() + System.Environment.NewLine + part + " ";
            }
            builder = TheMesh.text;
        }
    }
}
