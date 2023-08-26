﻿using e_learning.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace e_learning.Models
{
    public class Enrollment
    {
        [Key]
        public int Id { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public string UserId { get; set; }
        public bool estado { get; set; }

        public ApplicationUser User { get; set; }
    }
}
