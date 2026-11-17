namespace Habita.Application.Models.Emails;

             
                                                                                                
              
public sealed record AgentActivatedNoticeModel(string FullName, string LoginUrl, int CurrentYear)
    : IEmailModel;
