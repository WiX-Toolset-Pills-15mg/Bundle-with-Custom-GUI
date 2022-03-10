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
using System.Windows.Threading;
using DustInTheWind.BundleWithCustomGui.CustomBootstrapperApplication.Domain;
using DustInTheWind.BundleWithCustomGui.CustomBootstrapperApplication.Presentation.ViewModels;
using DustInTheWind.BundleWithCustomGui.CustomBootstrapperApplication.Presentation.Views;

namespace DustInTheWind.BundleWithCustomGui.CustomBootstrapperApplication.Presentation
{
    public class GuiApplication
    {
        private readonly IInstallerEngine installerEngine;
        private readonly MainWindow mainView;
        private readonly Dispatcher dispatcher;

        public GuiApplication(IInstallerEngine installerEngine)
        {
            this.installerEngine = installerEngine ?? throw new ArgumentNullException(nameof(installerEngine));

            installerEngine.PlanComplete += HandlePlanComplete;

            dispatcher = Dispatcher.CurrentDispatcher;

            MainViewModel viewModel = new MainViewModel(installerEngine);

            mainView = new MainWindow { DataContext = viewModel };
            mainView.Closed += HandleMainViewClosed;
        }

        private void HandleMainViewClosed(object sender, EventArgs e)
        {
            installerEngine.InvokeShutDown();
        }

        private void HandlePlanComplete(object sender, PlanCompleteEventArgs e)
        {
            if (e.Status >= 0)
                installerEngine.Apply();
        }

        public void Run()
        {
            installerEngine.Detect();
            mainView.Show();

            Dispatcher.Run();
        }

        public void InvokeShutdown()
        {
            dispatcher.InvokeShutdown();
        }
    }
}