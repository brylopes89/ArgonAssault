using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Test : MonoBehaviour
{   
    
    // Start is called before the first frame update
    void Start()
    {
        int num1, num2, num3, result;

        Console.WriteLine("Input the first number to multiply: ");
        num1 = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Input the second number to multiply: ");
        num2 = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Input the third number to multiply: ");
        num3 = Convert.ToInt32(Console.ReadLine());

        result = num1 * num2 * num3;

        Console.WriteLine("{0}x{1}x{2} = {3}", num1, num2, num3, result);

        Console.ReadKey();
    } 
}
