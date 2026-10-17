using SharedInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Printer;

public class PrinterFactoryes
{
    public IPrint GetPrinter() => new Epson();
        
}
