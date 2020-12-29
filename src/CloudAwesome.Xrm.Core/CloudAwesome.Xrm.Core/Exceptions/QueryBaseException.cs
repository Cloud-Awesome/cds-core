using System;

namespace CloudAwesome.Xrm.Core.Exceptions
{
    /// <summary>
    /// Exception thrown in validation or expected results from a CRM Query
    /// (QueryExpression, FetchQuery or QueryByAttribute)
    /// </summary>
    public class QueryBaseException: Exception
    {
        /// <summary>
        /// Exception thrown in validation or expected results from a CRM Query
        /// (QueryExpression, FetchQuery or QueryByAttribute)
        /// </summary>
        public QueryBaseException()
        {

        }

        /// <summary>
        /// Exception thrown in validation or expected results from a CRM Query
        /// (QueryExpression, FetchQuery or QueryByAttribute)
        /// </summary>
        /// <param name="message"></param>
        public QueryBaseException(string message) : base(message)
        {

        }

        /// <summary>
        /// Exception thrown in validation or expected results from a CRM Query
        /// (QueryExpression, FetchQuery or QueryByAttribute)
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public QueryBaseException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
