using System.Windows;
using System.Windows.Controls;
using SquirrelsNest.Desktop.Platform;
using SquirrelsNest.Desktop.ViewModels;

namespace SquirrelsNest.Desktop.Views.ViewSupport {
    public enum IssueViewStyle {
        TitleOnly,
        TitleDescription,
        TitleEntities,
        Everything
    }

    internal class IssueListTemplateSelector : DataTemplateSelector {
        public DataTemplate? TitleOnlyTemplate { get; set; }
        public DataTemplate? TitleDescriptionTemplate { get; set; }
        public DataTemplate? TitleEntitiesTemplate { get; set; }
        public DataTemplate? EverythingTemplate { get; set; }

        public override DataTemplate? SelectTemplate( object item, DependencyObject container ) {
            var retValue = EverythingTemplate;

            if( container is FrameworkElement element ) {
                var userControl = element.FindVisualAscendant<UserControl>();

                if( userControl?.DataContext is IssueListViewModel vm ) {
                    retValue = vm.DisplayStyle switch {
                        IssueViewStyle.TitleOnly => TitleOnlyTemplate,
                        IssueViewStyle.TitleDescription => TitleDescriptionTemplate,
                        IssueViewStyle.TitleEntities => TitleEntitiesTemplate,
                        IssueViewStyle.Everything => EverythingTemplate,
                        _ => EverythingTemplate
                    };
                }
            }

            return retValue;
        }
    }
}
