using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PrimalEditor.Dictionaries
{
    public partial class ControlTemplates : ResourceDictionary
    {
        private void OnTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            var exp = textBox.GetBindingExpression(TextBox.TextProperty);
            if (exp == null) return;

            if(e.Key == Key.Enter)
            { 
                if(textBox.Tag is ICommand command && command.CanExecute(textBox.Text)) // Is there a command in the tag? Can it execute? Then do so
                {
                    command.Execute(textBox.Text);
                }
                else // binds directly to the property without a command. 
                {
                    exp.UpdateSource();
                }
                Keyboard.ClearFocus();
                e.Handled = true; // similiar to consumed inputs in UE.
            }
            else if(e.Key == Key.Escape)
            {
                exp.UpdateTarget(); // read-backs old values
                Keyboard.ClearFocus();
            }
        }
    }
}
