using System;

namespace CRUDy.Exit
{
    public class Screen
    {
        public Feature Create()
        {
            return new Feature("Exit", () => Environment.Exit(0));
        }
    }
}
