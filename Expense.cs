using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker
{
   public class Expense
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = "";
    public decimal Amount { get; set; }
    public string Category { get; set; } = "General";
}
}