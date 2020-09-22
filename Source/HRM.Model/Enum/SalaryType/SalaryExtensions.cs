using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace HRM.Model.Enum.Salary
{
    public static class SalaryExtensions
    {
        public static SolidColorBrush ToForeground(this SalaryType type)
        {
            switch (type)
            {
                case SalaryType.Increase:
                    return new SolidColorBrush(Color.FromRgb(0, 197, 65));
                case SalaryType.Decrease:
                    return new SolidColorBrush(Color.FromRgb(255, 71, 71));
                case SalaryType.Net:
                    return new SolidColorBrush(Color.FromRgb(108, 185, 255));
                default:
                    return new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }
        }

        public static Visibility ToVisibility(this SalaryType type) 
        {
            switch (type)
            {
                case SalaryType.Net:
                    return Visibility.Collapsed;
                case SalaryType.Increase:
                case SalaryType.Decrease:
                default:
                    return Visibility.Visible;
            }
        }
    }
}
