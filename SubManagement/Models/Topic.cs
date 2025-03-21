using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SubManagement.Models
{
    public class Topic
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TopicName { get; set; }


        public ICollection<SubTopic> Subtopics { get; set; } = new List<SubTopic>();
    }
}
