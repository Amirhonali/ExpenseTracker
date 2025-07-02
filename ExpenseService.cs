using System.Text.Json;
using ExpenseTracker;

public class ExpenseService
{
    private const string FilePath = "expense.json";
    private List<Expense> expenses;
    public ExpenseService()
    {
        if (File.Exists(FilePath))
        {
            try
            {
                var json = File.ReadAllText(FilePath);
                expenses = JsonSerializer.Deserialize<List<Expense>>(json) ?? new();
            }
            catch
            {
                Console.WriteLine("Warning: Failed to read or parse expenses.json. Starting with empty list.");
                expenses = new List<Expense>();
            }
        }
        else
        {
            expenses = new List<Expense>();
        }
    }

    public void Save() =>
        File.WriteAllText(FilePath, JsonSerializer.Serialize(expenses, new JsonSerializerOptions { WriteIndented = true }));

    public void Add(string desc, decimal amount, string category)
    {
        var id = expenses.Any() ? expenses.Max(e => e.Id) + 1 : 1;
        expenses.Add(new Expense
        {
            Id = id,
            Description = desc,
            Amount = amount,
            Date = DateTime.Now,
            Category = category
        });
        Save();
        Console.WriteLine($"Expense added successfully (ID: {id})");
    }

    public void Delete(int id)
    {
        var exp = expenses.FirstOrDefault(e => e.Id == id);
        if (exp == null)
        {
            Console.WriteLine("Expense not found.");
            return;
        }
        expenses.Remove(exp);
        Save();
        Console.WriteLine("Expense delete successfully");
    }

    public void Update(int id, string? desc, decimal? amount, string? category)
    {
        var exp = expenses.FirstOrDefault(e => e.Id == id);
        if (exp == null)
        {
            Console.WriteLine("Expense not found");
            return;
        }

        if (!string.IsNullOrWhiteSpace(desc)) exp.Description = desc;
        if (amount.HasValue) exp.Amount = amount.Value;
        if (!string.IsNullOrWhiteSpace(category)) exp.Category = category;

        Save();
        Console.WriteLine("Expense updated successfully.");
    }

    public void List()
    {
        Console.WriteLine("ID  Date        Description        Amount   Category");
        foreach (var e in expenses)
        {
            Console.WriteLine($"{e.Id,-3} {e.Date:yyyy-MM-dd}  {e.Description,-18} ${e.Amount,-7} {e.Category}");
        }
    }

    public void Summery(int? month = null)
    {
        var filtered = expenses.Where(e => !month.HasValue || e.Date.Month == month);
        var total = filtered.Sum(e => e.Amount);
    }
}