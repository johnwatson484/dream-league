namespace DreamLeague.ViewModels
{
    public class Search
    {
        public string Label { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string Id { get; set; }

        public string Url
        {
            get
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    return string.Format("/{0}/{1}/{2}", Controller, Action, Id);
                }
                else
                {
                    return string.Format("/{0}/{1}", Controller, Action);
                }
            }
        }

        public Search() { }

        public Search(string label, string controller, string action, string id = null)
        {
            Label = label;
            Controller = controller;
            Action = action;
            Id = id;
        }
    }
}