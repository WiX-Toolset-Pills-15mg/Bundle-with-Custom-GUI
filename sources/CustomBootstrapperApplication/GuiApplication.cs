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
using DustInTheWind.BundleWithGui.Gui.ViewModels;
using DustInTheWind.BundleWithGui.Gui.Views;

namespace DustInTheWind.BundleWithGui.Gui
{
    internal class GuiApplication
    {
        private readonly IWixEngine wixEngine;
        private readonly MainWindow mainView;
        private readonly Dispatcher dispatcher;

        public GuiApplication(IWixEngine wixEngine)
        {
            this.wixEngine = wixEngine ?? throw new ArgumentNullException(nameof(wixEngine));

            wixEngine.PlanComplete += HandlePlanComplete;

            dispatcher = Dispatcher.CurrentDispatcher;

            MainViewModel viewModel = new MainViewModel(wixEngine);

            mainView = new MainWindow { DataContext = viewModel };
            mainView.Closed += HandleMainViewClosed;
        }

        private void HandleMainViewClosed(object sender, EventArgs e)
        {
            wixEngine.InvokeShutDown();
        }

        private void HandlePlanComplete(object sender, PlanCompleteEventArgs e)
        {
            if (e.Status >= 0)
                wixEngine.Apply();
        }

        public void Run()
        {
            wixEngine.Detect();
            mainView.Show();

            Dispatcher.Run();
        }

        public void InvokeShutdown()
        {
            dispatcher.InvokeShutdown();
        }
    }
}