using System.Collections.Generic;

namespace DomaciZadatakMVC.Models
{
    public class  AdminViewModel 
    {
        public Project Projects { get; set; }
        public User Users { get; set; }

        public Task Tasks { get; set; }

        //
        public IEnumerable<Project> Pro { get; set; }
        public IEnumerable<Task> TaskE { get; set; }

        public List<double> Avge { get; set; }

    }
}