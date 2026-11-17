using Habita.Domain.Common;

namespace Habita.Domain.Entities;

             
                                                                            
              
public class PropertyImprovement
{
    public int PropertyId { get; private set; }
    public int ImprovementId { get; private set; }

    public Property Property { get; private set; } = null!;
    public Improvement Improvement { get; private set; } = null!;

    private PropertyImprovement() { }

    internal PropertyImprovement(int propertyId, int improvementId)
    {
        PropertyId = propertyId;
        ImprovementId = improvementId;
    }
}
