using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Globalization;

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
    }
    public void Operador_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button boton)
        {
            if (double.TryParse(Pantalla.Text, NumberStyles.Float,
                CultureInfo.InvariantCulture, out double numero))
            {
                primerNumero = numero;
                operador = boton.Content?.ToString() ?? "";
                nuevoNumero = true;
            }
        }
    }
    public void Calcular_Click(object? sender, RoutedEventArgs e)
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
    public void Limpiar_Click(object? sender, RoutedEventArgs e)
    {
        Pantalla.Text = "0";
        primerNumero = 0;
        operador = "";
        nuevoNumero = true;
    }
    public void Borrar_Click(object? sender, RoutedEventArgs e)
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
}