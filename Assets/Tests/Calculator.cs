using UnityEngine;

public class Calculator : MonoBehaviour
{
    private const string Message = "Нельзя делить на ноль!";
    public string ErrorMessageProp => Message;
    public string ErrorMessage() => Message;

    public int AddNumbers(int numA, int numB) =>
         numA + numB;

    public int DivideNumbers(int numA, int numB)
    {
        if (numB == 0)
        {
            Debug.LogError(Message);
            return 0;
        }

        return numA / numB;
    }

    public float GetPiValue() =>
         Mathf.PI;
}