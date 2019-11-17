using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class calculator : MonoBehaviour{
    private List<float> numbers = new List<float>();
    private List<char> operators = new List<char>();
    private string compute_string;

    void OnGUI()
    {
       // TextAlignment text = Right;
        //输出计算结果
        GUI.TextField(new Rect(210, 120, 230, 40), compute_string);
        //数字键
        if (GUI.Button(new Rect(180, 330, 110, 40), "0")) compute_string += "0";
        if (GUI.Button(new Rect(180, 280, 50, 40), "1")) compute_string += "1";
        if (GUI.Button(new Rect(240, 280, 50, 40), "2")) compute_string += "2";
        if (GUI.Button(new Rect(300, 280, 50, 40), "3")) compute_string += "3";
        if (GUI.Button(new Rect(180, 230, 50, 40), "4")) compute_string += "4";
        if (GUI.Button(new Rect(240, 230, 50, 40), "5")) compute_string += "5";
        if (GUI.Button(new Rect(300, 230, 50, 40), "6")) compute_string += "6";
        if (GUI.Button(new Rect(180, 180, 50, 40), "7")) compute_string += "7";
        if (GUI.Button(new Rect(240, 180, 50, 40), "8")) compute_string += "8";
        if (GUI.Button(new Rect(300, 180, 50, 40), "9")) compute_string += "9";

        //运算符键
        if (GUI.Button(new Rect(360, 280, 50, 90), "+")) compute_string += "+";
        if (GUI.Button(new Rect(420, 280, 50, 40), "-")) compute_string += "-";
        if (GUI.Button(new Rect(360, 230, 50, 40), "x")) compute_string += "x";
        if (GUI.Button(new Rect(420, 230, 50, 40), "/")) compute_string += "/";

        //功能键
        if (GUI.Button(new Rect(300, 330, 50, 40), ".")) compute_string += ".";
        if (GUI.Button(new Rect(420, 180, 50, 40), "CE")) compute_string = "";
        if (GUI.Button(new Rect(360, 180, 50, 40), "<<")) compute_string = compute_string.Substring(0, compute_string.Length - 1);
        if (GUI.Button(new Rect(420, 330, 50, 40), "="))
        {
            compute_string = compute(compute_string).ToString();
            numbers.Clear();
            operators.Clear();
        }
    }

    float compute(string str)
    {
        if (str == "") return 0;
        pre_treat(str);
        for (int i = 0; i < operators.Count; i++) 
        {
            if(operators[i] == 'x' || operators[i] == '/')
            {
                float tmp;
                float left = numbers[i];
                float right = numbers[i + 1];
                if (operators[i] == 'x') tmp = left * right;
                else tmp = left / right;
                numbers.RemoveAt(i);
                numbers[i] = tmp;
                operators.RemoveAt(i);
                i--;
            }
            
        }
        for (int i = 0; i < operators.Count; i++)
        {
            float tmp;
            float left = numbers[i];
            float right = numbers[i + 1];
            if (operators[i] == '+') tmp = left + right;
            else tmp = left - right;
            numbers.RemoveAt(i);
            numbers[i] = tmp;
            operators.RemoveAt(i);
            i--;
        }
        return numbers[0];
    }

    void pre_treat(string str)
    {
        string tmp = "";
        for (int i = 0; i < str.Length; i++)
        {
            if (i == 0 && str[i] == '-'){
                str = "0" + str;//当第一个字符为‘-’号时，在前面加‘0’
            }
            if (i >= str.Length - 1)
            {
                tmp += str[i];
                numbers.Add(float.Parse(tmp));
            }
            else if (is_number(str[i]))
            {
                tmp += str[i];
            }
            else
            {
                numbers.Add(float.Parse(tmp));
                tmp = "";
                if (is_operator(str[i]))
                {
                    operators.Add(str[i]);
                }
            }
        }
    }

    bool is_number(char c)
    {
        return (c == '0' || c == '1' || c == '2' || c == '3' ||
            c == '4' || c == '5' || c == '6' || c == '7' ||
            c == '8' || c == '9' || c == '.');
    }

    bool is_operator(char c)
    {
        return (c == '+' || c == '-' || c == 'x' || c == '/');
    }

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
