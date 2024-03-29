﻿using System;
using System.Threading.Tasks;

namespace MvvmSupport.DialogService {
    /// <summary>
    /// Extensions for the IDialogService
    /// </summary>
    public static class DialogServiceExtensions {
#if !HAS_UWP && !HAS_WINUI
        /// <summary>
        /// Shows a non-modal dialog.
        /// </summary>
        /// <param name="dialogService">The DialogService</param>
        /// <param name="name">The name of the dialog to show.</param>
        public static void Show( this IDialogService dialogService, string name ) {
            dialogService.Show( name, new DialogParameters(), _ => { });
        }

        /// <summary>
        /// Shows a non-modal dialog.
        /// </summary>
        /// <param name="dialogService">The DialogService</param>
        /// <param name="name">The name of the dialog to show.</param>
        /// <param name="callback">The action to perform when the dialog is closed.</param>
        public static void Show( this IDialogService dialogService, string name, Action<IDialogResult> callback ) {
            dialogService.Show( name, new DialogParameters(), callback );
        }

        public static void Show( this IDialogService dialogService, string name, Func<IDialogResult, Task> callback ) {
            dialogService.Show( name, new DialogParameters(), callback );
        }
#endif
        /// <summary>
        /// Shows a modal dialog.
        /// </summary>
        /// <param name="dialogService">The DialogService</param>
        /// <param name="name">The name of the dialog to show.</param>
        public static void ShowDialog( this IDialogService dialogService, string name ) {
            dialogService.ShowDialog( name, new DialogParameters(), _ => { });
        }

        /// <summary>
        /// Shows a modal dialog.
        /// </summary>
        /// <param name="dialogService">The DialogService</param>
        /// <param name="name">The name of the dialog to show.</param>
        /// <param name="callback">The action to perform when the dialog is closed.</param>
        public static void ShowDialog( this IDialogService dialogService, string name, Action<IDialogResult> callback ) {
            dialogService.ShowDialog( name, new DialogParameters(), callback );
        }

        public static void ShowDialog( this IDialogService dialogService, string name, Func<IDialogResult, Task> callback ) {
            dialogService.ShowDialog( name, new DialogParameters(), callback );
        }
    }
}
