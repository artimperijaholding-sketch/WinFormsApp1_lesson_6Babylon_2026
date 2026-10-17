using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace WinFormsApp1_lesson_6Babylon_2026;

internal class LibraryScanner
{
    private const string spacing = "  ";

    public static string ScanLibrary(string dllPath)
    {
        StringBuilder report = new StringBuilder();
        report.AppendLine($"=== SCAN REPORT: {dllPath} ===");

        try
        {
            Assembly assembly = Assembly.LoadFrom(dllPath);

            Type[] types = assembly.GetTypes();
            report.AppendLine($"TYPES: {types.Length}");

            foreach (Type type in types)
            {
                if (!type.IsClass)
                    continue;

                report.AppendLine($"\n[CLASS]: {type.FullName}");

                MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

                if (methods.Length == 0)
                {
                    report.AppendLine($"{spacing}(No public methods)");
                }
                else
                {
                    report.AppendLine($"{spacing}[METHODS]:");
                    foreach (MethodInfo method in methods)
                    {
                        string returnType = method.ReturnType.Name;
                        report.AppendLine($"{spacing}{spacing} -> {returnType} {method.Name}()");

                        ParameterInfo[] parameters = method.GetParameters();
                        if (parameters.Length > 0)
                        {
                            report.AppendLine($"{spacing}{spacing}{spacing}Parameters:");
                            foreach (ParameterInfo param in parameters)
                            {
                                string prefix = param.IsOut ? "out " : (param.ParameterType.IsByRef ? "ref " : "");
                                string typeName = param.ParameterType.Name.Replace("&", "");
                                report.AppendLine($"{spacing}{spacing}{spacing}{spacing} - {prefix}{typeName} {param.Name}");
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            report.AppendLine($"ERROR SCANNING: {ex.Message}");
        }

        return report.ToString();
    }

}
