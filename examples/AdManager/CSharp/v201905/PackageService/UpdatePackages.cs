// Copyright 2019 Google LLC
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

using Google.Api.Ads.AdManager.Lib;
using Google.Api.Ads.AdManager.Util.v201905;
using Google.Api.Ads.AdManager.v201905;

using System;

namespace Google.Api.Ads.AdManager.Examples.CSharp.v201905
{
    /// <summary>
    /// This code example updates the comments of a package. To determine which packages exist,
    /// run GetAllPackages.cs.
    /// </summary>
    public class UpdatePackages : SampleBase
    {
        /// <summary>
        /// Returns a description about the code example.
        /// </summary>
        public override string Description
        {
            get
            {
                return "This code example updates the comments of a package. To determine which " +
                    "packages exist, run GetAllPackages.cs.";
            }
        }

        /// <summary>
        /// Main method, to run this code example as a standalone application.
        /// </summary>
        public static void Main()
        {
            UpdatePackages codeExample = new UpdatePackages();
            Console.WriteLine(codeExample.Description);
            codeExample.Run(new AdManagerUser());
        }

        /// <summary>
        /// Run the code example.
        /// </summary>
        public void Run(AdManagerUser user)
        {
            using (PackageService packageService = user.GetService<PackageService>())
            {
                long packageId = long.Parse(_T("INSERT_PACKAGE_ID_HERE"));

                // Create a statement to get the package.
                StatementBuilder statementBuilder = new StatementBuilder()
                    .Where("id = :id")
                    .OrderBy("id ASC")
                    .Limit(1)
                    .AddValue("id", packageId);

                try
                {
                    // Get packages by statement.
                    PackagePage page =
                        packageService.getPackagesByStatement(statementBuilder.ToStatement());

                    Package package = page.results[0];

                    // Update the package object by changing its comments.
                    package.comments = "This package is ready to be made into proposal line items.";

                    // Update the package on the server.
                    Package[] packages = packageService.updatePackages(new Package[]
                    {
                        package
                    });

                    if (packages != null)
                    {
                        foreach (Package updatedPackage in packages)
                        {
                            Console.WriteLine(
                                "Package with ID = \"{0}\", name = \"{1}\", and " +
                                "comments = \"{2}\" was updated.",
                                updatedPackage.id, updatedPackage.name, updatedPackage.comments);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No packages updated.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to update packages. Exception says \"{0}\"",
                        e.Message);
                }
            }
        }
    }
}
