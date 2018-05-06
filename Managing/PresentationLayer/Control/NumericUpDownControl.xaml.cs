using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Managing.PresentationLayer.Control
{
    /// <summary>
    /// Represents a NumericUpDownControl control.
    /// </summary>
    public partial class NumericUpDownControl : UserControl
    {
        private readonly Regex _numMatch = new Regex(@"^-?\d*$");
        private readonly string _minus = "-";

        private static readonly RoutedEvent ValueChangedEvent =
                   EventManager.RegisterRoutedEvent(nameof(ValueChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NumericUpDownControl));

        private static readonly RoutedEvent IncreaseClickedEvent =
            EventManager.RegisterRoutedEvent(nameof(IncreaseClicked), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NumericUpDownControl));

        private static readonly RoutedEvent DecreaseClickedEvent =
            EventManager.RegisterRoutedEvent(nameof(DecreaseClicked), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NumericUpDownControl));

        /// <summary>
        /// Dependency property for "IsNullable".
        /// </summary>
        public static readonly DependencyProperty IsNullableProperty =
            DependencyProperty.Register(nameof(IsNullable), typeof(bool), typeof(NumericUpDownControl), new PropertyMetadata(false));

        /// <summary>
        /// Dependency property for "Value".
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(int?), typeof(NumericUpDownControl), new PropertyMetadata(0, PropertyValueChanged));

        /// <summary>
        /// Dependency property for "Minimum".
        /// </summary>
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(nameof(Minimum), typeof(int), typeof(NumericUpDownControl), new UIPropertyMetadata(0));


        /// <summary>
        /// Dependency property for "Maximum".
        /// </summary>
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(nameof(Maximum), typeof(int), typeof(NumericUpDownControl), new UIPropertyMetadata(100));

        /// <summary>
        /// Dependency property for "IsValid".
        /// </summary>
        private static readonly DependencyPropertyKey IsValidProperty =
            DependencyProperty.RegisterReadOnly(nameof(IsValid), typeof(bool), typeof(NumericUpDownControl), new PropertyMetadata(true));

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericUpDownControl"/> class.
        /// </summary>
        public NumericUpDownControl()
        {
            InitializeComponent();

            UpdateText(null);
        }

        /// <summary>
        /// Gets or sets whether the <see cref="NumericUpDownControl"/> control is nullable or not.
        /// </summary>
        public bool IsNullable
        {
            get => (bool)GetValue(IsNullableProperty);
            set => SetValue(IsNullableProperty, value);
        }

        /// <summary>
        /// Gets or sets a value of the <see cref="NumericUpDownControl"/> control.
        /// </summary>
        public int? Value
        {
            get => (int?)GetValue(ValueProperty);
            set
            {
                UpdateText(value);
                SetValue(ValueProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a minimum value of the <see cref="NumericUpDownControl"/> control.
        /// </summary>
        public int Minimum
        {
            get => (int)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        /// <summary>
        /// Gets or sets a maximum value for the <see cref="NumericUpDownControl"/> control.
        /// </summary>
        public int Maximum
        {
            get => (int)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        /// <summary>
        /// Gets a value indicating whether the value of the <see cref="NumericUpDownControl"/> is valid.
        /// </summary>
        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty.DependencyProperty);
            private set => SetValue(IsValidProperty, value);
        }

        /// <summary>
        /// The ValueChanged event is called when the TextBoxValue of the control changes.
        /// </summary>
        public event RoutedEventHandler ValueChanged
        {
            add => AddHandler(ValueChangedEvent, value);
            remove => RemoveHandler(ValueChangedEvent, value);
        }

        /// <summary>
        /// Occurs when the increase button is clicked.
        /// </summary>
        public event RoutedEventHandler IncreaseClicked
        {
            add => AddHandler(IncreaseClickedEvent, value);
            remove => RemoveHandler(IncreaseClickedEvent, value);
        }

        /// <summary>
        /// Occurs when the decrease button is clicked.
        /// </summary>
        public event RoutedEventHandler DecreaseClicked
        {
            add => AddHandler(DecreaseClickedEvent, value);
            remove => RemoveHandler(DecreaseClickedEvent, value);
        }

        public void FocusAll()
        {
            Keyboard.Focus(TextBoxValue);
            TextBoxValue.SelectAll();
        }

        private static void PropertyValueChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var numericUpDown = (NumericUpDownControl)target;
            if (numericUpDown == null)
            {
                return;
            }

            if (!numericUpDown.IsValid)
            {
                return;
            }

            numericUpDown.UpdateText((int?)e.NewValue);
        }

        private int GetDefaultValue(int value = 0)
        {
            return Math.Min(Math.Max(value, Minimum), Maximum);
        }

        private void UpdateText(int? value)
        {
            if (value == null)
            {
                TextBoxValue.Text = IsNullable ? string.Empty : GetDefaultValue().ToString();
                TextBoxValue.SelectAll();
            }
            else
            {
                TextBoxValue.Text = value.ToString();
            }
        }

        private void ValuePreviewKeyDownEventHandler(object sender, KeyEventArgs e)
        {
            if (e.IsDown && e.Key == Key.Up && (Value ?? Maximum - 1) < Maximum)
            {
                UpdateValue(Value == null ? GetDefaultValue() : Value + 1);
                RaiseEvent(new RoutedEventArgs(IncreaseClickedEvent));
            }
            else if (e.IsDown && e.Key == Key.Down && (Value ?? Minimum + 1) > Minimum)
            {
                UpdateValue(Value == null ? GetDefaultValue() : Value - 1);
                RaiseEvent(new RoutedEventArgs(DecreaseClickedEvent));
            }
        }

        private void ValueTextChangedEventHandler(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;

            if (!_numMatch.IsMatch(textBox.Text))
            {
                UpdateText(Value);

                return;
            }

            if (string.Equals(textBox.Text, _minus))
            {
                if (Minimum < 0)
                {
                    IsValid = false;
                    var newValue = IsNullable ? (int?)null : GetDefaultValue();
                    SetCurrentValue(ValueProperty, newValue);

                    return;
                }

                UpdateText(Value);
            }

            var value = string.IsNullOrEmpty(textBox.Text) ? (IsNullable ? (int?)null : GetDefaultValue()) : GetDefaultValue(Convert.ToInt32(textBox.Text));
            UpdateValue(value);
            UpdateText(value);

            RaiseEvent(new RoutedEventArgs(ValueChangedEvent));
        }

        private void IncreaseClickEventHandler(object sender, RoutedEventArgs e)
        {
            if ((Value ?? Maximum - 1) >= Maximum)
            {
                return;
            }

            UpdateValue(Value == null ? GetDefaultValue() : Value + 1);
            RaiseEvent(new RoutedEventArgs(IncreaseClickedEvent));
        }

        private void DecreaseClickEventHandler(object sender, RoutedEventArgs e)
        {
            if ((Value ?? Minimum + 1) <= Minimum)
            {
                return;
            }

            UpdateValue(Value == null ? GetDefaultValue() : Value - 1);
            RaiseEvent(new RoutedEventArgs(DecreaseClickedEvent));
        }

        private void UpdateValue(int? value)
        {
            IsValid = true;
            SetCurrentValue(ValueProperty, value);
        }
    }
}
