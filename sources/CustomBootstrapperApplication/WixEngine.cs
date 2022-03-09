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
using DustInTheWind.BundleWithCustomGui.CustomBootstrapperApplication.Domain;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using PlanCompleteEventArgs = DustInTheWind.BundleWithCustomGui.CustomBootstrapperApplication.Domain.PlanCompleteEventArgs;

namespace DustInTheWind.BundleWithCustomGui.CustomBootstrapperApplication
{
    internal class WixEngine : IWixEngine
    {
        private readonly CustomBootstrapperApplication customBootstrapperApplication;
        private readonly List<Package> packages = new List<Package>();

        public event EventHandler<DetectEventArgs> DetectComplete;
        public event EventHandler PlanBegin;
        public event EventHandler<PlanCompleteEventArgs> PlanComplete;
        public event EventHandler ApplyComplete;

        public WixEngine(CustomBootstrapperApplication customBootstrapperApplication)
        {
            this.customBootstrapperApplication = customBootstrapperApplication ?? throw new ArgumentNullException(nameof(customBootstrapperApplication));

            customBootstrapperApplication.DetectBegin += HandleDetectBegin;
            customBootstrapperApplication.DetectPackageComplete += HandleDetectPackageComplete;
            customBootstrapperApplication.DetectComplete += HandleDetectComplete;
            customBootstrapperApplication.PlanBegin += HandlePlanBegin;
            customBootstrapperApplication.PlanComplete += HandlePlanComplete;
            customBootstrapperApplication.ApplyComplete += HandleApplyComplete;
        }

        private void HandleDetectBegin(object sender, DetectBeginEventArgs e)
        {
            packages.Clear();
        }

        private void HandleDetectPackageComplete(object sender, DetectPackageCompleteEventArgs e)
        {
            Package package = new Package
            {
                Id = e.PackageId,
                State = e.State.ToLocalEntity()
            };
            packages.Add(package);
        }

        private void HandleDetectComplete(object sender, DetectCompleteEventArgs e)
        {
            DetectEventArgs args = new DetectEventArgs(packages);
            OnDetectComplete(args);
        }

        private void HandlePlanBegin(object sender, PlanBeginEventArgs e)
        {
            OnPlanBegin();
        }

        private void HandlePlanComplete(object sender, Microsoft.Tools.WindowsInstallerXml.Bootstrapper.PlanCompleteEventArgs e)
        {
            PlanCompleteEventArgs args = new PlanCompleteEventArgs(e.Status);
            OnPlanComplete(args);
        }

        private void HandleApplyComplete(object sender, ApplyCompleteEventArgs e)
        {
            OnApplyComplete();
        }

        public void Detect()
        {
            customBootstrapperApplication.Engine.Detect();
        }

        public void PlanInstall()
        {
            customBootstrapperApplication.Engine.Plan(LaunchAction.Install);
        }

        public void PlanUninstall()
        {
            customBootstrapperApplication.Engine.Plan(LaunchAction.Uninstall);
        }

        public void Apply()
        {
            customBootstrapperApplication.Engine.Apply(IntPtr.Zero);
        }

        public void InvokeShutDown()
        {
            customBootstrapperApplication.InvokeShutdown();
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