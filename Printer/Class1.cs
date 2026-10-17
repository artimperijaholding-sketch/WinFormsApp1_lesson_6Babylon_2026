using SharedInterfaces;
using System.Diagnostics;

namespace Printer;

internal class Epson:IPrint
{
    public void Print(string data)
    {
        PrintViaPrintingHeader(data);
    }

    private void PrintViaPrintingHeader(string data)
    {
        Debug.WriteLine($"Epson: PrintViaPrintingHeader:started:{data}");
    }
}

