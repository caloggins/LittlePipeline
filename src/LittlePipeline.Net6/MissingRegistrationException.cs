namespace LittlePipeline.Net6
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class MissingRegistrationException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public MissingRegistrationException()
        {
        }

        public MissingRegistrationException(string message) : base(message)
        {
        }

        public MissingRegistrationException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MissingRegistrationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}