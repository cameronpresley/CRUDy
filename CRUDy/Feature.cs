using System;

namespace CRUDy
{
    public interface Feature
    {
        string Name { get; }
        Action Workflow { get; }
    }
}
