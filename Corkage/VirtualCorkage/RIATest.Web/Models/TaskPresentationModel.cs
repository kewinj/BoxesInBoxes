using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ServiceModel.DomainServices.Server;

namespace RIATest.Web.Models
{
    /// <summary>
    /// Responsibility is to handle Task realted data (DTO), and adds a level of abstraction from the db
    /// </summary>
    [Serializable]
    public class TaskPresentationModel
    {
        [Key]
        [Display(AutoGenerateField=false)]
        public int TaskId { get; set; }
        
        [Required]
        [Display(AutoGenerateField = false)]
        public int StoryId { get; set; }
        
        [Required]
        [Display(AutoGenerateField = false)]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name="Task Description")]
        public string Description { get; set; }

    }

    /// <summary>
    /// Responsibility is to handle Category/Task realted data (DTO), and adds a level of abstraction from the db
    /// </summary>
    [Serializable]
    public class CategoryTaskPresentationModel
    {
        [Key]
        [Required]
        [Display(AutoGenerateField = false)]
        public int CategoryId { get; set; }

        [Required]
        [Display(AutoGenerateField = false)]
        public int StoryId { get; set; }

        [Required]
        [Display(Name = "Task Category")]
        public string Category { get; set; }
 
        [Include]
        [Association("CategoryTaskPresentationModel_Task", "CategoryId", "CategoryId")]
        public IEnumerable<TaskPresentationModel> Tasks { get; set; } 

    }

    
    public class StoryPM
    {
        [Key]
        [Required]
        [Display(AutoGenerateField = false)]
        public int StoryId { get; set; }

        [Required]
        [Display(Name = "Story Description")]
        public string Description { get; set; }

        [Required]
        [Display(AutoGenerateField = false)]
        public int SprintId { get; set; }

        [Include]
        [Association("StoryPM_CategoryTaskPresentationModel", "StoryId", "StoryId")]
        public IEnumerable<CategoryTaskPresentationModel> CategoryTasks { get; set; }

    }
}