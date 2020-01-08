﻿using Raven.Client.ServerWide.Operations;
using Raven.Client.Documents;
using System.Threading.Tasks;

namespace Raven.Documentation.Samples.ClientApi.Operations.ServerWide
{
    class Class1
    {
        private class Foo
        {
            public class SetDatabaseDynamicDistributionOperation
            {
                #region syntax
                public SetDatabaseDynamicDistributionOperation(string databaseName, bool allowDynamicDistribution)
                #endregion
                {
                }
            }
        }

        static async Task MainInternal()
        {
            using (IDocumentStore documentStore = new DocumentStore())
            {
                #region operation

                SetDatabaseDynamicDistributionOperation operation =
                    new SetDatabaseDynamicDistributionOperation("NorthWind", true);
                documentStore.Maintenance.Server.Send(operation);

                #endregion
            }
        }
    }
}
