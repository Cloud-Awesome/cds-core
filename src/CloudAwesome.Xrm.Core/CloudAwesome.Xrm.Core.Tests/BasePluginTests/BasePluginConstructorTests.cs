using NUnit.Framework;

namespace CloudAwesome.Xrm.Core.Tests.BasePluginTests
{
    [TestFixture]
    public class BasePluginConstructorTests
    {
        // Decide how best to test the base class... Directly? And/Or create a sample plugin and execute that?

        // TODO - empty constructor

        // TODO - secure and unsecure config
        
    }

    //public class SamplePlugin : BasePlugin
    //{
    //    SamplePlugin()
    //    {
    //        RegisterStep(PluginStage.PostOperation, PluginExecutionMode.Asynchronous,
    //            "create", Account.EntityLogicalName, Run, null);
    //    }

    //    public void Run()
    //    {

    //    }
    //}
}