﻿namespace BackendFriend.Extensions.Exceptions
{
    public class MyBadRequestException : Exception
    {
        public MyBadRequestException(string message) : base(message)
        { }
    }
}
