// <copyright file="TracerProviderOptionsTest.cs" company="OpenTelemetry Authors">
// Copyright The OpenTelemetry Authors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using System;
using System.Diagnostics;
using OpenTelemetry.Tests;
using Xunit;

namespace OpenTelemetry.Trace.Tests
{
    public class TracerProviderOptionsTest
    {
        private const string ActivitySourceName = "TracerProviderOptionsTest";

        [Fact]
        public void SetErrorStatusOnExceptionEnabled()
        {
            using var activitySource = new ActivitySource(ActivitySourceName);
            using var tracerProvider = Sdk.CreateTracerProviderBuilder(options =>
                {
                    options.SetErrorStatusOnException = true;
                })
                .AddSource(ActivitySourceName)
                .SetSampler(new AlwaysOnSampler())
                .Build();

            var activity = activitySource.StartActivity("Activity");

            try
            {
                using (activity)
                {
                    throw new Exception("Oops!");
                }
            }
            catch (Exception)
            {
            }

            Assert.Equal(StatusCode.Error, activity.GetStatus().StatusCode);
        }

        [Fact]
        public void SetErrorStatusOnExceptionDefault()
        {
            using var activitySource = new ActivitySource(ActivitySourceName);
            using var tracerProvider = Sdk.CreateTracerProviderBuilder(options => { })
                .AddSource(ActivitySourceName)
                .SetSampler(new AlwaysOnSampler())
                .Build();

            var activity = activitySource.StartActivity("Activity");

            try
            {
                using (activity)
                {
                    throw new Exception("Oops!");
                }
            }
            catch (Exception)
            {
            }

            Assert.Equal(StatusCode.Unset, activity.GetStatus().StatusCode);
        }
    }
}
