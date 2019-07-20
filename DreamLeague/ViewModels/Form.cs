using System.ComponentModel.DataAnnotations;

namespace DreamLeague.ViewModels
{
    public class Form
    {
        public int ManagerId { get; set; }

        public string Manager { get; set; }

        [Display(Name = "Form")]
        public string Text { get; set; }

        public int Value
        {
            get
            {
                int value = 0;

                foreach (var c in Text)
                {
                    switch (c)
                    {
                        case 'W':
                            value += 3;
                            break;
                        case 'D':
                            value += 1;
                            break;
                        default:
                            break;
                    }
                }

                return value;
            }
        }

        public Form() { }

        public Form(int managerId, string manager, string form)
        {
            ManagerId = managerId;
            Manager = manager;
            Text = form;
        }
    }
}