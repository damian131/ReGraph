using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace ReGraph.Common
{
    /// <summary>
    /// Attachable Property which causes that only numeric values will be handled by TextBox.
    /// </summary>
    public class NumericOnlyTextBoxBehavior : DependencyObject, IBehavior
    {
        /// <summary>
        /// Gets the associated object.
        /// </summary>
        /// <value>
        /// The associated object.
        /// </value>
        public DependencyObject AssociatedObject
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the text box.
        /// </summary>
        /// <value>
        /// The text box.
        /// </value>
        private TextBox TextBox { get; set; }

        /// <summary>
        /// Attaches the specified associated object.
        /// </summary>
        /// <param name="associatedObject">The associated object.</param>
        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;
            TextBox = (TextBox)AssociatedObject;

            TextBox.TextChanged += TextBox_TextChanged;
          //  TextBox.KeyDown += TextBox_KeyDown;
        }

        /// <summary>
        /// Handles the TextChanged event of the TextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int selectionStart = TextBox.SelectionStart - 1;
            var matches = Regex.Matches(TextBox.Text, "^[1-9]+[0-9]*");
            String value = String.Empty;
            foreach (var m in matches)
                value += m;
            if(value != TextBox.Text)
            {
                TextBox.Text = value;
                selectionStart = Math.Max(0, selectionStart);
                TextBox.SelectionStart = (selectionStart <= TextBox.Text.Length) ? selectionStart : TextBox.Text.Length;
            }
        }

        /// <summary>
        /// Handles the KeyDown event of the TextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyRoutedEventArgs"/> instance containing the event data.</param>
        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Number0 ||
                e.Key == VirtualKey.Number1 ||
                e.Key == VirtualKey.Number2 ||
                e.Key == VirtualKey.Number3 ||
                e.Key == VirtualKey.Number4 ||
                e.Key == VirtualKey.Number5 ||
                e.Key == VirtualKey.Number6 ||
                e.Key == VirtualKey.Number7 ||
                e.Key == VirtualKey.Number8 ||
                e.Key == VirtualKey.Number9 ||
                e.Key == VirtualKey.Tab ||
                e.Key == VirtualKey.Escape)
            {
              
            }
            else
                e.Handled = true;

        }

        /// <summary>
        /// Detaches this instance.
        /// </summary>
        public void Detach()
        {
            TextBox.KeyDown -= TextBox_KeyDown;
            TextBox = null;
            AssociatedObject = null;
        }
    }
}
