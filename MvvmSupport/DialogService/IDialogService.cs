﻿using System;
using System.Threading.Tasks;

namespace MvvmSupport.DialogService {
    /// <summary>
    /// Interface to show modal and non-modal dialogs.
    /// </summary>
    public interface IDialogService {
#if !HAS_UWP && !HAS_WINUI
        /// <summary>
        /// Shows a non-modal dialog.
        /// </summary>
        /// <param name="name">The name of the dialog to show.</param>
        /// <param name="parameters">The parameters to pass to the dialog.</param>
        /// <param name="callback">The action to perform when the dialog is closed.</param>
        void Show( string name, IDialogParameters parameters, Action<IDialogResult> callback );
        void Show( string name, IDialogParameters parameters, Func<IDialogResult, Task> taskCallback );

        /// <summary>
        /// Shows a non-modal dialog.
        /// </summary>
        /// <param name="name">The name of the dialog to show.</param>
        /// <param name="parameters">The parameters to pass to the dialog.</param>
        /// <param name="callback">The action to perform when the dialog is closed.</param>
        /// <param name="windowName">The name of the hosting window registered with the IContainerRegistry.</param>
        void Show( string name, IDialogParameters parameters, Action<IDialogResult> callback, string windowName );
        void Show( string name, IDialogParameters parameters, Func<IDialogResult, Task> taskCallback, string windowName );
#endif

        /// <summary>
        /// Shows a modal dialog.
        /// </summary>
        /// <param name="name">The name of the dialog to show.</param>
        /// <param name="parameters">The parameters to pass to the dialog.</param>
        /// <param name="callback">The action to perform when the dialog is closed.</param>
        void ShowDialog( string name, IDialogParameters parameters, Action<IDialogResult> callback );
        void ShowDialog( string name, IDialogParameters parameters, Func<IDialogResult, Task> taskCallback );


        /// <summary>
        /// Shows a modal dialog.
        /// </summary>
        /// <param name="name">The name of the dialog to show.</param>
        /// <param name="parameters">The parameters to pass to the dialog.</param>
        /// <param name="callback">The action to perform when the dialog is closed.</param>
        /// <param name="windowName">The name of the hosting window registered with the IContainerRegistry.</param>
        void ShowDialog( string name, IDialogParameters parameters, Action<IDialogResult> callback, string windowName );
        void ShowDialog( string name, IDialogParameters parameters, Func<IDialogResult, Task> taskCallback, string windowName );
    }
}
