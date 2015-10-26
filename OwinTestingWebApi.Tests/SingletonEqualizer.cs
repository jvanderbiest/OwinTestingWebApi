using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.ModelBuilder;

namespace OwinTestingWebApi.Tests
{
    /// <summary>
    /// Replaces PerLifeStyleWebRequest to singleton as it is not supported in Unit Tests
    /// </summary>
    public class SingletonEqualizer : IContributeComponentModelConstruction
    {
        public void ProcessModel(IKernel kernel, ComponentModel model)
        {
            model.LifestyleType = LifestyleType.Singleton;
        }
    }
}