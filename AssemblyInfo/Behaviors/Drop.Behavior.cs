using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace com.akoimeexx.utilities.assemblyinformation.Behaviors {
    public static partial class DropBehavior {
        public static readonly DependencyProperty DropCommandProperty =
            DependencyProperty.RegisterAttached(
                "DropCommand", 
                typeof(ICommand), 
                typeof(DropBehavior), 
                new FrameworkPropertyMetadata(
                    null, 
                    OnDropCommandChanged
                )
            );
        public static ICommand GetDropCommand(
            DependencyObject source
        ) {
            ICommand c = default(ICommand);
            try {
                c = (ICommand)source.GetValue(DropCommandProperty);
            } finally { }
            return c;
        }
        public static void SetDropCommand(
            DependencyObject target, 
            ICommand command
        ) {
            try {
                target.SetValue(DropCommandProperty, command);
            } finally { }
        }
        private static void OnDropCommandChanged(
            DependencyObject instance,
            DependencyPropertyChangedEventArgs dropArgs
        ) {
            if ((instance as UIElement) != null) try {
                UIElement elementInstance = (UIElement)instance;
                ICommand elementCommand = default(ICommand);
                
                if (dropArgs.OldValue as ICommand != null) {
                    elementCommand = ((ICommand)dropArgs.OldValue);
                    elementInstance.Drop -= (sender, args) => {
                        elementCommand.Execute(args.Data);
                    };
                }
                if (dropArgs.NewValue as ICommand != null) {
                    elementCommand = ((ICommand)dropArgs.NewValue);
                    elementInstance.Drop +=
                        (sender, args) => {
                            elementCommand.Execute(args.Data);
                        };
                }
            } finally { }
        }
    }
}
