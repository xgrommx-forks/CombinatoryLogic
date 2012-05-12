﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace WPF_FrontEnd
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Output.InitializeColorScheme();
        }

        private void ExecuteBackend()
        {
            if (!IsInitialized)
                return;

            string arguments = "";

            if (OptSKI.IsChecked.Value)
                arguments += "--SKI ";
            if (OptParentheses.IsChecked.Value)
                arguments += "--show_parentheses ";
            if (OptDefaultCombinators.IsChecked.Value)
                arguments += "--use_predefined_symbols ";

            // Append the user defined symbols
            foreach (var line in Regex.Split(Combinators.Text, "\r\n"))
            {
                if (String.IsNullOrEmpty(line))
                    continue;
                arguments += "--symbol \"" + line + "\" ";
            }

            arguments += "-c \"" + Input.Text + "\"";

            var psi = new ProcessStartInfo
            {
                FileName = @"C:\Code\Haskell\CombinatoryLogic\Combinator.exe",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                Arguments = arguments,
                CreateNoWindow = true,
            };

            using (var process = Process.Start(psi))
            {
                using (var reader = process.StandardOutput)
                {
                    Output.Text = reader.ReadToEnd();
                }
            }
        }

        private void CurrentTerm_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExecuteBackend();
        }

        private void Combinators_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExecuteBackend();
        }

        private void Opt_CheckedChanged(object sender, RoutedEventArgs e)
        {
            ExecuteBackend();
        }
    }
}
