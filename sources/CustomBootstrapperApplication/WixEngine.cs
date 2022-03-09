﻿// WiX Toolset Pills 15mg
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
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace DustInTheWind.BundleWithGui.Gui
{
    internal class WixEngine : IWixEngine
    {
        private readonly CustomBootstrapperApplication customBootstrapperApplication;

        public event EventHandler<DetectPackageEventArgs> DetectPackageComplete;
        public event EventHandler PlanBegin;
        public event EventHandler<PlanCompleteEventArgs> PlanComplete;
        public event EventHandler ApplyComplete;

        public WixEngine(CustomBootstrapperApplication customBootstrapperApplication)
        {
            this.customBootstrapperApplication = customBootstrapperApplication ?? throw new ArgumentNullException(nameof(customBootstrapperApplication));

            customBootstrapperApplication.DetectPackageComplete += HandleDetectPackageComplete;
            customBootstrapperApplication.PlanBegin += HandlePlanBegin;
            customBootstrapperApplication.PlanComplete += HandlePlanComplete;
            customBootstrapperApplication.ApplyComplete += HandleApplyComplete;
        }

        private void HandleDetectPackageComplete(object sender, DetectPackageCompleteEventArgs e)
        {
            DetectPackageEventArgs args = new DetectPackageEventArgs(e.State.ToLocalEntity());
            OnDetectPackageComplete(args);
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

        protected virtual void OnDetectPackageComplete(DetectPackageEventArgs e)
        {
            DetectPackageComplete?.Invoke(this, e);
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