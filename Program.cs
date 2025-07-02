using System.Globalization;

class Program
{
    static void Main(string[] args)
    {
        var service = new ExpenseService();

        if (args.Length == 0)
        {
            Console.WriteLine("Usage: add | delete | update | list | summary");
            return;
        }

        switch (args[0])
        {
            case "add":
                {
                    string desc = GetArg("--description");
                    string amountStr = GetArg("--amount");
                    string category = GetArg("--category") ?? "General";

                    if (desc == null || amountStr == null || !decimal.TryParse(amountStr, out var amount))
                    {
                        Console.WriteLine("Invalid input. Use --description and --amount.");
                        return;
                    }

                    service.Add(desc, amount, category);
                    break;
                }
            case "delete":
                {
                    if (int.TryParse(GetArg("--id"), out var id))
                        service.Delete(id);
                    else
                        Console.WriteLine("Missing or Invalid --id");
                    break;
                }
            
                        case "update":
                {
                    if (!int.TryParse(GetArg("--id"), out var id))
                    {
                        Console.WriteLine("Missing or invalid --id");
                        return;
                    }

                    var desc = GetArg("--description");
                    var amountStr = GetArg("--amount");
                    var category = GetArg("--category");
                    decimal? amount = decimal.TryParse(amountStr, out var a) ? a : null;

                    service.Update(id, desc, amount, category);
                    break;
                }

            case "list":
                service.List();
                break;

            case "summary":
                {
                    var monthStr = GetArg("--month");
                    int? month = int.TryParse(monthStr, out var m) ? m : null;
                    service.Summery(month);
                    break;
                }

            default:
                Console.WriteLine("Unknown command.");
                break;
        
        }

        string? GetArg(string name)
        {
            var index = Array.IndexOf(args, name);
            return (index != -1 && index + 1 < args.Length) ? args[index + 1] : null;
        }
    }
}