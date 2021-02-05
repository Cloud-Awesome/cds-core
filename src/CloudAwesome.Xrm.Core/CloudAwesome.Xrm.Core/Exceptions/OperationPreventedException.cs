using System;

namespace CloudAwesome.Xrm.Core.Exceptions
{
    /// <summary>
    /// CRM message has been prevented from execution due to missing information, failed validation or
    /// prevented by configuration parameters in method call
    /// </summary>
    public class OperationPreventedException : Exception
    {
        /// <summary>
        /// CRM message has been prevented from execution due to missing information, failed validation or
        /// prevented by configuration parameters in method call
        /// </summary>
        public OperationPreventedException(): base()
        {

        }

        /// <summary>
        /// CRM message has been prevented from execution due to missing information, failed validation or
        /// prevented by configuration parameters in method call
        /// </summary>
        /// <param name="message"></param>
        public OperationPreventedException(string message) : base(message)
        {

        }

        /// <summary>
        /// CRM message has been prevented from execution due to missing information, failed validation or
        /// prevented by configuration parameters in method call
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public OperationPreventedException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
