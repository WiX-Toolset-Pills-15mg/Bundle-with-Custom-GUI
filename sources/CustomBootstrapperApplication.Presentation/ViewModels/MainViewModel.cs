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
using DustInTheWind.BundleWithCustomGui.CustomBootstrapperApplication.Presentation.Commands;

namespace DustInTheWind.BundleWithCustomGui.CustomBootstrapperApplication.Presentation.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private static Dispatcher dispatcher;

        private bool isLoading;

        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                OnPropertyChanged();
            }
        }

        public InstallCommand InstallCommand { get; }

        public UninstallCommand UninstallCommand { get; }

        public ExitCommand ExitCommand { get; }

        public MainViewModel(IWixEngine wixEngine)
        {
            dispatcher = Dispatcher.CurrentDispatcher;

            InstallCommand = new InstallCommand(wixEngine);
            UninstallCommand = new UninstallCommand(wixEngine);
            ExitCommand = new ExitCommand(wixEngine);

            wixEngine.PlanBegin += HandlePlanBegin;
            wixEngine.ApplyComplete += HandleApplyComplete;
        }

        private void HandlePlanBegin(object sender, EventArgs e)
        {
            dispatcher.Invoke(() =>
            {
                IsLoading = true;
            });
        }

        private void HandleApplyComplete(object sender, EventArgs e)
        {
            dispatcher.Invoke(() =>
            {
                IsLoading = false;
            });
        }
    }
}