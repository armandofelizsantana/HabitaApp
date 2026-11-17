using Microsoft.AspNetCore.Identity;
using Habita.Domain.Common;

namespace Habita.Infrastructure.Identity.Entities;

             
                                                                                     
              
public class AppUser : IdentityUser
{
                                              
    public required string FirstName { get; set; }

                                                
    public required string LastName { get; set; }

                                                            
    public string? Phone { get; set; }

                 
                                                                            
                                                    
                  
    public void SetPhone(string? phone)
    {
        Phone = phone;
        PhoneNumber = phone;
    }

                                                                         
    public string? ProfileImage { get; set; }

                 
                                                               
                  
    public string? IdentityDocument { get; set; }

                                                                     
    public bool Active { get; set; } = true;

                                                       
    public void Activate()
    {
        Active = true;
        UpdatedAt = DomainTime.UtcNow;
    }

                                                          
    public void Deactivate()
    {
        Active = false;
        UpdatedAt = DomainTime.UtcNow;
    }

                                                          
    public DateTimeOffset CreatedAt { get; set; } = DomainTime.UtcNow;

                                                                      
    public DateTimeOffset? UpdatedAt { get; set; }

                                                                   
    public string GetDisplayName() => $"{FirstName} {LastName}".Trim();
}
