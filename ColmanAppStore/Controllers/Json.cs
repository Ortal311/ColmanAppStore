using System.Collections.Generic;

namespace ColmanAppStore.Controllers
{
    internal class Json
    {
        private Dictionary<string, int> sortedMap;

        public Json(Dictionary<string, int> sortedMap)
        {
            this.sortedMap = sortedMap;
        }
    }
}