namespace MagpyServerLinux.Commands;

public class Utils
{
  public static bool GetUserConfirmation(string message)
  {
    Console.WriteLine(message);
    string? response = Console.ReadLine()?.Trim().ToLower();

    return response == "y" || response == "yes";
  }
}
