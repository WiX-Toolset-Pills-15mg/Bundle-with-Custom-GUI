// WiX Toolset Pills 15mg
// Copyright (C) 2019-2022 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using DustInTheWind.BundleWithCustomGui.CustomBootstrapperApplication.Domain;

namespace DustInTheWind.BundleWithCustomGui.CustomBootstrapperApplication
{
    internal static class PackageStateExtensions
    {
        public static PackageState ToLocalEntity(this Microsoft.Tools.WindowsInstallerXml.Bootstrapper.PackageState packageState)
        {
            switch (packageState)
            {
                case Microsoft.Tools.WindowsInstallerXml.Bootstrapper.PackageState.Unknown:
                    return PackageState.Unknown;

                case Microsoft.Tools.WindowsInstallerXml.Bootstrapper.PackageState.Obsolete:
                    return PackageState.Obsolete;

                case Microsoft.Tools.WindowsInstallerXml.Bootstrapper.PackageState.Absent:
                    return PackageState.Absent;

                case Microsoft.Tools.WindowsInstallerXml.Bootstrapper.PackageState.Cached:
                    return PackageState.Cached;

                case Microsoft.Tools.WindowsInstallerXml.Bootstrapper.PackageState.Present:
                    return PackageState.Present;

                case Microsoft.Tools.WindowsInstallerXml.Bootstrapper.PackageState.Superseded:
                    return PackageState.Superseded;

                default:
                    throw new ArgumentOutOfRangeException(nameof(packageState), packageState, null);
            }
        }
    }
}