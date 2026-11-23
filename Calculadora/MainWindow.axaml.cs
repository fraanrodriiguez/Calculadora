using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Input;
using Avalonia.Layout;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Calculadora;

public partial class MainWindow : Window
{
    private double primerNumero = 0;
    private string operador = "";
    private bool nuevoNumero = true;

    public MainWindow()
    {
        InitializeComponent();
    }

    public void Numero_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button boton)
        {
            string numero = boton.Content?.ToString() ?? "";
            EscribirNumero(numero);
        }
    }

    private void EscribirNumero(string numero)
    {
        if (Pantalla.Text == "Error")
            Limpiar();

        if (nuevoNumero || Pantalla.Text == "0")
        {
            Pantalla.Text = numero;
            nuevoNumero = false;
        }
        else
        {
            Pantalla.Text += numero;
        }
    }

    public void Operador_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button boton)
        {
            string op = boton.Content?.ToString() ?? "";
            EscribirOperador(op);
        }
    }

    private void EscribirOperador(string op)
    {
        if (double.TryParse(Pantalla.Text, NumberStyles.Float,
            CultureInfo.InvariantCulture, out double numero))
        {
            primerNumero = numero;
            operador = op;
            nuevoNumero = true;
        }
    }

    public async void Calcular_Click(object? sender, RoutedEventArgs e)
    {
        await CalcularAsync();
    }

    private async Task CalcularAsync()
    {
        if (!double.TryParse(Pantalla.Text, NumberStyles.Float,
            CultureInfo.InvariantCulture, out double segundoNumero))
            return;
        double resultado = 0;
        switch (operador)
        {
            case "+":
                resultado = primerNumero + segundoNumero;
                break;
            case "-":
                resultado = primerNumero - segundoNumero;
                break;
            case "x":
                resultado = primerNumero * segundoNumero;
                break;
            case "/":
                if (segundoNumero == 0)
                {
                    Pantalla.Text = "Error";
                    nuevoNumero = true;
                    operador = "";
                    await MostrarError("No se puede dividir entre cero.");
                    return;
                }
                resultado = primerNumero / segundoNumero;
                break;
            default:
                return;
        }
        Pantalla.Text = resultado.ToString("G", CultureInfo.InvariantCulture);
        operador = "";
        nuevoNumero = true;
    }

    private async Task MostrarError(string mensaje)
    {
        var ventanaError = new Window
        {
            Title = "Error",
            Width = 320,
            Height = 160,
            CanResize = false,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };
        var texto = new TextBlock
        {
            Text = mensaje,
            TextWrapping = Avalonia.Media.TextWrapping.Wrap,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        var botonAceptar = new Button
        {
            Content = "Aceptar",
            Width = 100,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        botonAceptar.Click += (_, _) => ventanaError.Close();
        ventanaError.Content = new StackPanel
        {
            Margin = new Avalonia.Thickness(20),
            Spacing = 20,
            VerticalAlignment = VerticalAlignment.Center,
            Children =
            {
                texto,
                botonAceptar
            }
        };

        await ventanaError.ShowDialog(this);
    }

    public void Limpiar_Click(object? sender, RoutedEventArgs e)
    {
        Limpiar();
    }

    private void Limpiar()
    {
        Pantalla.Text = "0";
        primerNumero = 0;
        operador = "";
        nuevoNumero = true;
    }

    public void Borrar_Click(object? sender, RoutedEventArgs e)
    {
        BorrarUltimo();
    }

    private void BorrarUltimo()
    {
        if (Pantalla.Text == "Error")
        {
            Pantalla.Text = "0";
            nuevoNumero = true;
            return;
        }
        if (!nuevoNumero && Pantalla.Text?.Length > 1)
        {
            Pantalla.Text = Pantalla.Text![..^1];
        }
        else
        {
            Pantalla.Text = "0";
            nuevoNumero = true;
        }
    }

    public void Decimal_Click(object? sender, RoutedEventArgs e)
    {
        EscribirDecimal();
    }

    private void EscribirDecimal()
    {
        if (Pantalla.Text == "Error")
            Limpiar();
        if (nuevoNumero)
        {
            Pantalla.Text = "0.";
            nuevoNumero = false;
        }
        else if (Pantalla.Text?.Contains(".") != true)
        {
            Pantalla.Text += ".";
        }
    }

    private async void Window_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.D0:
            case Key.NumPad0:
                EscribirNumero("0");
                break;
            case Key.D1:
            case Key.NumPad1:
                EscribirNumero("1");
                break;
            case Key.D2:
            case Key.NumPad2:
                EscribirNumero("2");
                break;
            case Key.D3:
            case Key.NumPad3:
                EscribirNumero("3");
                break;
            case Key.D4:
            case Key.NumPad4:
                EscribirNumero("4");
                break;
            case Key.D5:
            case Key.NumPad5:
                EscribirNumero("5");
                break;
            case Key.D6:
            case Key.NumPad6:
                EscribirNumero("6");
                break;
            case Key.D7:
            case Key.NumPad7:
                EscribirNumero("7");
                break;
            case Key.D8:
            case Key.NumPad8:
                EscribirNumero("8");
                break;
            case Key.D9:
            case Key.NumPad9:
                EscribirNumero("9");
                break;
            case Key.Add:
            case Key.OemPlus:
                EscribirOperador("+");
                break;
            case Key.Subtract:
            case Key.OemMinus:
                EscribirOperador("-");
                break;
            case Key.Multiply:
                EscribirOperador("x");
                break;
            case Key.Divide:
                EscribirOperador("/");
                break;
            case Key.Decimal:
            case Key.OemPeriod:
            case Key.OemComma:
                EscribirDecimal();
                break;
            case Key.Enter:
                await CalcularAsync();
                break;
            case Key.Back:
                BorrarUltimo();
                break;
            case Key.Escape:
                Limpiar();
                break;
            default:
                return;
        }
        e.Handled = true;
    }
}
