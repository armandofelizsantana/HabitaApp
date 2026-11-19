namespace Habita.Application.Models.Emails;

public record AgentActivatedModel(string UserName) : IEmailModel;
