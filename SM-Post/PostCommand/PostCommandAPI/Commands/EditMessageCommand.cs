using CQRSCore.Commands;

namespace API.Commands;

public class EditMessageCommand: BaseCommand
{
    public string Message { get; set; }
}