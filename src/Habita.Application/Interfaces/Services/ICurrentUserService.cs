using Habita.Domain.Enums;

namespace Habita.Application.Interfaces.Services;

             
                                                                                 
                          
              
public interface ICurrentUserService
{
                 
                                                              
                                     
                  
    string? UserId { get; }

                 
                                                           
                  
    string? UserName { get; }

                 
                                                                     
                  
    string? Email { get; }

                 
                                                                                         
                  
    string? FullName { get; }

                 
                                                                        
                  
    string? ProfilePicturePath { get; }

                 
                                                                        
                  
    bool IsAuthenticated { get; }

                 
                                                                
                                        
                  
    IReadOnlyCollection<string> Roles { get; }

                 
                                                                           
                                                                
                                                 
                 
              
                                                                                      
               
                  
                  
    bool IsInRole(string role);

                 
                                                                                            
                                                                                         
                  
    Task<bool> IsActiveAsync(CancellationToken cancellationToken = default);
}
