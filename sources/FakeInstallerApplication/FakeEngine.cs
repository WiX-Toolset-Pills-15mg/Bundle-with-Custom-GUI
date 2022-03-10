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
using System.Collections.Generic;
using System.Threading.Tasks;
using DustInTheWind.BundleWithCustomGui.CustomBootstrapperApplication.Domain;

namespace DustInTheWind.BundleWithCustomGui.DesktopApplication
{
    internal class FakeEngine : IInstallerEngine
    {
        private readonly App app;

        private readonly List<Package> packages = new List<Package>
        {
            new Package
            {
                Id = "Installer 1",
                State = PackageState.Absent
            },
            new Package
            {
                Id = "Installer 2",
                State = PackageState.Absent
            },
            new Package
            {
                Id = "Installer 3",
                State = PackageState.Absent
            }
        };

        public event EventHandler<DetectEventArgs> DetectComplete;
        public event EventHandler PlanBegin;
        public event EventHandler<PlanCompleteEventArgs> PlanComplete;
        public event EventHandler ApplyComplete;

        public FakeEngine(App app)
        {
            this.app = app ?? throw new ArgumentNullException(nameof(app));
        }

        public async void Detect()
        {
            await Task.Delay(2000);

            DetectEventArgs args = new DetectEventArgs(packages);
            OnDetectComplete(args);
        }

        public async void PlanInstall()
        {
            OnPlanBegin();

            await Task.Delay(1000);

            PlanCompleteEventArgs args = new PlanCompleteEventArgs(3);
            OnPlanComplete(args);
        }

        public async void PlanUninstall()
        {
            OnPlanBegin();

            await Task.Delay(1000);

            PlanCompleteEventArgs args = new PlanCompleteEventArgs(3);
            OnPlanComplete(args);
        }

        public async void Apply()
        {
            await Task.Delay(5000);
            OnApplyComplete();
        }

        public void InvokeShutDown()
        {
            app.Shutdown();
        }

        protected virtual void OnDetectComplete(DetectEventArgs e)
        {
            DetectComplete?.Invoke(this, e);
        }

        protected virtual void OnPlanBegin()
        {
            PlanBegin?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnPlanComplete(PlanCompleteEventArgs e)
        {
            PlanComplete?.Invoke(this, e);
        }

        protected virtual void OnApplyComplete()
        {
            ApplyComplete?.Invoke(this, EventArgs.Empty);
        }
    }
}