﻿namespace MatthewDotCare.XStatic
{
    public interface IXStaticWebResult
    {
        bool WasSuccessful { get; set; }

        string Message { get; set; }
    }
}