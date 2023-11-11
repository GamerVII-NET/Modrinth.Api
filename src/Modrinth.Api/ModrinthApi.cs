using Modrinth.Api.Core.Projects;
using Modrinth.Api.Core.System;

namespace Modrinth.Api
{
    public class ModrinthApi
    {
        private HttpClientFactory HttpClientFactory { get; }
        public Projects Projects { get; }
        public Mods Mods { get; }
        public Settings Settings { get; }
        public Versions Versions { get; }

        public ModrinthApi()
        {

            Settings = new Settings();

            HttpClientFactory = new HttpClientFactory(Settings.RequestTimeout);

            Projects = new Projects(this, HttpClientFactory);
            Mods = new Mods(this, HttpClientFactory);
            Versions = new Versions(this, HttpClientFactory);

        }

    }

}
