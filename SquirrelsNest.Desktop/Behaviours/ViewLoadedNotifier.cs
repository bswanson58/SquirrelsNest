using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace SquirrelsNest.Desktop.Behaviours {
    // usage:
    //
    // <FrameworkElement ... >
    //  <i:Interaction.Behaviors>
    //    <behaviours:ViewLoadedNotifier NotifyCommand="{Binding Command}" />
    //  </i:Interaction.Behaviors>
    // </FrameworkElement>

    public class ViewLoadedNotifier : Behavior<FrameworkElement> {
        public static readonly DependencyProperty NotifyCommandProperty = 
            DependencyProperty.RegisterAttached( "NotifyCommand", typeof( ICommand ), typeof( ViewLoadedNotifier ), new PropertyMetadata( null ));

        public ICommand ? NotifyCommand {
            get => GetValue( NotifyCommandProperty ) as ICommand;
            set => SetValue( NotifyCommandProperty, value );
        }

        protected override void OnAttached() {
            base.OnAttached();

            AssociatedObject.Loaded += OnLoaded;
            AssociatedObject.Unloaded += OnUnloaded;
        }

        private void OnLoaded( object sender, RoutedEventArgs args ) {
            NotifyCommand?.Execute( true );
        }

        private void OnUnloaded( object sender, RoutedEventArgs args ) {
            NotifyCommand?.Execute( false );
        }

        protected override void OnDetaching() {
            AssociatedObject.Loaded -= OnLoaded;
            AssociatedObject.Unloaded -= OnUnloaded;

            base.OnDetaching();
        }
    }
}
