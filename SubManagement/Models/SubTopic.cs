using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SubManagement.Models
{
    public class SubTopic
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string SubtopicName { get; set; }

        [Required]
        [ForeignKey("Topic")]
        public int TopicId { get; set; }

        public Topic Topic { get; set; }
    }
}
