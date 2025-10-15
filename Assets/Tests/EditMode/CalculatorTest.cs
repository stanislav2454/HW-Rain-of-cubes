using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;

public class CalculatorTest
{
    /// <summary>
    /// Тест ведет себя как обычный метод 
    /// </summary>
  /*  [Test]
    public void NewTestScriptSimplePasses()
    {//      Имя_сценарий/условие_ОжидаемоеПоведение
        // Используйте класс Assert для проверки условий
        // 1. Подготовить. Setup & Init tested obj ()
        // 2. Выполнить. Run tested method ()
        // 3. Убедиться. validate end result (проверка конечного результата)
    }*/

    [Test]
    public void AddNumbers_WhenGivenTwoInts_ReturnSum()
    {
        Calculator calculator = new Calculator();

        int actual = calculator.AddNumbers(10, 5);
        //                  "adding two numbers didn't produce the expected result"
        Assert.AreEqual(15, actual, "Cложение двух чисел не дало ожидаемого результата");
    }

    [TestCase(10, 0)]
    [TestCase(-20, 10)]
    [TestCase(5000, 10000)]
    public void AddNumbers_WhenGivenTwoInts_ReturnSum(int numA, int numB)
    {
        Calculator calculator = new Calculator();

        int actual = calculator.AddNumbers(numA, numB);
        //                  "adding two numbers didn't produce the expected result"
        Assert.AreEqual(numA + numB, actual, "Cложение двух чисел не дало ожидаемого результата");
    }

    [Test]
    public void GetPiValue_WhenCalled_ReturnPiValue()
    {
        Calculator calculator = new Calculator();

        float actual = calculator.GetPiValue();
        //                  "adding two numbers didn't produce the expected result"
        FloatEqualityComparer floatEqualityComparer = new FloatEqualityComparer(.01f);
        //Assert.That(actual, Is.EqualTo(3.14f));
        Assert.That(actual, Is.EqualTo(3.14f).Using(floatEqualityComparer));
    }

    [Test]
    public void DivideNumbers_WhenDivideByZero_ReturnError()
    {
        Calculator calculator = new Calculator();

        float actual = calculator.DivideNumbers(10, 0);
        //int actual = calculator.DivideNumbers(numA, numB);
        // LogAssert.Expect
        // Проверяет, что в журнале появляется сообщение журнала указанного типа. 
        // Тест не завершится неудачно из-за ожидаемой ошибки, утверждения или сообщения журнала исключений. 
        // Это не удается, если ожидаемое сообщение не появляется в журнале.
        // Если для ожидания нескольких сообщений используется несколько журналов Assert.Expect,
        // ожидается, что они будут записаны в этом порядке./// </summary>
        // <param name="type">Ожидаемый тип Expect. Это может занять один из [LogType enum](https://docs.unity3d.com/ScriptReference/LogType.html) values.</param>
        // <param name="message">Строковое значение, которое должно соответствовать ожидаемому сообщению..</param>

        // [Test]
        // public void LogAssertExample()
        // {
        //     // Expect a regular log message
        //     LogAssert.Expect(LogType.Log, "Log message");
        // 
        //     // The test fails without the following expected log message     
        //     Debug.Log("Log message");
        // 
        //     // An error log
        //     Debug.LogError("Error message");
        // 
        //     // Without expecting an error log, the test would fail
        //     LogAssert.Expect(LogType.Error, "Error message");
        // }
        //LogAssert.Expect(LogType.Error, "Cannot divide by zero!");
        LogAssert.Expect(LogType.Error, calculator.ErrorMessageProp);
    }

    /// <summary>
    /// Тест Unity ведет себя как сопрограмма в режиме воспроизведения.
    /// В режиме редактирования вы можете использовать
    /// `yield return null;` для пропуска кадра.
    /// </summary>
  /*  [UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses()
    {
        // Используйте класс Assert для проверки условий.
        // Используйте выход, чтобы пропустить кадр.
        yield return null;
    }*/
}
