using CQRSCore.Commands;

namespace API.Commands;

public class DeletePostCommand: BaseCommand
{
    public string Username { get; set; }
}