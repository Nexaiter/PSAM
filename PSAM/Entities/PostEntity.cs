﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PSAM.Entities
{
    public class PostEntity
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public AccountEntity Author { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Likes { get; set; }

        public virtual ICollection<CommentEntity> Comments { get; set; }
    }
}
