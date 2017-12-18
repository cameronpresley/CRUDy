using System;

namespace CRUDy.Features.Exit
{
    public class Screen : Feature
    {
        public string Name => "Exit";

        public Action Workflow => () => Environment.Exit(0);
    }
}
