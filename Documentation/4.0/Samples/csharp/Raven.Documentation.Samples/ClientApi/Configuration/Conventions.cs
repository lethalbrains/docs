﻿using System;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Sparrow;

namespace Raven.Documentation.Samples.ClientApi.Configuration
{
    public class Conventions
    {
        public Conventions()
        {
            #region conventions_1
            using (var store = new DocumentStore()
            {
                Conventions =
                {
                    // customizations go here
                }
            }.Initialize())
            {

            }
            #endregion
        }

        public void Examples()
        {
            var store = new DocumentStore()
            {
                Conventions =
                {
	                #region MaxHttpCacheSize
	                MaxHttpCacheSize = new Size(256, SizeUnit.Megabytes)
	                #endregion
	                ,
	                #region MaxNumberOfRequestsPerSession
	                MaxNumberOfRequestsPerSession = 10
	                #endregion
	                ,
	                #region UseOptimisticConcurrency
	                UseOptimisticConcurrency = true
	                #endregion
	                ,
	                #region DisableTopologyUpdates
                    DisableTopologyUpdates = false
	                #endregion
                    ,
	                #region SaveEnumsAsIntegers
                    SaveEnumsAsIntegers = true
	                #endregion
                    ,
                    #region RequestTimeout
                    RequestTimeout = TimeSpan.FromSeconds(90)
                    #endregion
                    ,
                    #region UseCompression
                    UseCompression = true
                    #endregion
                    ,
                    #region OperationStatusFetchMode
                    OperationStatusFetchMode = OperationStatusFetchMode.ChangesApi
                    #endregion
	            }
            };

            var stor2e = new DocumentStore()
            {
                Conventions =
                {
                    #region disable_cache
                    MaxHttpCacheSize = new Size(0, SizeUnit.Megabytes)
                    #endregion
                }
            };
        }
    }
}
