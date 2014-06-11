using System;
using System.Collections;

class BITalinoException : Exception
{
    private string message;

    #region GETTER/SETTER

    int Value { get; set; }

    public override string Message { get { return message; } }

    #endregion

    public BITalinoException ( BITalinoErrorTypes errorType )
    {
        Value = errorType.Value;

        this.message = errorType.Message;
    }
}
