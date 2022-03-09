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
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using DustInTheWind.BundleWithCustomGui.CustomBootstrapperApplication.Domain;

namespace DustInTheWind.BundleWithCustomGui.CustomBootstrapperApplication.Presentation.Commands
{
    public class UninstallCommand : ICommand
    {
        private static Dispatcher dispatcher;
        private readonly IWixEngine wixEngine;
        private volatile bool canExecute;

        public event EventHandler CanExecuteChanged;

        public UninstallCommand(IWixEngine wixEngine)
        {
            this.wixEngine = wixEngine ?? throw new ArgumentNullException(nameof(wixEngine));

            dispatcher = Dispatcher.CurrentDispatcher;

            wixEngine.PlanBegin += HandlePlanBegin;
            wixEngine.DetectComplete += HandleDetectComplete;
        }

        private void HandlePlanBegin(object sender, EventArgs e)
        {
            dispatcher.Invoke(() =>
            {
                canExecute = false;
                OnCanExecuteChanged();
            });
        }

        private void HandleDetectComplete(object sender, DetectEventArgs e)
        {
            dispatcher.Invoke(() =>
            {
                bool isAnyPackagePresent = e.Packages.Any(x => x.State == PackageState.Present);

                if (isAnyPackagePresent)
                {
                    canExecute = true;
                    OnCanExecuteChanged();
                }
            });
        }

        public bool CanExecute(object parameter)
        {
            return canExecute;
        }

        public void Execute(object parameter)
        {
            wixEngine.PlanUninstall();
        }

        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}