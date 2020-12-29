using System;

namespace CloudAwesome.Xrm.Core.Exceptions
{
    public class FeatureRequestException: Exception
    {
        public FeatureRequestException(string message) :
            base(
                $"Exception: {message}. This functionality is not yet implemented but we're always up for feature requests " +
                $"with ideas for new functionality, or to push features up the backlog! " +
                $"Please raise an issue in the GitHub repo")
        { }

        public static FeatureRequestException NotImplementedFeatureException(string type)
        {
            return new FeatureRequestException($"The {type} feature is not implemented yet " +
                                               $"but we're always up for feature requests " +
                                               $"with ideas for new functionality, or to push features up the backlog! " +
                                               $"Please raise an issue in the GitHub repo");
        }

        public static FeatureRequestException PartiallyImplementedFeatureException(string type)
        {
            return new FeatureRequestException($"The {type} feature is only partially implemented " +
                                               $"but we're always up for feature requests " +
                                               $"with ideas for new functionality, or to push features up the backlog! " +
                                               $"Please raise an issue in the GitHub repo");
        }
    }
}
